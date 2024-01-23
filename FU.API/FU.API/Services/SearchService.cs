namespace FU.API.Services;

using System.Linq.Expressions;
using FU.API.Data;
using FU.API.Interfaces;
using FU.API.Models;
using Microsoft.EntityFrameworkCore;
using LinqKit;

public class SearchService : CommonService, ISearchService
{
    private readonly AppDbContext _dbContext;

    public SearchService(AppDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Post>> SearchPosts(PostQuery query)
    {
        var dbQuery = _dbContext.Posts.Select(p => p);

        // Filters are addded one at a time, generally by the amount of posts they filter out

        // Filter by games
        if (query.GameIds.Count > 0)
        {
            dbQuery = dbQuery.Where(p => query.GameIds.Contains(p.Game.Id));
        }

        // Filter by tags
        // A post must have every tag in the filter
        foreach (var tagId in query.TagIds)
        {
            dbQuery = dbQuery.Where(p => p.Tags.Any(tr => tr.TagId == tagId));
        }

        // Filter by posts after a time
        if (query.After is not null)
        {
            dbQuery = dbQuery.Where(p => p.StartTime >= query.After);
        }

        // Filter by required players
        if (query.MinimumRequiredPlayers > 0)
        {
            // TODO
        }

        // Filter by search keywords
        dbQuery = dbQuery.Where(DescriptionContains(query.DescriptionContains));

        // Sort results
        IOrderedQueryable<Post> orderedDbQuery = query.SortBy?.Direction == SortDirection.Ascending
            ? dbQuery.OrderBy(
                SelectPostProperty(query.SortBy?.Type))
            : dbQuery.OrderByDescending(
                SelectPostProperty(query.SortBy?.Type));

        // Always end ordering by Id to ensure order is unique. This ensures order is consistent across calls.
        orderedDbQuery = orderedDbQuery.ThenBy(p => p.Id);

        return await orderedDbQuery
                .Skip(query.Offset)
                .Take(query.Limit)
                .Include(p => p.Creator)
                .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Game)
                .ToListAsync();
    }

    private static Expression<Func<Post, bool>> DescriptionContains(List<String> keywords)
    {
        if (keywords.Count == 0) return PredicateBuilder.New<Post>(true); // nothing to do so return a true predicate

        var predicate = PredicateBuilder.New<Post>(false); // create a predicate that's false by default
        foreach (string keyword in keywords)
        {
            predicate = predicate.Or(p => p.Description.Contains(keyword));
        }
        return predicate;
    }

    private static Expression<Func<Post, object>> SelectPostProperty(SortType? sortType)
    {
        return sortType switch
        {
            SortType.NewestCreated => (post) => post.CreatedAt,
            SortType.Title => (post) => post.Title,
            SortType.EarliestToScheduledTime => (post) => post.StartTime ?? post.CreatedAt,
            _ => (post) => post.CreatedAt,
        };
    }
}
