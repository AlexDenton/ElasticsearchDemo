using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MovieSearchApi.Common
{
    public class ElasticsearchMovieFieldHelper
    {
        public const string Name = "name";

        public const string PlotSummary = "plot-summary";

        public static IDictionary<string, Expression<Func<Movie, object>>> AllIndexFields =>
            new Dictionary<string, Expression<Func<Movie, object>>>
            {
                { Name, movie => movie.Name },
                { PlotSummary, movie => movie.PlotSummary },
            };
    }
}
