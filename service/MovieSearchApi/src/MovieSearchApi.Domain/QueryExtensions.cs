using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MovieSearchApi.Common;
using Nest;

namespace MovieSearchApi.Domain
{
    public static class QueryExtensions
    {
        public static MatchQueryDescriptor<T> BuildMatchQueryDescriptor<T>(
            this MatchQueryDescriptor<T> matchQueryDescriptor, 
            SearchRequest searchRequest, 
            string indexAnalyzer,
            KeyValuePair<string, Expression<Func<T, object>>> indexField) where T : class
        {
            return matchQueryDescriptor
                .Field(indexField.Value.AppendSuffix(indexAnalyzer))
                .Analyzer(indexAnalyzer.ToSearchAnalyzer())
                .Boost(GetBoost(indexAnalyzer, indexField.Key, searchRequest.SearchSettings))
                .Slop(50)
                .Query(searchRequest.Query);
        }

        private static double GetBoost(string analyzer, string field, SearchSettings searchSettings)
        {
            var boost = 1.0;

            if (searchSettings.FieldBoosting)
            {
                boost *= GetFieldBoost(field);
            }

            if (searchSettings.AnalyzerBoosting)
            {
                boost *= GetAnalyzerBoost(analyzer);
            }

            return boost;
        }

        private static double GetFieldBoost(string field)
        {
            switch (field)
            {
                case ElasticsearchMovieFieldHelper.Name:
                    return 1;
                case ElasticsearchMovieFieldHelper.PlotSummary:
                    return 0.1;
                default:
                    return 1;
            }
        }

        private static double GetAnalyzerBoost(string analyzer)
        {
            switch (analyzer)
            {
                case ElasticsearchMovieAnalyzerHelper.Standard:
                    return 1.0;
                case ElasticsearchMovieAnalyzerHelper.Snowball:
                    return 0.1;
                default:
                    return 1;
            }
        }
    }
}