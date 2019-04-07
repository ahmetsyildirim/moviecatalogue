using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using MovieModel;

namespace MovieService
{
    public class MovieProviderService : IMovieProviderService
    {
        private readonly ICinemaWorldService _cinemaWorldService;
        private readonly IFilmWorldService _filmWorldService;

        public MovieProviderService(ICinemaWorldService cinemaWorldService, IFilmWorldService filmWorldService)
        {
            _cinemaWorldService = cinemaWorldService;
            _filmWorldService = filmWorldService;
        }

        public async Task<List<MovieDetails>> GetMovies()
        {
            List<MovieDetails> cinemaworldmovieList = new List<MovieDetails>();
            List<MovieDetails> filmworldMovieList = new List<MovieDetails>();

            try
            {
                try
                {
                    cinemaworldmovieList = await _cinemaWorldService.GetMovieList();
                }
                catch (Exception)
                {
                    //Log
                }


                try
                {
                    filmworldMovieList = await _filmWorldService.GetMovieList();
                }
                catch (Exception)
                {
                    //Log
                }


                var entireList = cinemaworldmovieList.Concat(filmworldMovieList)
                     .GroupBy(m => m.Title)
                     .Select(g => g.OrderBy(x => x.Price).First())
                     .ToList();

                return entireList;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
