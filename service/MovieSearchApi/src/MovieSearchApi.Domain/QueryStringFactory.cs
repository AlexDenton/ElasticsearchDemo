using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MovieSearchApi.Common;
using Nest;

namespace MovieSearchApi.Domain
{
    public static class QueryStringFactory
    {
        private static IEnumerable<string> _IndexAnalyzers => new List<string>
        {
            ElasticsearchMovieAnalyzerHelper.Standard,
            ElasticsearchMovieAnalyzerHelper.Snowball,
            ElasticsearchMovieAnalyzerHelper.EdgeNGram,
        };

        private static IEnumerable<Expression<Func<Movie, object>>> _IndexFields => new List<Expression<Func<Movie, object>>>
        {
            movie => movie.Name,
            movie => movie.PlotSummary
        };

        public static QueryContainerDescriptor<Movie> CreateQueryString(SearchRequest searchRequest)
        {
            var queryContainerDescriptor = new QueryContainerDescriptor<Movie>();

            foreach (var indexField in _IndexFields)
            {
                foreach (var indexAnalyzer in _IndexAnalyzers)
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

        private static string ToSearchAnalyzer(this string indexAnalyzer)
        {
            if (indexAnalyzer == ElasticsearchMovieAnalyzerHelper.EdgeNGram)
            {
                return ElasticsearchMovieAnalyzerHelper.Standard;
            }

            return indexAnalyzer;
        }
    }
}