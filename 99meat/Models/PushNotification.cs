using System;
using System.Collections.Generic;
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
            // Your Account SID from twilio.com/console
            //  var accountSid = "AC0ab059bec97d061fbc4b742f86d01bea";
            // Your Auth Token from twilio.com/console
            //  var authToken = "c0ba1f4ca1f63e4e57b1cf3982f0bf07";

            //var client = new RestClient("https://api.twilio.com/2010-04-01/Accounts/ACd7bd0f2c7c4c4fae3a5d2df4c186f75f/Messages");
            //var request = new  RestRequest(Method.POST);
            //request.AddHeader("postman-token", "6b5ef0fc-b7e2-c20d-170c-3b720192a43d");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("authorization", "Basic QUNkN2JkMGYyYzdjNGM0ZmFlM2E1ZDJkZjRjMTg2Zjc1ZjozOGViMWU2NzdhNzUzMzIzMDVmMGFlZjEyMzU0NDM1MA==");
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddParameter("application/x-www-form-urlencoded", "From=%2B12408396762&To=%2B14246340454&Body=hiii", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

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
        public async Task<string> SendPushNotification(string tittle, string message, string token)
        {
            var notify = new PushNotification();
            notify.tokens = new List<string>();
            notify.tokens.Add(token);
            notify.profile = "dev";
            notify.notification = new Notification()
            {
                title = tittle,
                message = message
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.ionic.io/");

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyMDZmOTlkMi1hMmUzLTQxZjAtOWE2NC1kNjZlZTNlZGEwODQifQ.yNYeHNZ36LuHSHioqqSi8HZCInsH4ujQLgnJFYIpOAI");
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                HttpResponseMessage response = await client.PostAsJsonAsync("push/notifications", notify);

                if (response.IsSuccessStatusCode)
                {


                }
                else
                {

                }
            }

            return string.Empty;
        }
    }

}