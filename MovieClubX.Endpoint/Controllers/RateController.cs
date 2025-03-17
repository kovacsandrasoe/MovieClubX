using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieClubX.Data;
using MovieClubX.Entities.Dto.Rate;
using MovieClubX.Entities.Entity;
using MovieClubX.Logic.Dto;

namespace MovieClubX.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateController : ControllerBase
    {
        MovieClubContext ctx;
        Mapper mapper;
        public RateController(MovieClubContext ctx, DtoProvider dtoProvider)
        {
            this.ctx = ctx;
            this.mapper = dtoProvider.Mapper;
        }

        [HttpPost]
        public void AddRate([FromBody] RateCreateDto dto)
        {
            ctx.Rates.Add(mapper.Map<Rate>(dto));
            ctx.SaveChanges();
        }
    }
}
