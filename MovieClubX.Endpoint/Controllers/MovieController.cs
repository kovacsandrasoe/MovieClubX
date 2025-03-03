using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieClubX.Data;
using MovieClubX.Endpoint.Helpers;
using MovieClubX.Entities.Dto;
using MovieClubX.Entities.Entity;
using SlugGenerator;
using System.Security.Cryptography;

namespace MovieClubX.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        MovieClubContext ctx;
        Mapper mapper;

        public MovieController(MovieClubContext ctx, DtoProvider provider)
        {
            this.ctx = ctx;
            this.mapper = provider.Mapper;
        }

        [HttpGet]
        public IEnumerable<MovieViewDto> Get()
        {
            //return ctx.Movies.Select(t => new MovieViewDto
            //{
            //    Id = t.Id,
            //    Title = t.Title,
            //    Rate = t.Rate
            //});
            return ctx.Movies.Select(t => mapper.Map<MovieViewDto>(t));
        }

        [HttpGet("Short")]
        public IEnumerable<MovieShortViewDto> GetShort()
        {
            return ctx.Movies.Select(t => mapper.Map<MovieShortViewDto>(t));
        }

        [HttpPost]
        public void Post(MovieCreateUpdateDto dto)
        {
            //var movie = new Movie
            //{
            //    Title = dto.Title,
            //    Rate = dto.Rate
            //};
            var movie = mapper.Map<Movie>(dto);

            //barátságos url -> majd id-nak használjuk
            //var slug = SlugGenerator.SlugGenerator.GenerateSlug(dto.Title);


            ctx.Movies.Add(movie);
            ctx.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var movie = ctx.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                ctx.Movies.Remove(movie);
                ctx.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Update(string id, [FromBody] MovieCreateUpdateDto dto)
        {
            var movieToUpdate = ctx.Movies.FirstOrDefault(m => m.Id == id);
            if (movieToUpdate != null)
            {
                mapper.Map(dto, movieToUpdate);
                ctx.SaveChanges();
            }
        }
    }
}
