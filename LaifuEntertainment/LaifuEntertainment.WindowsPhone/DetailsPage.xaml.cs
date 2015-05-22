using System;
using System.Collections.Generic;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LaifuEntertainment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private static string jokeContent = null;
        private static MediaElement media = null;
        private static Models.JokeModel joke = null;
        private static int index = -1, maxIndex = MainPage.jokes.Count-1;

        public DetailsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            //进入页面，注册后退键处理方法
            this.Loaded += (sender, e) =>
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            };
            // 退出页面，取消对后退键处理方法的注册
            this.Unloaded += (sender, e) =>
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            };
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
            }
            //设置事件已处理
            e.Handled = true;
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            joke = e.Parameter as Models.JokeModel;
            contentViewer.DataContext = joke;
            jokeContent = joke.content;
            index = MainPage.jokes.IndexOf(joke);
            if (index == -1)
            {
                throw new ArgumentException("参数错误");
            }
            if (index == 0)
            {
                appbarBack.IsEnabled = false;
            }
            else
            {
                appbarBack.IsEnabled = true;
            }
            if (index == maxIndex)
            {
                appbarForward.IsEnabled = false;
            }
            else
            {
                appbarForward.IsEnabled = true;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (media != null)
            {
                media.Stop();
            }
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// 返回主页 Navigate to homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appbarHome_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as AppBarButton).Tag.ToString();
            if (tag.Equals("Next"))
            {
                index += 1;
                joke = MainPage.jokes[index];
            }
            else
            {
                index -= 1;
                
            }
            contentViewer.DataContext = joke;
            jokeContent = joke.content;

            if (index == 0)
            {
                appbarBack.IsEnabled = false;
            }
            else
            {
                appbarBack.IsEnabled = true;
            }
            if (index == maxIndex)
            {
                appbarForward.IsEnabled = false;
            }
            else
            {
                appbarForward.IsEnabled = true;
            }
        }

        private void appbarSettings_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void appbarSpeech_Click(object sender, RoutedEventArgs e)
        {
            TextToSpeech(HandleString(jokeContent));
        }

        private string HandleString(string text)
        {
            return System.Net.WebUtility.UrlEncode(text.Replace("\r\n", "").Trim());
        }

        private async void TextToSpeech(string text)
        {
            if (Helper.NetworkHelper.IsNetworkAvailable())
            {
                media = new MediaElement();
                media.AutoPlay = true;
                media.Volume = 100;
                media.CurrentStateChanged += Media_CurrentStateChanged;
                
                //从网页获取语音流
                //media.SetSource(await Helper.SpeechHelper.GetSpeechStream(text), "audio/mpeg");
                //利用本地的语音
                SpeechSynthesisStream stream = await Helper.SpeechHelper.GetTTSStream(text);
                if (stream==null)
                {
                    await new Windows.UI.Popups.MessageDialog("未找到合适的语音").ShowAsync();
                    return;
                }
                media.SetSource(stream,stream.ContentType);
                if (media.CurrentState != Windows.UI.Xaml.Media.MediaElementState.Playing)
                {
                    media.Play();
                }

            }
            else
            {
                string tip = "The internet is not available now";
                if (!System.Globalization.CultureInfo.CurrentCulture.DisplayName.Contains("en"))
                {
                    tip = "当前网络不可用，请连接到网络后再重试";
                }
                await new Windows.UI.Popups.MessageDialog(tip).ShowAsync();
            }
        }

        private void Media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing)
            {
                appbarSpeech.IsEnabled = false;
            }
            else 
            {
                appbarSpeech.IsEnabled = true;
            }
        }
    }
}
