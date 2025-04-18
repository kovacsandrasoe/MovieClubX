﻿using AutoMapper;
using Hangfire;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MovieClubX.Data;
using MovieClubX.Endpoint.Helpers;
using MovieClubX.Entities.Dto.Movie;
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
        UserManager<AppUser> userManager;
        IHubContext<MovieHub> movieHub;
        IBackgroundJobClient backgroundJobClient;

        public MovieController(MovieLogic logic, UserManager<AppUser> userManager, IHubContext<MovieHub> movieHub, IBackgroundJobClient backgroundJobClient)
        {
            this.logic = logic;
            this.userManager = userManager;
            this.movieHub = movieHub;
            this.backgroundJobClient = backgroundJobClient;
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
        [Authorize]
        public async Task Post(MovieCreateUpdateDto dto)
        {
            await logic.Create(dto);

            //backgroundJobClient.Enqueue(() => Console.WriteLine("Ez egy háttérfeladat!"));

            backgroundJobClient.Schedule(() => SendResponse(), TimeSpan.FromSeconds(15));

            //await movieHub.Clients.All.SendAsync("newMovie");
        }

        [HttpGet("job")]
        public void SendResponse()
        {
            movieHub.Clients.All.SendAsync("newMovie");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task Delete(string id)
        {
            await logic.Delete(id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task Update(string id, [FromBody] MovieCreateUpdateDto dto)
        {
            var user = await userManager.GetUserAsync(User);
            await logic.Update(id, dto, user!.Id);
        }
    }
}
