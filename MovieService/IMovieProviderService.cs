using MovieModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieService
{
    public interface IMovieProviderService
    {
        Task<List<MovieDetails>> GetMovies();
    }
}
