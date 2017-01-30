using System.Threading.Tasks;
using MovieSearchApi.Application.Dto;

namespace MovieSearchApi.Application
{
    public interface ISearchManager
    {
        Task<SearchResponseDto> GetSearchResults(SearchRequestDto searchRequestDto);
    }
}