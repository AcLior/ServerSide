using Microsoft.AspNetCore.Mvc;
using HW1.BL;
using HW1.DAL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // GET: api/<MoviesController>
        [HttpGet]
        public IEnumerable<Movies> Get()
        {
            return null;
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        // POST api/<MoviesController>
        [HttpPost]
        public IActionResult Post([FromBody] Movies movie)
        {
            if (movie.InsertMovie())
            {
                return Ok(new { message = "Movie inserted successfully." });
            }

            return Conflict("❌ Failed to insert movie.");
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movies movie)
        {
            if (movie.UpdateMovie(id))
                return Ok(new { message = "Movie updated successfully." });

            return NotFound(new { message = "Movie not found." });
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (new Movies().DeleteMovie(id))
                return Ok(new { message = "Movie deleted successfully." });

            return NotFound(new { message = "Movie not found." });
        }

        // POST api/Movies/batch
        [HttpPost("batch")]
        public IActionResult InsertMoviesBatch([FromBody] List<Movies> moviesList)
        {
            if (moviesList == null || moviesList.Count == 0)
                return BadRequest("No movies provided.");
            Movies movie = new Movies();
            
            int successCount = movie.InsertMoviesList(moviesList);
            return Ok(new { message = $"{successCount} movies inserted successfully." });
        }

        // POST api/Movies/rent

        [HttpPost("rent")]
        public IActionResult RentMovie([FromBody] RentRequest rentRequest)
        {
           
            bool success = new Movies().RentMovie(rentRequest);

            if (success)
                return Ok(new { message = "Movie rented successfully!" });

            return BadRequest(new { message = "❌ Failed to rent movie." });
        }

        // GET api/Movies/rented/{userId}
        [HttpGet("rented/{userId}")]
        public IActionResult GetRentedMovies(int userId)
        {
            var movie = new Movies();
            var rentedMovies = movie.GetRentedMovies(userId);

            if (rentedMovies == null || !rentedMovies.Any())
            {
                return NotFound(new { message = "No currently rented movies found for this user." });
            }

            return Ok(rentedMovies);
        }


        // GET api/Movies/filter
        [HttpGet("filter")]
        public IActionResult GetMoviesByFilters(
                [FromQuery] string? title = "",
                [FromQuery] DateTime? startDate = null,
                [FromQuery] DateTime? endDate = null,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 9)
        {
            Movies m = new Movies();
            int totalCount;
            var movies = m.GetMoviesFiltered(title, startDate, endDate, page, pageSize, out totalCount);

            return Ok(new
            {
                movies = movies,
                totalCount = totalCount
            });
        }

        // POST api/Movies/transfer

        [HttpPost("transfer")]
        public IActionResult TransferMovie([FromBody] TransferRequest req)
        {
            Movies movie = new Movies();
            bool result = movie.TransferRentedMovie(req.FromUserId, req.ToUserId, req.MovieId);

            if (result)
                return Ok(new { message = "Movie transferred successfully" });
            return BadRequest(new { message = "Failed to transfer movie" });
        }


    }
}
