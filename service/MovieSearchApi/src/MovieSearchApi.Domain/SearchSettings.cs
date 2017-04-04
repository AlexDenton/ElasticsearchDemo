namespace MovieSearchApi.Domain
{
    public class SearchSettings
    {
        public bool StandardAnalyzer { get; set; }

        public bool SnowballAnalyzer { get; set; }

        public bool EdgeNGramAnalyzer { get; set; }

        public bool PhraseMatching { get; set; }

        public bool FieldBoosting { get; set; }
    }
}