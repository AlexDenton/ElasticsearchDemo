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
                        var qcd = new QueryContainerDescriptor<Movie>();

                        if (searchRequest.SearchSettings.PhraseMatching)
                        {
                            queryContainer |= qcd.MatchPhrase(mqd => mqd.BuildMatchQueryDescriptor(searchRequest, indexAnalyzer, indexField));
                        }
                        else
                        {
                            queryContainer |= qcd.Match(mqd => mqd.BuildMatchQueryDescriptor(searchRequest, indexAnalyzer, indexField));
                        }
                    }
                }
            }

            if (false)
            {
                foreach (var indexAnalyzer in ElasticsearchMovieAnalyzerHelper.AllIndexAnalyzers)
                {
                    queryContainer |= new QueryContainerDescriptor<Movie>()
                        .MultiMatch(mmqd => mmqd
                            .Analyzer(indexAnalyzer)
                            .Type(TextQueryType.CrossFields)
                            .Boost(0.1)
                            .Query(searchRequest.Query)
                            .Operator(Operator.And)
                            .Fields(fd =>
                            {
                                foreach (var indexField in ElasticsearchMovieFieldHelper.AllIndexFields)
                                {
                                    fd.Field(indexField.Value.AppendSuffix(indexAnalyzer));
                                }

                                return fd;
                            }));
                }
            }

            return new QueryContainerDescriptor<Movie>()
                .FunctionScore(s => s
                    .Query(_ => queryContainer)
                    .Functions(sfqd => sfqd
                        .FieldValueFactor(fvffd => fvffd
                            .Field(m => m.Popularity)
                            .Modifier(FieldValueFactorModifier.Log1P))));
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