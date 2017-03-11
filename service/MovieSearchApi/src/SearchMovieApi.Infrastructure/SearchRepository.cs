using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
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

        public async Task<SearchResponse> GetSearchResults(SearchRequest searchRequest)
        {
            var searchDescriptor = new SearchDescriptor<Movie>()
                .SearchType(SearchType.QueryThenFetch)
                .Query(q => QueryStringFactory.CreateQueryString(searchRequest));

            var elasticsearchResponse = await _ElasticClient.SearchAsync<Movie>(searchDescriptor);
            var searchResponse = new SearchResponse
            {
                Results = elasticsearchResponse.Hits.Select(h => h.Source)
            };

            return searchResponse;
        }
    }
}