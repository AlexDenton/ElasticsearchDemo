using System.Threading.Tasks;
using MovieSearchApi.Domain;

namespace SearchMovieApi.Infrastructure
{
    public class SearchRepository : ISearchRepository
    {
        public Task<SearchResponse> GetSearchResults(SearchRequest searchRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}