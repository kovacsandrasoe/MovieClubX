using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieClubX.Data;
using MovieClubX.Entities.Dto;
using MovieClubX.Entities.Entity;
using MovieClubX.Logic;
using MovieClubX.Logic.Dto;
using SlugGenerator;
using System.Security.Cryptography;

namespace MovieClubX.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        MovieLogic logic;

        public MovieController(MovieLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public IEnumerable<MovieViewDto> Get()
        {
            return logic.Read();
        }

        [HttpGet("{slug}")]
        public MovieViewDto Get(string slug)
        {
            return logic.Read(slug);
        }

        [HttpGet("Short")]
        public IEnumerable<MovieShortViewDto> GetShort()
        {
            return logic.ReadShort();
        }

        [HttpPost]
        public async Task Post(MovieCreateUpdateDto dto)
        {
            await logic.Create(dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await logic.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task Update(string id, [FromBody] MovieCreateUpdateDto dto)
        {
            await logic.Update(id, dto);
        }
    }
}
