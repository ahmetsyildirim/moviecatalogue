using MovieModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieService
{
    public interface IFilmWorldService
    {
        Task<List<MovieDetails>> GetMovieList();

        Task<MovieDetails> GetMovieDetails(string movieId);

        Task<IEnumerable<Movie>> GetMovies();
    }
}
