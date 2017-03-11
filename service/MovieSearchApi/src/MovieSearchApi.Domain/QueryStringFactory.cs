using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;

namespace MovieSearchApi.Domain
{
    public static class QueryStringFactory
    {
        private static IEnumerable<string> _IndexAnalyzers => new List<string>
        {
            "standard",
            "snowball",
            //"edgeNGram"
        };

        private static IEnumerable<Expression<Func<Movie, object>>> _IndexFields => new List<Expression<Func<Movie, object>>>
        {
            movie => movie.Name
        };

        public static QueryContainerDescriptor<Movie> CreateQueryString(SearchRequest searchRequest)
        {
            var queryContainerDescriptor = new QueryContainerDescriptor<Movie>();

            foreach (var indexField in _IndexFields)
            {
                foreach (var indexAnalyzer in _IndexAnalyzers)
                {
                    queryContainerDescriptor
                        .Match(mqd => mqd
                            .Field(indexField)
                            .Analyzer(indexAnalyzer.ToSearchAnalyzer())
                            .Query(searchRequest.Query));
                }
            }

            return queryContainerDescriptor;
        }

        private static string ToSearchAnalyzer(this string indexAnalyzer)
        {
            if (indexAnalyzer == "edgeNGram")
            {
                return "standard";
            }

            return indexAnalyzer;
        }
    }
}