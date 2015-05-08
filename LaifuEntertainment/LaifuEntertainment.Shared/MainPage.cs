using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LaifuEntertainment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static List<Models.JokeModel> jokes = null;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            InitDataSource();
        }

        private async void InitDataSource()
        {
            if (jokes == null)
            {
                jokes = await new Helper.RequestHelper().LoadJokes();
            }
            lvJokes.ItemsSource = jokes;
        }
    }
}