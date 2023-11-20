﻿namespace FU.API.Controllers
{
    using FU.API.DTOs.Tag;
    using FU.API.Helpers;
    using FU.API.Interfaces;
    using FU.API.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly AccountsService _accountsService;

        public TagsController(ITagService tagService, AccountsService accountsService)
        {
            _tagService = tagService;
            _accountsService = accountsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagRequestDTO dto)
        {
            var user = await _accountsService.GetCurrentUser(User);

            if (user is null)
            {
                return Unauthorized("User is not signed in");
            }

            var tagName = dto.Name;
            var tag = await _tagService.GetTag(tagName);

            if (tag is not null)
            {
                return BadRequest("Tag already exists");
            }

            tag = await _tagService.CreateTag(tagName);

            return await GetTag(tag.Id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTags([FromQuery] string tagName)
        {
            var tags = await _tagService.GetTags(tagName);

            var response = tags.ToDtos();

            return Ok(response);
        }

        [HttpGet]
        [Route("{tagId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTag(int tagId)
        {
            var tag = await _tagService.GetTag(tagId);

            if (tag is null)
            {
                return NotFound("Tag not found");
            }

            var response = tag.ToDto();

            return Ok(response);
        }

        [HttpPatch]
        [Route("{tagId}")]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody] TagRequestDTO dto)
        {
            var user = await _accountsService.GetCurrentUser(User);

            if (user is null || !user.IsAdmin)
            {
                return Unauthorized("User is not authorized");
            }

            var tag = await _tagService.GetTag(tagId);

            if (tag is null)
            {
                return NotFound("Tag not found");
            }

            tag.Name = dto.Name;

            var updatedTag = await _tagService.UpdateTag(tag);

            var response = updatedTag.ToDto();

            return Ok(response);
        }

        [HttpDelete]
        [Route("{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            var user = await _accountsService.GetCurrentUser(User);

            if (user is null || !user.IsAdmin)
            {
                return Unauthorized("User is not authorized");
            }

            var tag = await _tagService.GetTag(tagId);

            if (tag is null)
            {
                return NotFound("Tag not found");
            }

            await _tagService.DeleteTag(tag);

            return NoContent();
        }
    }
}
