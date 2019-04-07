using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieModel;
using MovieService;

namespace MovieCatalogue.Web.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        IMovieProviderService _service;

        public MovieController(IMovieProviderService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<MovieDetails>> MovieList()
        {

            var result = await _service.GetMovies();

            return result;
        }
    }
}
