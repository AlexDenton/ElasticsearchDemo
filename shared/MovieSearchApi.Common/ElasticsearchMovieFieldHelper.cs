using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MovieSearchApi.Common
{
    public class ElasticsearchMovieFieldHelper
    {
        public static IEnumerable<Expression<Func<Movie, object>>> AllIndexFields =>
            new List<Expression<Func<Movie, object>>>
            {
                movie => movie.Name,
                movie => movie.PlotSummary
            };
    }
}
