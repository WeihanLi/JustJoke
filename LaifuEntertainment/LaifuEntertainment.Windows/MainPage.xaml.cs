using System;
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
        private static List<Models.JokeModel> jokes=null;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Load();
        }

        /// <summary>
        /// 首次进入本页加载内容
        /// </summary>
        private void Load()
        {
            if (jokes != null)
            {
                return;
            }
            InitDataSource();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private async void InitDataSource()
        {
            progressRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await System.Threading.Tasks.Task.Delay(2000);
            if (Helper.NetworkHelper.IsNetworkAvailable())
            {
                jokes = await new Helper.RequestHelper().LoadJokes();
                lvJokes.ItemsSource = jokes;
            }
            else
            {
                await new Windows.UI.Popups.MessageDialog("当前网络不可用~~").ShowAsync();
            }
            progressRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        /// <summary>
        /// 刷新内容，重新加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            InitDataSource();
        }

        private void lvJokes_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.JokeModel model = e.ClickedItem as Models.JokeModel;
            Frame.Navigate(typeof(DetailPage), model);
        }
    }
}