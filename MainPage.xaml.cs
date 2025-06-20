namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnSendMessageClicked(object? sender, EventArgs e) { }

        private async Task LoadCaptcha()
        {
            try
            {
                string captchaApiUrl =
                    "https://venus.sod.asu.edu/WSRepository/Services/ImageVerifier/Service.svc/GetImage";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not load image: " + ex.Message, "OK");
            }
        }
    }
}
