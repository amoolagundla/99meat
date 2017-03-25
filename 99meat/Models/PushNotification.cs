using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _99meat.Models
{
    public class PushNotification
    {
        public List<string> tokens { get; set; }
        public string profile { get; set; }
        public Notification notification { get; set; }
    }

    public class Notification
    {
        public string message { get; set; }
        public string title { get; set; }

    }

    public class SendNotification
    {
        public async Task<string> SendText(string message, string to)
        {
             using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.twilio.com");

                var req = new HttpRequestMessage(HttpMethod.Post, "/2010-04-01/Accounts/ACd7bd0f2c7c4c4fae3a5d2df4c186f75f/Messages");

                var byteArray = new UTF8Encoding().GetBytes("ACd7bd0f2c7c4c4fae3a5d2df4c186f75f:38eb1e677a75332305f0aef123544350");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formData = new List<KeyValuePair<string, string>>();
                formData.Add(new KeyValuePair<string, string>("Access-Control-Allow-Origin", "*"));
                formData.Add(new KeyValuePair<string, string>("From", "+12408396762"));
                formData.Add(new KeyValuePair<string, string>("To", to));
                formData.Add(new KeyValuePair<string, string>("Body", message));
                req.Content = new FormUrlEncodedContent(formData);
                var res = await client.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {


                }
                else
                {

                }
            }
            return string.Empty;
        }
        public async Task<string> SendPushNotification(string tittle, string message, string token,string phonenumber=null)
        {
            var isTokenActive = Newtonsoft.Json.JsonConvert.DeserializeObject<OneSignalTokens>(token);

            var notification = new OneSignalNotification()
            {
                android_accent_color = "ed1717",
                small_icon = "icon",
                large_icon = "http://biryanicity.azurewebsites.net/assets/img/icon.png",
                contents = new Contents()
                {
                    en = message
                },
                app_id = "19e74911-eb39-4e13-83dd-1c11cb3cba1e",
                include_player_ids = new List<string>() { isTokenActive.userId }
            };
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://onesignal.com/");

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "basic NTU5MjkwNjQtNTljMS00ZTBhLTg2YjMtYTI1ZWY2OTJiMDUz");
                    client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/notifications", notification);

                    if (response.IsSuccessStatusCode)
                    {


                    }
                    else
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }

            return string.Empty;
        }

        private async Task<bool> getTokenActive(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.ionic.io/");

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJmODA4ZGNiYS0yYWE1LTQzYTEtYmI5Yi0zZjY1ODc5OTk1MDcifQ.Se0V8bFULm8srr4_C4IH2rjwrRJylOXawE33rGRZ7aM");
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                HttpResponseMessage response = await client.GetAsync("push/tokens");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();                  
                    var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenData>(content);
                    var tokendata =  tokenData.data.Where(x => x.token.ToString().Equals(token)).FirstOrDefault();
                    if(tokendata!=null)
                    {
                        return tokendata.valid;
                    }
                }

                return false;

            }
        }
    }

}