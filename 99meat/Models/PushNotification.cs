using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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