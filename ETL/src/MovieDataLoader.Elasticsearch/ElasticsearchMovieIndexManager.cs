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

        public void RebuildMovieIndex(string indexName)
        {
            _ElasticClient.DeleteIndex(indexName);

            CreateMovieIndex(indexName);
        }

        public IndexSettings BuildElasticsearchMovieIndexSettings()
        {
            var indexSettings = new IndexSettings();

            var edgeNGramTokenFilter = new EdgeNGramTokenFilter()
            {
                MinGram = 1,
                MaxGram = 15
            };

            var edgeNGramAnalyzer = new CustomAnalyzer
            {
                Filter = new List<string>
                {
                    ElasticsearchTokenFilterHelper.Standard,
                    ElasticsearchTokenFilterHelper.Lowercase,
                    ElasticsearchTokenFilterHelper.Stop,
                    ElasticsearchTokenFilterHelper.EdgeNGramTokenFilter
                },
                Tokenizer = TokenizerHelper.Standard
            };

            indexSettings.Analysis = new Analysis
            {
                TokenFilters = new TokenFilters { { ElasticsearchTokenFilterHelper.EdgeNGramTokenFilter, edgeNGramTokenFilter } },
                Analyzers = new Analyzers { { ElasticsearchMovieAnalyzerHelper.EdgeNGram, edgeNGramAnalyzer } },
            };

            return indexSettings;
        }

        public void CreateMovieIndex(string indexName)
        {
            var createIndexRequest = new CreateIndexRequest(
                indexName,
                new IndexState
                {
                    Settings = BuildElasticsearchMovieIndexSettings()
                });

            _ElasticClient.CreateIndex(createIndexRequest);

            foreach (var indexField in ElasticsearchMovieFieldHelper.AllIndexFields)
            {
                foreach (var indexAnalyzer in ElasticsearchMovieAnalyzerHelper.AllIndexAnalyzers)
                {
                    _ElasticClient.Map<Movie>(pmd => pmd
                        .Properties(pd => pd
                            .Text(tpd => tpd
                                .Name(indexField)
                                .Fields(fs => fs
                                    .Text(t => t
                                        .Name(indexField.AppendSuffix(indexAnalyzer))
                                        .Analyzer(indexAnalyzer))
                    ))));
                }
            }
        }
    }
}