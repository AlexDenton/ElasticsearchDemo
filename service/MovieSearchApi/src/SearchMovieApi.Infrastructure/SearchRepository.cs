using System.Threading.Tasks;
using MovieSearchApi.Domain;
using Nest;
using SearchRequest = MovieSearchApi.Domain.SearchRequest;

namespace SearchMovieApi.Infrastructure
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ElasticClient _ElasticClient;
        public SearchRepository(ElasticClient elasticClient)
        {
            _ElasticClient = elasticClient;
        }

        public Task<SearchResponse> GetSearchResults(SearchRequest searchRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}