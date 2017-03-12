using System.Collections.Generic;
using MovieSearchApi.Common;

namespace MovieSearchApi.Domain
{
    public class SearchResponse
    {
        public IEnumerable<Movie> Results { get; set; }
    }
}