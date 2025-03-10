﻿using AutoMapper;
using MovieClubX.Entities.Dto;
using MovieClubX.Entities.Entity;

namespace MovieClubX.Endpoint.Helpers
{
    public class DtoProvider
    {
        public Mapper Mapper { get;  }

        public DtoProvider()
        {
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MovieCreateUpdateDto, Movie>()
                    .AfterMap((src, dest) => dest.Slug = SlugGenerator.SlugGenerator.GenerateSlug(src.Title));


                cfg.CreateMap<Movie, MovieViewDto>();
                cfg.CreateMap<RateCreateDto, Rate>();
                cfg.CreateMap<Rate, RateViewDto>();
                cfg.CreateMap<Movie, MovieShortViewDto>()
                    .AfterMap((src, dest) => dest.AvgRate = src?.Rates?.Count > 0 ? src.Rates.Average(r => r.Value) : 0);
            }));
        }
    }
}
