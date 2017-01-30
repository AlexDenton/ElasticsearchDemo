using System.Collections.Generic;

namespace MovieSearchApi.Application.Dto
{
    public class SearchResponseDto
    {
        public IEnumerable<MovieSearchResultDto> Results { get; set; }
    }
}