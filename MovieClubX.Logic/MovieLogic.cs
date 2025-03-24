using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieClubX.Data;
using MovieClubX.Entities.Dto.Movie;
using MovieClubX.Entities.Entity;
using MovieClubX.Logic.Dto;

namespace MovieClubX.Logic
{
    public class MovieLogic
    {
        public Repository<Movie> repository;
        public Mapper mapper;

        public MovieLogic(Repository<Movie> repository, DtoProvider provider)
        {
            this.repository = repository;
            this.mapper = provider.Mapper;
        }

        public IEnumerable<MovieViewDto> Read()
        {
            return repository.GetAll().Select(t => mapper.Map<MovieViewDto>(t));
        }

        public MovieViewDto Read(string slug)
        {
            return mapper.Map<MovieViewDto>(repository.GetAll().FirstOrDefault(t => t.Slug == slug));
        }

        public IEnumerable<MovieShortViewDto> ReadShort()
        {
            return repository.GetAll().Select(t => mapper.Map<MovieShortViewDto>(t));
        }

        public async Task Create(MovieCreateUpdateDto dto)
        {
            //minta kivétel dobás
            //if (dto.Title.Contains("asd"))
            //{
            //    throw new Exception("Title cannot contain asd");
            //}
            var movie = mapper.Map<Movie>(dto);
            await repository.CreateAsync(movie);
        }

        public async Task Delete(string id)
        {
            await repository.DeleteByIdAsync(id);
        }

        public async Task Update(string id, MovieCreateUpdateDto dto, string userid)
        {
            var movieToUpdate = repository.FindById(id);
            if (movieToUpdate != null && movieToUpdate.CreatorId == userid)
            {
                mapper.Map(dto, movieToUpdate);
                await repository.UpdateAsync(movieToUpdate);
            }
        }
    }
}
