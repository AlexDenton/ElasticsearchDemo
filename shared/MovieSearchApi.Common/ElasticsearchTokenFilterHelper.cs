using System;

namespace MovieSearchApi.Common
{
    public class ElasticsearchTokenFilterHelper
    {
        public const string Standard = "standard";

        public const string Lowercase = "lowercase";

        public const string Stop = "stop";

        public const string EdgeNGramTokenFilter = "edge_ngram";

        public const string NaturalSort = "naturalsort";
    }
}