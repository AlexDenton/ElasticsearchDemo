using System.Threading.Tasks;
using MovieSearchApi.Application.Dto;
using MovieSearchApi.Common;

namespace MovieSearchApi.Application
{
    public interface ISearchManager
    {
        Task<SearchResponseDto> GetSearchResults(SearchRequestDto searchRequestDto);

        Task<Movie> CreateMovie(Movie movieDto);

        Task DeleteMovie(string movieId);
    }
}