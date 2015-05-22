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
        public static List<Models.JokeModel> jokes=null;

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
            
            Helper.RequestHelper helper = new Helper.RequestHelper();
            if (Helper.NetworkHelper.IsNetworkAvailable())
            {
                jokes = await helper.LoadJokes();
            }
            else
            {
                string tip = "The internet is not available now";
                if (!System.Globalization.CultureInfo.CurrentCulture.DisplayName.Contains("en"))
                {
                    tip = "当前网络不可用，将加载本地数据";
                }
                await new Windows.UI.Popups.MessageDialog(tip).ShowAsync();
                jokes = await helper.LoadLocalJokesData();
            }
            lvJokes.ItemsSource = jokes;
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