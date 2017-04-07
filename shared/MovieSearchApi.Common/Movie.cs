using Nest;

namespace MovieSearchApi.Common
{
    public class Movie
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PlotSummary { get; set; }

        [Number(NullValue = 1)]
        public int Popularity { get; set; }
    }
}