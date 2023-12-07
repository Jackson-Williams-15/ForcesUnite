import * as React from 'react';
import {
  Button,
  TextField,
  Box,
  Container,
  Typography,
  CssBaseline,
  Grid,
} from '@mui/material';
import { TextareaAutosize } from '@mui/base/TextareaAutosize';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Radio from '@mui/material/Radio';

export default function CreatePost() {
  return (
    <ThemeProvider theme={createTheme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 1,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Typography component="h1" variant="h5">
            Create Post
          </Typography>
          <Box
            component="form"
            noValidate
            //onSubmit={handleSubmit}
            sx={{ mt: 3 }}
          >
            <Grid container spacing={2}>
              <Box
                sx={{
                  display: 'flex',
                  marginLeft: 2,
                }}
              >
                <Typography component="h1" variant="h6">
                  Post Title
                </Typography>
              </Box>
              <Grid item xs={12}>
                <TextField
                  required //may want to get rid of this and just check if it's empty when clicking create button.
                  fullWidth
                  id="searchGames"
                  label="Search Games" //might want to put a Search icon in front, if we can figure it out.
                  type="searchGames"
                  name="searchGames"
                  autofocus
                />
              </Grid>
              <Box
                sx={{
                  marginTop: 3,
                  display: 'flex',
                  marginLeft: 2,
                }}
              >
                <Typography component="h1" variant="h6">
                  Time:
                </Typography>
              </Box>
              <Grid item xs={12} sm={4}>
                <TextField name="startTime" id="startTime" label="Start Time" />
              </Grid>
              <Box
                sx={{
                  marginLeft: 2,
                  marginTop: 3,
                  display: 'flex',
                }}
              >
                <Typography component="h1" variant="h6">
                  and
                </Typography>
              </Box>
              <Grid item xs={12} sm={4}>
                <TextField
                  fullWidth
                  id="endTime"
                  label="End Time"
                  name="endTime"
                />
              </Grid>
              <Box
                sx={{
                  marginTop: 3,
                  display: 'flex',
                  marginLeft: 2,
                  marginRight: 2.5,
                }}
              >
                <Typography component="h1" variant="h6">
                  Hashtags
                </Typography>
              </Box>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  id="searchHashtags"
                  label="Search Hashtags"
                  name="searchHashtags"
                  autoComplete="searchHashtags"
                />{' '}
                {/* Need to put checkboxes under the hashtag section, which will display*/}
              </Grid>
              <Box
                sx={{
                  marginTop: 3,
                  display: 'flex',
                  marginLeft: 2,
                  marginRight: 2.5,
                }}
              >
                <Typography component="h1" variant="h6">
                  {' '}
                  {/* Need to have 2 radius buttons below for 'Any' and 'Between' */}
                  Description
                </Typography>
              </Box>
              <Grid item xs={12} sm={6} marginTop={2}>
                <TextareaAutosize
                  aria-setsize={300}
                  size={500}
                ></TextareaAutosize>
              </Grid>
            </Grid>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Create Post
            </Button>
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}
//removed brace here
