using System.Threading.Tasks;
using MovieSearchApi.Application;
using MovieSearchApi.Application.Dto;

namespace MovieSearchApi.Domain
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchRepository _SearchRepository;

        public async Task<SearchResponseDto> GetSearchResults(SearchRequestDto searchRequestDto)
        {
            var searchResponse = await _SearchRepository.GetSearchResults(new SearchRequest());
            return new SearchResponseDto();
        }
    }
}