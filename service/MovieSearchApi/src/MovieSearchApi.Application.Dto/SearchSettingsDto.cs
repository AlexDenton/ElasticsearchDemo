namespace MovieSearchApi.Application.Dto
{
    public class SearchSettingsDto
    {
        public bool StandardAnalyzer { get; set; }

        public bool SnowballAnalyzer { get; set; }

        public bool EdgeNGramAnalyzer { get; set; }

        public bool PhraseMatching { get; set; }

        public bool AnalyzerBoosting { get; set; }

        public bool FieldBoosting { get; set; }
    }
}