using System.Collections.Generic;

namespace MovieSearchApi.Domain
{
    public class SearchResponse
    {
        public IEnumerable<MovieSearchResult> Results { get; set; }
    }
}