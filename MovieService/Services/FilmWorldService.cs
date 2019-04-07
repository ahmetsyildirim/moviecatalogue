using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Polly;
using MovieModel;

namespace MovieService
{
    public class FilmWorldService : IFilmWorldService
    {
        private readonly string _endpoint;
        private readonly string _token;
        private readonly string _provider;

        public FilmWorldService(string endpoint, string token, string provider)
        {
            _endpoint = endpoint;
            _token = token;
            _provider = provider;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(100));

                var filmworldService = RestService.For<IFilmWorldRestClient>(new System.Net.Http.HttpClient { BaseAddress = new Uri(_endpoint), Timeout = TimeSpan.FromSeconds(5) });
                string filmworldResults = "";

                await retryPolicy.ExecuteAsync(async () =>
                {
                    filmworldResults = await filmworldService.Movies(_token);

                });
                return JsonConvert.DeserializeObject<MovieCollection>(filmworldResults).Movies.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MovieDetails> GetMovieDetails(string movieId)
        {
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(50));

            try
            {
                var filmworldService = RestService.For<IFilmWorldRestClient>(new System.Net.Http.HttpClient { BaseAddress = new Uri(_endpoint), Timeout = TimeSpan.FromSeconds(5) });
                string result = "";
                await retryPolicy.ExecuteAsync(async () =>
                {
                    result = await filmworldService.Movie(movieId,_token);

                });
                var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(result);
                movieDetails.Provider = _provider; 
                return movieDetails;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<MovieDetails>> GetMovieList()
        {
            try
            {
                var movies = await GetMovies();
                var tasks = movies.Select(m => GetMovieDetails(m.ID));

                var results = await Task.WhenAll(tasks);

                return results.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
