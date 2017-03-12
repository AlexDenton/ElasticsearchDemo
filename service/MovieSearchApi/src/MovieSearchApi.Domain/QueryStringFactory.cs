using MovieSearchApi.Common;
using Nest;

namespace MovieSearchApi.Domain
{
    public static class QueryStringFactory
    {
        public static QueryContainer CreateQueryString(SearchRequest searchRequest)
        {
            var queryContainer = new QueryContainer();

            foreach (var indexField in ElasticsearchMovieFieldHelper.AllIndexFields)
            {
                foreach (var indexAnalyzer in ElasticsearchMovieAnalyzerHelper.AllIndexAnalyzers)
                {
                    if (ShouldSearchIndex(indexAnalyzer, searchRequest.SearchSettings))
                    {
                        queryContainer |=
                            new QueryContainerDescriptor<Movie>()
                                .Match(mqd => mqd
                                    .Field(indexField.AppendSuffix(indexAnalyzer))
                                    .Analyzer(indexAnalyzer.ToSearchAnalyzer())
                                    .Query(searchRequest.Query));
                    }
                }
            }

            return queryContainer;
        }

        private static bool ShouldSearchIndex(string indexAnalyzer, SearchSettings searchSettings)
        {
            // Todo: refactor this
            if (indexAnalyzer == ElasticsearchMovieAnalyzerHelper.Standard && searchSettings.StandardAnalyzer)
            {
                return true;
            }

            if (indexAnalyzer == ElasticsearchMovieAnalyzerHelper.Snowball && searchSettings.SnowballAnalyzer)
            {
                return true;
            }

            if (indexAnalyzer == ElasticsearchMovieAnalyzerHelper.EdgeNGram && searchSettings.EdgeNGramAnalyzer)
            {
                return true;
            }

            return false;
        }
    }
}