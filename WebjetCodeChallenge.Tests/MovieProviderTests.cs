using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieModel;
using MovieService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebjetCodeChallenge.Tests
{
    [TestClass]
    public class MovieProviderTests
    {
        //Mock<IMovieProviderService> mockMovieProviderService;

        Mock<ICinemaWorldService> mockCinemaWorldService;
        Mock<IFilmWorldService> mockFilmWorldService;

        

        [TestMethod]
        public async Task ShouldProviderReturnTheCheapestPrice()
        {
            decimal cheapestPrice = 27;

            List<MovieDetails> cinemaWorldMovies = new List<MovieDetails>();
            cinemaWorldMovies.Add(new MovieDetails { ID = "cw123", Title = "LOTR 1", Price = cheapestPrice });

            mockCinemaWorldService = new Mock<ICinemaWorldService>(MockBehavior.Strict);
            mockCinemaWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(cinemaWorldMovies);


            List<MovieDetails> filmWorldMovies = new List<MovieDetails>();
            filmWorldMovies.Add(new MovieDetails { ID = "fw789", Title = "LOTR 1", Price = 99 });

            mockFilmWorldService = new Mock<IFilmWorldService>(MockBehavior.Strict);
            mockFilmWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(filmWorldMovies);




            MovieProviderService movieProviderService = new MovieProviderService(mockCinemaWorldService.Object, mockFilmWorldService.Object);

            var movieList = await movieProviderService.GetMovies();

            Assert.AreEqual(movieList.FirstOrDefault().Price,cheapestPrice);
        }

        [TestMethod]
        public async Task ShouldProviderReturnTheCheapestMovieId()
        {
            var cheapestMovieid = "fw789";

            List<MovieDetails> cinemaWorldMovies = new List<MovieDetails>();
            cinemaWorldMovies.Add(new MovieDetails { ID = "cw123", Title = "LOTR 2", Price = 202 });

            mockCinemaWorldService = new Mock<ICinemaWorldService>(MockBehavior.Strict);
            mockCinemaWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(cinemaWorldMovies);


            List<MovieDetails> filmWorldMovies = new List<MovieDetails>();
            filmWorldMovies.Add(new MovieDetails { ID = "fw789", Title = "LOTR 2", Price = 99 });

            mockFilmWorldService = new Mock<IFilmWorldService>(MockBehavior.Strict);
            mockFilmWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(filmWorldMovies);


            MovieProviderService movieProviderService = new MovieProviderService(mockCinemaWorldService.Object, mockFilmWorldService.Object);

            var movieList = await movieProviderService.GetMovies();

            Assert.AreEqual(movieList.FirstOrDefault().ID, cheapestMovieid);
        }

        [TestMethod]
        public async Task ShouldProviderMergeOverlappingMovies()
        {

            List<MovieDetails> cinemaWorldMovies = new List<MovieDetails>();
            cinemaWorldMovies.Add(new MovieDetails { ID = "cw123", Title = "HOBBIT 1", Price = 75 });
            cinemaWorldMovies.Add(new MovieDetails { ID = "cw955", Title = "HOBBIT 2", Price = 96 });

            mockCinemaWorldService = new Mock<ICinemaWorldService>(MockBehavior.Strict);
            mockCinemaWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(cinemaWorldMovies);


            List<MovieDetails> filmWorldMovies = new List<MovieDetails>();
            filmWorldMovies.Add(new MovieDetails { ID = "fw789", Title = "HOBBIT 1", Price = 41 });
            filmWorldMovies.Add(new MovieDetails { ID = "fw456", Title = "The Shining", Price = 17 });

            mockFilmWorldService = new Mock<IFilmWorldService>(MockBehavior.Strict);
            mockFilmWorldService.Setup(s => s.GetMovieList()).ReturnsAsync(filmWorldMovies);


            MovieProviderService movieProviderService = new MovieProviderService(mockCinemaWorldService.Object, mockFilmWorldService.Object);

            var movieList = await movieProviderService.GetMovies();

            Assert.AreEqual(movieList.Count(m=>m.Title == "HOBBIT 1"),1);
        }
    }
}
