using Microsoft.AspNetCore.Mvc;
using MovieClubX.Data;
using MovieClubX.Entities.Dto;
using MovieClubX.Entities.Entity;

namespace MovieClubX.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController
    {
        MovieClubContext ctx;

        public MovieController(MovieClubContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<MovieViewDto> Get()
        {
            return ctx.Movies.Select(t => new MovieViewDto
            {
                Id = t.Id,
                Title = t.Title,
                Rate = t.Rate
            });
        }

        [HttpPost]
        public void Post(MovieCreateUpdateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Rate = dto.Rate
            };

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
                movieToUpdate.Title = dto.Title;
                movieToUpdate.Rate = dto.Rate;
                ctx.SaveChanges();
            }
        }
    }
}
