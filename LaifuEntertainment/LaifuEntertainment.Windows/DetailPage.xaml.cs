using LaifuEntertainment.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace LaifuEntertainment
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private static MediaElement media = null;
        private static string jokeContent = null;
        private NavigationHelper navigationHelper;
        private static Models.JokeModel joke = null;
        private static int index = -1, maxIndex = MainPage.jokes.Count - 1;


        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public DetailPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
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
            //if (index == 0)
            //{
            //    appbarBack.IsEnabled = false;
            //}
            //else
            //{
            //    appbarBack.IsEnabled = true;
            //}
            //if (index == maxIndex)
            //{
            //    appbarForward.IsEnabled = false;
            //}
            //else
            //{
            //    appbarForward.IsEnabled = true;
            //}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (media != null)
            {
                media.Stop();
            }
            base.OnNavigatedFrom(e);
        }

        private void btnSpeak_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TextToSpeech(jokeContent);
        }

        private async void TextToSpeech(string text)
        {
            media = new MediaElement();
            media.AutoPlay = true;
            media.Volume = 100;
            media.SetSource(await Helper.SpeechHelper.GetSpeechStream(text), "audio/mpeg");
            media.CurrentStateChanged += Media_CurrentStateChanged;
            media.Play();
        }

        private void Media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (media.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing)
            {
                btnSpeak.IsEnabled = false;
            }
            else
            {
                btnSpeak.IsEnabled = true;
            }
        }
    }
}
