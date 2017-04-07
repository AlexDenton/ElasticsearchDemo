using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MovieSearchApi.Common;
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

        public async Task<Movie> IndexMovie(Movie movie)
        {
            var indexResponse = await _ElasticClient.IndexAsync(movie);

            movie.Id = indexResponse.Id;

            return movie;
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var parameters = new Dictionary<string, object>
            {
                { "value", movie.Popularity }
            };

            var updateRequest = new UpdateDescriptor<Movie, Movie>("movies", nameof(Movie).ToLowerInvariant(), movie.Id)
                .Script(ss => ss
                    .Inline("ctx._source.popularity = 100")
                    .Params(parameters));

            var indexResponse = await _ElasticClient.UpdateAsync<Movie>(updateRequest);

            return movie;
        }

        public async Task DeleteMovie(string movieId)
        {
            // Todo: make configurable
            var dr = new DeleteRequest("movies", nameof(Movie).ToLowerInvariant(), movieId);
            var deleteResponse = await _ElasticClient.DeleteAsync(dr);
        }
    }
}