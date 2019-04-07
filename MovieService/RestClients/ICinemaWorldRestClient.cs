using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieService
{
    internal interface ICinemaWorldRestClient
    {
        [Get("/movies")]
        Task<string> Movies([Header("x-access-token")] string token);

        [Get("/movie/{id}")]
        Task<string> Movie(string id, [Header("x-access-token")] string token);
    }
}
