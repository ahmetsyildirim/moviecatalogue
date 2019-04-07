using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace MovieService
{
    internal interface IFilmWorldRestClient
    {
        [Get("/movies")]
        Task<string> Movies([Header("x-access-token")] string token);

        [Get("/movie/{id}")]
        Task<string> Movie(string id, [Header("x-access-token")] string token);
    }
}
