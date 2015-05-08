using System;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LaifuEntertainment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {

        public SettingsPage()
        {
            this.InitializeComponent();
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
            //设置事件已经处理
            e.Handled = true;
        }

        private async void feedback_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:ben121011@126.com"));
            //Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            //mail.Subject = "JustLaugh Feedback ”开心一哈“建议反馈";
            //mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("ben121011@126.com", "SparkLee"));
            //await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void rateApp_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + Windows.ApplicationModel.Store.CurrentApp.AppId));
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }
    }
}
