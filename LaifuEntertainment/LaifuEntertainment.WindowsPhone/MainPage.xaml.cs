using System;
using System.Collections.Generic;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
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
        public static List<Models.JokeModel> jokes = null;
        private static DateTime now=DateTime.Now;
        private static bool bExit=false;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            Load();
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //判断是否 bExit是否为true，
            if (bExit)
            {
                //如果点击与上次点击超过 3秒，则重新将 bExit 设置为 false
                if ((now.AddSeconds(3)) < (DateTime.Now))
                {
                    bExit = false;
                }
            }
            //如果bExit为false，如果是第一次点击后退键
            if (!bExit)
            {
                //重新取新的时间, 则(now.AddSeconds(3)) >= (DateTime.Now)恒为true
                now = DateTime.Now;
            }
            //判断是否是3s之内点击了两次后退键，如果是，则重新定义bExit为false，并退出应用
            if (((now.AddSeconds(3)) >= (DateTime.Now)) && bExit)
            {
                //3秒之内并且是第二次点击后退键
                Application.Current.Exit();
            }
            else
            {
                //不满足退出条件则是第一次点击，设置 bExit 为true，并屏蔽掉后退键，再次点击则是第二次点击
                now = DateTime.Now;
                //开始缓动动画
                Tip.IsOpen = true;
                tip.Begin();
                //设置bExit为true
                bExit = true;
                e.Handled = true;
            }
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


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //
            Frame.BackStack.Clear();
            //Prepare page for display here.
            //lvDetails.ItemsSource = await new Helper.RequestHelper().LoadJokes();
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            base.OnNavigatedFrom(e);
        }

        private void appbarSettings_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void lvJokes_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.JokeModel model = e.ClickedItem as Models.JokeModel;
            Frame.Navigate(typeof(DetailsPage), model);
        }
    }
}
