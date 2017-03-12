using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MovieDataLoader.Elasticsearch;

namespace MovieDataLoader
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var conifgurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = conifgurationBuilder.Build();
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var movieData = MovieDataReader.ReadMovieData();
            var indexName = Configuration["ElasticsearchSettings:ElasticsearchIndexName"];
            var elasticsearchServiceUri = Configuration["ElasticsearchSettings:ElasticsearchServiceUri"];
            var elasticsearchMovieIndexManager = new ElasticsearchMovieIndexManager(elasticsearchServiceUri, indexName);
            elasticsearchMovieIndexManager.RebuildMovieIndex(indexName);
            await elasticsearchMovieIndexManager.LoadMovieData(movieData.Values);
        }
    }
}
