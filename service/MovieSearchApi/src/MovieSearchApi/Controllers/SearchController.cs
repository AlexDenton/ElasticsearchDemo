using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieSearchApi.Application;
using MovieSearchApi.Application.Dto;

namespace MovieSearchApi.Controllers
{
    [Route("search")]
    public class SearchController : Controller
    {
        private readonly ISearchManager _SearchManager;

        public SearchController(ISearchManager searchManager)
        {
            _SearchManager = searchManager;
        }

        [HttpPost]
        public async Task<SearchResponseDto> PostSearchRequest(SearchRequestDto searchRequestDto)
        {
            return await _SearchManager.GetSearchResults(searchRequestDto);
        }
    }
}