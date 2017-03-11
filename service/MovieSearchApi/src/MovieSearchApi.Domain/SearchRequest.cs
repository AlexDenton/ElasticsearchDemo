namespace MovieSearchApi.Domain
{
    public class SearchRequest
    {
        public string Query { get; set; }

        public SearchSettings SearchSettings { get; set; }
    }
}