﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieSearchApi.Application;
using MovieSearchApi.Application.Dto;
using MovieSearchApi.Common;

namespace MovieSearchApi.Controllers
{
    [Route("movies/{movieId?}")]
    public class MovieController : Controller
    {
        private readonly ISearchManager _SearchManager;

        public MovieController(ISearchManager searchManager)
        {
            _SearchManager = searchManager;
        }

        [HttpPost]
        public async Task<Movie> PostMovie([FromBody]Movie movieDto)
        {
            return await _SearchManager.CreateMovie(movieDto);
        }

        [HttpPatch]
        public async Task<Movie> UpdateMovie(string movieId, [FromBody] Movie movieDto)
        {
            movieDto.Id = movieId;
            return await _SearchManager.UpdateMovie(movieDto);
        }

        [HttpDelete]
        public async Task DeleteMovie(string movieId)
        {
            await _SearchManager.DeleteMovie(movieId);
        }
    }

}