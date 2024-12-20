using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LSC.AzureFunctions.Examples
{
    public class MovieSearch
    {
        private readonly ILogger<MovieSearch> _logger;

        public MovieSearch(ILogger<MovieSearch> logger)
        {
            _logger = logger;
        }


        private static readonly List<Movie> Movies = new List<Movie>
    {
        new Movie { Id = 1, Title = "The Shawshank Redemption", Year = 1994 },
        new Movie { Id = 2, Title = "The Godfather", Year = 1972 },
        new Movie { Id = 3, Title = "The Dark Knight", Year = 2008 },
        new Movie { Id = 4, Title = "Pulp Fiction", Year = 1994 },
        new Movie { Id = 5, Title = "Inception", Year = 2010 },
        new Movie { Id = 2, Title = "The Dark Knight", Year = 2008, Genre = "Action", Rating = 9 },
    new Movie { Id = 3, Title = "Mankatha", Year = 2011, Genre = "Crime", Rating = 8 },
    new Movie { Id = 4, Title = "Asuran", Year = 2019, Genre = "Drama", Rating = 9 },
    new Movie { Id = 5, Title = "Thuppakki", Year = 2012, Genre = "Action", Rating = 8 },
    new Movie { Id = 6, Title = "Master", Year = 2021, Genre = "Action", Rating = 8 },
    new Movie { Id = 7, Title = "Aramm", Year = 2017, Genre = "Drama", Rating = 9 },
    new Movie { Id = 8, Title = "Black Panther", Year = 2018, Genre = "Action", Rating = 8 },
    new Movie { Id = 9, Title = "Drishyam", Year = 2013, Genre = "Thriller", Rating = 8 },
    new Movie { Id = 10, Title = "Vikram Vedha", Year = 2017, Genre = "Action", Rating = 9 },
    new Movie { Id = 11, Title = "Captain America: The Winter Soldier", Year = 2014, Genre = "Action", Rating = 8 },
    new Movie { Id = 12, Title = "Soorarai Pottru", Year = 2020, Genre = "Drama", Rating = 9 }
    };
        [Function("GetMovieRecommendations")]
        public static async Task<IActionResult> GetMovieRecommendations(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var genre = req.Query["genre"];
            var year = req.Query["year"];
            var rating = req.Query["rating"];

            var recommendations = new List<Movie>
    {
        new Movie() { Title = "Inception", Genre = "Sci-Fi", Year = 2010, Rating = 9  },
        new Movie() { Title = "Interstellar", Genre = "Sci-Fi", Year = 2014, Rating = 8 },
        new Movie()  { Title = "The Godfather", Genre = "Crime", Year = 1972, Rating = 9 },
        new Movie() { Title = "Inception", Genre = "Sci-Fi", Year = 2010, Rating = 9 },
    new Movie() { Title = "Interstellar", Genre = "Sci-Fi", Year = 2014, Rating = 8 },
    new Movie() { Title = "The Godfather", Genre = "Crime", Year = 1972, Rating = 9 },
    new Movie() { Title = "Baahubali: The Beginning", Genre = "Action", Year = 2015, Rating = 8 },
    new Movie() { Title = "Visaranai", Genre = "Crime", Year = 2015, Rating = 9 },
    new Movie() { Title = "Super Deluxe", Genre = "Drama", Year = 2019, Rating = 8 },
    new Movie() { Title = "Parasite", Genre = "Thriller", Year = 2019, Rating = 9 },
    new Movie() { Title = "KGF: Chapter 1", Genre = "Action", Year = 2018, Rating = 8 },
    new Movie() { Title = "Avengers: Endgame", Genre = "Sci-Fi", Year = 2019, Rating = 9 },
    new Movie() { Title = "Jai Bhim", Genre = "Drama", Year = 2021, Rating = 9 }
    };

            var filtered = recommendations
          .Where(movie =>
              (string.IsNullOrEmpty(genre) || movie.Genre == genre) &&
              (string.IsNullOrEmpty(year) || movie.Year == int.Parse(year)) &&
              (string.IsNullOrEmpty(rating) || movie.Rating >= int.Parse(rating)))
          .ToList();

            return new OkObjectResult(filtered);
        }


        [Function("SearchMovies")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies")] HttpRequest req,
            ILogger log)
        {
            string query = req.Query["search"];

            if (string.IsNullOrEmpty(query))
            {
                return new BadRequestObjectResult("Please provide a search query.");
            }

            var results = Movies
                .Where(m => m.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return new OkObjectResult(results);
        }

        private class Movie
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }
            public decimal Rating { get; set; }
        }
    }
}
