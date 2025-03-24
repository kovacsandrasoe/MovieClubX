using Microsoft.AspNetCore.SignalR;
using MovieClubX.Entities.Dto.Movie;

namespace MovieClubX.Endpoint.Helpers
{
    public class MovieHub : Hub
    {
        public async Task NewMovieSignal()
        {
            await Clients.All.SendAsync("newMovie");
        }
    }
}
