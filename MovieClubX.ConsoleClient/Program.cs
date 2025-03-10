namespace MovieClubX.ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient c = new HttpClient();
            Client.Client client = new Client.Client("https://localhost:7215", c);

            var movies = await client.MovieAllAsync();
            
        }
    }
}
