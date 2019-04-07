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
    public class CinemaWorldService : ICinemaWorldService
    {
        private readonly string _endpoint;
        private readonly string _token;
        private readonly string _provider;

        public CinemaWorldService(string endpoint, string token, string provider)
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

                var service = RestService.For<ICinemaWorldRestClient>(new System.Net.Http.HttpClient { BaseAddress = new Uri(_endpoint), Timeout = TimeSpan.FromSeconds(5) });
                
                string result = "";

                await retryPolicy.ExecuteAsync(async () =>
                {
                    result = await service.Movies(_token);

                });
                return JsonConvert.DeserializeObject<MovieCollection>(result).Movies.ToList();
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
                var service = RestService.For<ICinemaWorldRestClient>(new System.Net.Http.HttpClient { BaseAddress = new Uri(_endpoint), Timeout = TimeSpan.FromSeconds(5) });
                string result = "";
                await retryPolicy.ExecuteAsync(async () =>
                {
                    result = await service.Movie(movieId,_token);

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
