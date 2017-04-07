using System;
using System.Linq;
using System.Threading.Tasks;
using MovieSearchApi.Application;
using MovieSearchApi.Application.Dto;
using MovieSearchApi.Common;

namespace MovieSearchApi.Domain
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchRepository _SearchRepository;

        public SearchManager(ISearchRepository searchRepository)
        {
            _SearchRepository = searchRepository;
        }

        public async Task<SearchResponseDto> GetSearchResults(SearchRequestDto searchRequestDto)
        {
            var searchResponse = await _SearchRepository.GetSearchResults(
                new SearchRequest
                {
                    Query = searchRequestDto.Query,
                    SearchSettings = new SearchSettings
                    {
                        StandardAnalyzer = searchRequestDto.SearchSettings.StandardAnalyzer,
                        SnowballAnalyzer = searchRequestDto.SearchSettings.SnowballAnalyzer,
                        EdgeNGramAnalyzer = searchRequestDto.SearchSettings.EdgeNGramAnalyzer,
                        PhraseMatching = searchRequestDto.SearchSettings.PhraseMatching,
                        AnalyzerBoosting = searchRequestDto.SearchSettings.AnalyzerBoosting,
                        FieldBoosting = searchRequestDto.SearchSettings.FieldBoosting
                    }
                });

            return new SearchResponseDto
            {
                Results = searchResponse.Results.Select(r => 
                    new MovieSearchResultDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        PlotSummary = r.PlotSummary
                    })
            };
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            movie.Id = Guid.NewGuid().ToString();
            return await _SearchRepository.IndexMovie(movie);
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            return await _SearchRepository.UpdateMovie(movie);
        }

        public async Task DeleteMovie(string movieId)
        {
            await _SearchRepository.DeleteMovie(movieId);
        }
    }
}