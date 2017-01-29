using System.Linq;
using MovieDataLoader.Elasticsearch;

namespace MovieDataLoader
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var movieData = MovieDataReader.ReadMovieData();
            const string indexName = "movies";
            var elasticsearchMovieIndexManager = new ElasticsearchMovieIndexManager(indexName);
            elasticsearchMovieIndexManager.CreateMovieIndex(indexName);
            await elasticsearchMovieIndexManager.LoadMovieData(movieData.Values);
        }
    }
}
