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
        public IActionResult Get(int pageSize = 10, int pageNum = 1, [FromQuery] List<string>? categories = null)
        {
            var query = _movieContext.Movies.AsQueryable();

            //Okay when we come back to filtering. We are getting categories passed into the route and we need to do someting with it. 
            //That's what the code below this is for, rn it does nothing. 

            //if (categories != null && categories.Any())
            //{
            //    query = query.Where(m => categories.Contains(m.description));
            //}

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

        //[HttpGet("GetCategories")]
        //public IActionResult GetCategories()
        //{
        //    var categories = _movieContext.movies_titles
        //        .Select(b => b.Category)
        //        .Distinct()
        //        .ToList();

        //    return Ok(categories);
        //}

        //[HttpPost("AddBook")]
        //public IActionResult AddBook([FromBody] Book newBook)
        //{
        //    _bookContext.Books.Add(newBook);
        //    _bookContext.SaveChanges();
        //    return Ok(newBook);
        //}

        //[HttpPut("UpdateBook/{bookId}")]
        //public IActionResult UpdateBook(int bookId, [FromBody] Book updatedBook)
        //{
        //    var existingBook = _bookContext.Books.Find(bookId);

        //    existingBook.Title = updatedBook.Title;
        //    existingBook.Author = updatedBook.Author;
        //    existingBook.Publisher = updatedBook.Publisher;
        //    existingBook.ISBN = updatedBook.ISBN;
        //    existingBook.Classification = updatedBook.Classification;
        //    existingBook.Category = updatedBook.Category;
        //    existingBook.PageCount = updatedBook.PageCount;
        //    existingBook.Price = updatedBook.Price;

        //    _bookContext.Books.Update(existingBook);
        //    _bookContext.SaveChanges();

        //    return Ok(existingBook);
        //}

        //[HttpDelete("DeleteBook/{bookId}")]
        //public IActionResult DeleteBook(int bookId)
        //{
        //    var book = _bookContext.Books.Find(bookId);

        //    if (book == null)
        //    {
        //        return NotFound(new {message = "Book not found" });
        //    }

        //    _bookContext.Books.Remove(book);
        //    _bookContext.SaveChanges();

        //    return NoContent();
        //}
    }
}

