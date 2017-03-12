using System.Collections.Generic;

namespace MovieSearchApi.Common
{
    public static class ElasticsearchMovieAnalyzerHelper
    {
        public static IEnumerable<string> AllIndexAnalyzers =>
            new List<string>
            {
                Standard,
                Snowball,
                EdgeNGram
            };

        public static string ToSearchAnalyzer(this string indexAnalyzer)
        {
            if (indexAnalyzer == EdgeNGram)
            {
                return Standard;
            }

            return indexAnalyzer;
        }
        public const string Standard = "standard";

        public const string Snowball = "snowball";
        
        public const string EdgeNGram = "edgeNGram";
    }
}
