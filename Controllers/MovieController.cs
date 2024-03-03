using Microsoft.AspNetCore.Mvc;
using MovieApi.Helpers;
using MovieApi.Models;
using MovieApi.Policies;
using Newtonsoft.Json;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;

        private readonly ClientPolicy _clientPolicy;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public MovieController(ILogger<MovieController> logger, ClientPolicy clientPolicy, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _logger = logger;
            _clientPolicy = clientPolicy;
        }

        [HttpGet("getmovies")]
        public async Task<ActionResult> GetMovies()
        {            
            var apiHelper = new ApiHelpers();

            var client = _clientFactory.CreateClient("Fetch");

            string apiKey = _configuration["ProviderSettings:ApiKey"];

            client.DefaultRequestHeaders.Add("x-access-token", apiKey);

            string cinemaWorldBaseUrl = _configuration["ProviderSettings:ProviderBaseUrls:CinemaWorld"];
            string filmWorldBaseUrl =  _configuration["ProviderSettings:ProviderBaseUrls:FilmWorld"];
            
            HttpResponseMessage cinemaWorldResponse = await client.GetAsync(cinemaWorldBaseUrl + "movies");
            HttpResponseMessage filmWorldResponse = await client.GetAsync(filmWorldBaseUrl + "movies");

            if(filmWorldResponse.IsSuccessStatusCode && cinemaWorldResponse.IsSuccessStatusCode)
            {
                string cinemaWorldJson = cinemaWorldResponse.Content.ReadAsStringAsync().Result;
                string filmWorldJson = filmWorldResponse.Content.ReadAsStringAsync().Result;                

                List<MovieList> filmWorldMovieList = apiHelper.GetMoviesFromProvider(filmWorldJson);
                List<MovieList> cinemaWorldMovieList  = apiHelper.GetMoviesFromProvider(cinemaWorldJson);

                List<MovieList> combinedMovieList = filmWorldMovieList.Concat(cinemaWorldMovieList).ToList();

                Console.WriteLine("Successful response!");
                return Ok(combinedMovieList);
            }
            Console.WriteLine("Failed response!");
            return StatusCode(StatusCodes.Status500InternalServerError);        
        }

        [HttpPost("getcheapestmovie")]
        public async Task<ActionResult> GetCheapestMovie()
        { 
            var apiHelper = new ApiHelpers();

            var client = _clientFactory.CreateClient("Fetch");

            string apiKey = _configuration["ProviderSettings:ApiKey"];

            client.DefaultRequestHeaders.Add("x-access-token", apiKey);

            string cinemaWorldBaseUrl = _configuration["ProviderSettings:ProviderBaseUrls:CinemaWorld"];
            string filmWorldBaseUrl =  _configuration["ProviderSettings:ProviderBaseUrls:FilmWorld"];

            string movieIDsFromRequest = Request.Form["movieIDs"];

            List<string> movieIDList = apiHelper.getStringList(movieIDsFromRequest);

            float  cheapestPriceSoFar = float.MaxValue;
            Movie cheapestPriceMovie = new Movie();

            foreach(var movieID in movieIDList)
            {
                if(movieID.StartsWith("cw")) 
                {
                    HttpResponseMessage cinemaWorldResponse = await client.GetAsync(cinemaWorldBaseUrl + "movie/" + movieID);

                    string cinemaWorldJson = cinemaWorldResponse.Content.ReadAsStringAsync().Result;
                    
                    Movie movie = JsonConvert.DeserializeObject<Movie>(cinemaWorldJson);

                    if (cheapestPriceSoFar > float.Parse(movie.Price))
                    {
                        cheapestPriceSoFar = float.Parse(movie.Price);
                        cheapestPriceMovie = new Movie();
                        cheapestPriceMovie = apiHelper.DeepClone(movie);
                    }                    
                } else {
                    HttpResponseMessage filmWorldResponse = await client.GetAsync(filmWorldBaseUrl + "movie/" + movieID);

                    string filmWorldJson = filmWorldResponse.Content.ReadAsStringAsync().Result;
                    
                    Movie movie = JsonConvert.DeserializeObject<Movie>(filmWorldJson);

                    if (cheapestPriceSoFar > float.Parse(movie.Price))
                    {
                        cheapestPriceSoFar = float.Parse(movie.Price);
                        cheapestPriceMovie = new Movie();
                        cheapestPriceMovie = apiHelper.DeepClone(movie);
                    }  
                }                
            }

            return Ok(cheapestPriceMovie);
        }
    }
}