using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using MovieSearchApi.Common;

namespace MovieDataLoader.Elasticsearch
{
    public class ElasticsearchMovieIndexManager
    {
        private readonly ElasticClient _ElasticClient;

        public ElasticsearchMovieIndexManager(string elasticsearchServiceUri, string indexName)
        {
            var settings = new ConnectionSettings(
                new Uri(elasticsearchServiceUri));

            settings.DefaultIndex(indexName);

            _ElasticClient = new ElasticClient(settings);
        }

        public async Task LoadMovieData(IEnumerable<Movie> movies)
        {
            var bulkResponse = await _ElasticClient.IndexManyAsync(movies);

            if (!bulkResponse.IsValid)
            {
                throw bulkResponse.OriginalException;
            }
        }

        public void CreateMovieIndex(string indexName)
        {
            _ElasticClient.CreateIndex(indexName);

            _ElasticClient.Map<Movie>(pmd => pmd
                .Properties(pd => pd
                    .Text(tpd => tpd
                        .Name(movie => movie.Name)
                        .Fields(fs => fs
                            .Text(t => t.Name(movie => movie.Name.Suffix(ElasticsearchMovieFieldHelper.Standard))
                                .Analyzer(ElasticsearchMovieAnalyzerHelper.Standard))
                            .Text(t => t.Name(movie => movie.Name.Suffix(ElasticsearchMovieFieldHelper.Snowball))
                                .Analyzer(ElasticsearchMovieAnalyzerHelper.Snowball))
                    ))
                    .Text(tpd => tpd
                        .Name(movie => movie.PlotSummary)
                        .Fields(fs => fs
                            .Text(t => t.Name(movie => movie.PlotSummary.Suffix(ElasticsearchMovieFieldHelper.Standard))
                                .Analyzer(ElasticsearchMovieAnalyzerHelper.Standard))
                            .Text(t => t.Name(movie => movie.PlotSummary.Suffix(ElasticsearchMovieFieldHelper.Snowball))
                                .Analyzer(ElasticsearchMovieAnalyzerHelper.Snowball))
                    ))
                ));
        }
    }
}