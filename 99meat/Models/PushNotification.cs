using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public Notification()
        {
            var cliet = new HttpClient(); 
        }
    }

    public class SendNotification
    {
        public void SendPushNotification()
        {

        }

}