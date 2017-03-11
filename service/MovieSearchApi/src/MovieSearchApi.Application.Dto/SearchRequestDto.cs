namespace MovieSearchApi.Application.Dto
{
    public class SearchRequestDto
    {
        public string Query { get; set; }

        public SearchSettingsDto SearchSettings { get; set; }
    }
}