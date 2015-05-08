using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LaifuEntertainment
{
    /// <summary>
    /// About
    /// </summary>
    public sealed partial class AboutPage : Page
    {

        public AboutPage()
        {
            this.InitializeComponent();
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
            e.Handled = true;
        }
    }
}
