using System.Collections.Generic;

namespace MovieSearchApi.Domain
{
    public class SearchResponse
    {
        public IEnumerable<Movie> Results { get; set; }
    }
}