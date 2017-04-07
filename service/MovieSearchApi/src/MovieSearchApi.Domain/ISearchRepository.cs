using System.Threading.Tasks;
using MovieSearchApi.Common;

namespace MovieSearchApi.Domain
{
    public interface ISearchRepository
    {
        Task<SearchResponse> GetSearchResults(SearchRequest searchRequest);

        Task<Movie> IndexMovie(Movie movie);

        Task<Movie> UpdateMovie(Movie movie);

        Task DeleteMovie(string movieId);
    }
}