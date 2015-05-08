using System;
using System.Collections.Generic;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LaifuEntertainment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private static string jokeContent = null;

        public DetailsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            //
            this.Loaded += (sender, e) =>
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            };
            // Undo the same changes when the page is no longer visible
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
            Models.JokeModel joke = e.Parameter as Models.JokeModel;
            contentViewer.DataContext = joke;
            jokeContent = joke.title + "   " + joke.content;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// 返回主页 Navigate to homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appbarHome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void appbarSettings_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void appbarSpeech_Click(object sender, RoutedEventArgs e)
        {
            //TextToSpeech("Hello world!Hi xiaoming!");
            TextToSpeech(jokeContent);
        }

        private static async void TextToSpeech(string text)
        {
            VoiceInformation voiceInfo = null;
            IEnumerable<VoiceInformation> voices = SpeechSynthesizer.AllVoices;
            foreach (VoiceInformation voice in voices)
            {
                if (voice.Language=="zh-CN")
                {
                    voiceInfo = voice;
                    break;
                }
            }
            if (voiceInfo == null)
            {
                await new Windows.UI.Popups.MessageDialog("没有合适的语音 ！").ShowAsync();
                return;
            }
            else
            {
                Helper.SpeechHelper.TextToSpeech(text, voiceInfo);
            }
        }
    }
}
