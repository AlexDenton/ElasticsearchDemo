using System.Threading.Tasks;

namespace MovieSearchApi.Domain
{
    public interface ISearchRepository
    {
        Task<SearchResponse> GetSearchResults(SearchRequest searchRequest);
    }
}