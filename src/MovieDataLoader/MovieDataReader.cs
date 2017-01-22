using System.Collections.Generic;
using System.IO;

namespace MovieDataLoader
{
    public class MovieDataReader
    {
        public static IDictionary<string, Movie> ReadMovieData()
        {
            var movies = ReadMovieMetadata();
            var plotSummaries = ReadPlotSummaries();

            foreach (var movie in movies)
            {
                if (plotSummaries.ContainsKey(movie.Key))
                {
                    movie.Value.PlotSummary = plotSummaries[movie.Key];
                }
            }

            return movies;
        }

        private static IDictionary<string, Movie> ReadMovieMetadata()
        {
            var rawLines = File.ReadAllLines("movie-metadata.tsv");
            var movieDictionary = new Dictionary<string, Movie>();

            foreach (var rawLine in rawLines)
            {
                var values = rawLine.Split('\t');

                var movieMetadata = new Movie
                {
                    Id = values[0],
                    Name = values[2]
                };
                
                movieDictionary.Add(values[0], movieMetadata);
            }

            return movieDictionary;
        }

        private static IDictionary<string, string> ReadPlotSummaries()
        {
            var rawLines = File.ReadAllLines("plot-summaries.tsv");
            var plotSummaries = new Dictionary<string, string>();

            foreach (var rawLine in rawLines)
            {
                var values = rawLine.Split('\t');
                plotSummaries.Add(values[0], values[1]);
            }

            return plotSummaries;
        }
    }
}