using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieSearchApi.Application;
using MovieSearchApi.Application.Dto;
using MovieSearchApi.Common;

namespace MovieSearchApi.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
        private readonly ISearchManager _SearchManager;

        public MovieController(ISearchManager searchManager)
        {
            _SearchManager = searchManager;
        }

        [HttpPost]
        public async Task<Movie> PostSearchRequest([FromBody]Movie movieDto)
        {
            return await _SearchManager.CreateMovie(movieDto);
        }
    }

}