using Microsoft.AspNetCore.SignalR.Client;
using MovieClubX.Client;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieClubX.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private HubConnection connection;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICollection<MovieViewDto> Movies { get;set; } = new BindingList<MovieViewDto>();   
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7215/movieHub") // A szerver SignalR URL-je
                .WithAutomaticReconnect() // Automatikus újracsatlakozás
                .Build();

            connection.StartAsync().GetAwaiter().GetResult();


            this.DataContext = this;
            HttpClient c = new HttpClient();
            Client.Client client = new Client.Client("https://localhost:7215", c);

            this.Movies = client.MovieAllAsync().GetAwaiter().GetResult();

            connection.On("newMovie", () =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.Movies = client.MovieAllAsync().GetAwaiter().GetResult();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Movies)));
                });
            });

        }
    }
}