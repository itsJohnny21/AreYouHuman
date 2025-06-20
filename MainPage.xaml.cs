using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();
        private string captchaString;

        public MainPage()
        {
            InitializeComponent();
            LoadCaptcha();
        }

        private async Task<string> GetRandomString()
        {
            try
            {
                string randomStringApiUrl =
                    "https://venus.sod.asu.edu/WSRepository/Services/RandomString/Service.svc/GetRandomString/8";

                string xmlResponse = await client.GetStringAsync(randomStringApiUrl);
                var doc = System.Xml.Linq.XDocument.Parse(xmlResponse);
                string randomString = doc?.Root.Value;

                return randomString ?? "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not get random string: " + ex.Message, "OK");
                return "";
            }
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string inputString = UserInput.Text;
            if (inputString.Equals(captchaString))
            {
                await DisplayAlert("Result", "Howdy human!", "OK");
            }
            else
            {
                await DisplayAlert("Result", "Beep boop beep boop.", "OK");
            }

            await LoadCaptcha();
            UserInput.Text = "";
        }

        private async void OnLoadCaptchaClicked(object sender, EventArgs e)
        {
            await LoadCaptcha();
        }

        private async Task LoadCaptcha()
        {
            string randomString = await GetRandomString();
            if (string.IsNullOrWhiteSpace(randomString))
                return;

            captchaString = randomString;

            try
            {
                string captchaApiUrl =
                    $"https://venus.sod.asu.edu/WSRepository/Services/ImageVerifier/Service.svc/GetImage/{randomString}";

                byte[] imageBytes = await client.GetByteArrayAsync(captchaApiUrl);
                CaptchaImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not load image: " + ex.Message, "OK");
            }
        }
    }
}
