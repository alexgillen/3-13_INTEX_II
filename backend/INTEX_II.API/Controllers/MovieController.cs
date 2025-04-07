using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mission11.API.Data;

namespace Mission11.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private MovieDbContext _movieContext;
        public MovieController(MovieDbContext temp)
        {
            _movieContext = temp;
        }

        [HttpGet("GetMovies")]
        public IActionResult Get(int pageSize = 10, int pageNum = 1, [FromQuery] List<string>? genres = null)
        {
            var query = _movieContext.Movies.AsQueryable();

            //Okay when we come back to filtering. We are getting categories passed into the route and we need to do someting with it. 
            //That's what the code below this is for, rn it does nothing. 
            if (genres != null && genres.Any())
            {
                query = query.Where(m => genres.Any(g => !string.IsNullOrEmpty(m.genres) && m.genres.Contains(g)));
            }

            var totalNumMovies = query.Count();

            var something = query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var someObject = new
            {
                Movies = something,
                TotalNumMovies = totalNumMovies
            };

            return Ok(someObject);
        }

        [HttpGet("GetGenres")]
        public IActionResult GetGenres()
        {
            var genreList = _movieContext.Movies
                .Where(m => m.genres != null)
                .AsEnumerable()
                .SelectMany(m => m.genres.Split(new[] { ',' }, StringSplitOptions.None))
                .Select(g => g.Trim())
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            return Ok(genreList);
}

        [HttpPost("AddMovie")]
        public IActionResult AddMovie([FromBody] Movie newMovie)
        {
           _movieContext.Movies.Add(newMovie);
           _movieContext.SaveChanges();
           return Ok(newMovie);
        }

        // [HttpPut("UpdateMovie/{show_id}")]
        // public IActionResult UpdateMovie(int show_id, [FromBody] Movie updatedMovie)
        // {
        //    var existingMovie = _movieContext.Movies.Find(show_id);

        //    existingMovie.type = updatedMovie.type;
        //    existingMovie.Description = updatedMovie.Description;
        //    existingMovie.Poster = updatedMovie.Poster;
        //    existingMovie.Rating = updatedMovie.Rating;
        //    existingMovie.Category = updatedMovie.Category;
        //    existingMovie.Price = updatedMovie.Price;

        //    _movieContext.Movies.Update(existingMovie);
        //    _movieContext.SaveChanges();

        //    return Ok(existingMovie);
        // }

        [HttpPut("UpdateMovie/{show_id}")]
        public IActionResult UpdateMovie(string show_id, [FromBody] Movie updatedMovie)
        {
            var existingMovie = _movieContext.Movies.Find(show_id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            // Basic fields
            existingMovie.type = updatedMovie.type;
            existingMovie.title = updatedMovie.title;
            existingMovie.director = updatedMovie.director;
            existingMovie.cast = updatedMovie.cast;
            existingMovie.country = updatedMovie.country;
            existingMovie.release_year = updatedMovie.release_year;
            existingMovie.rating = updatedMovie.rating;
            existingMovie.duration = updatedMovie.duration;
            existingMovie.description = updatedMovie.description;
            existingMovie.genres = updatedMovie.genres;

            _movieContext.Movies.Update(existingMovie);
            _movieContext.SaveChanges();

            return Ok(existingMovie);
        }

        [HttpDelete("DeleteMovie/{show_id}")]
        public IActionResult DeleteMovie(string show_id)
        {
           var movie = _movieContext.Movies.Find(show_id);

           if (movie == null)
           {
               return NotFound(new {message = "Movie not found" });
           }

           _movieContext.Movies.Remove(movie);
           _movieContext.SaveChanges();

           return NoContent();
        }
    }
}

