using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MovieSearchApi.Common;
using Nest;

namespace MovieSearchApi.Domain
{
    public static class QueryStringFactory
    {
        public static QueryContainerDescriptor<Movie> CreateQueryString(SearchRequest searchRequest)
        {
            var queryContainerDescriptor = new QueryContainerDescriptor<Movie>();

            foreach (var indexField in ElasticsearchMovieFieldHelper.AllIndexFields)
            {
                foreach (var indexAnalyzer in ElasticsearchMovieAnalyzerHelper.AllIndexAnalyzers)
                {
                    if (ShouldSearchIndex(indexAnalyzer, searchRequest.SearchSettings))
                    {
                        queryContainerDescriptor
                            .Match(mqd => mqd
                                .Field(indexField)
                                .Analyzer(indexAnalyzer.ToSearchAnalyzer())
                                .Query(searchRequest.Query));
                    }
                }
            }

            return queryContainerDescriptor;
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