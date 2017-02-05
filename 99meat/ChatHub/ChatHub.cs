using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace _99meat.ChatHub
{
    public class ChatHub : Hub
    {
        static ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void sendToSpecific(string name, string message, string to)
        {
            // Call the broadcastMessage method to update clients.
           // Clients.Caller.broadcastMessage(name, message);
            Clients.Client(dic[to]).broadcastMessage(name, message);
        }
        public void Notify(string name, string id)
        {
            if (dic.ContainsKey(name))
            {
                var s = dic.FirstOrDefault(x => x.Key == name);
                string value = string.Empty;
                dic.TryRemove(s.Key,out value);
            }
           
                dic.TryAdd(name, id);
                Clients.Others.enters(name);
            
        }
        //public void Notify(string name, string id)
        //{
        //    if (dic.ContainsKey(name))
        //    {
        //        string s = dic.FirstOrDefault(x => x.Key == name).Value;

        //        dic.TryUpdate(name, id, s);
        //        Clients.Others.enters(name);
        //    }
        //    else
        //    {
        //        dic.TryAdd(name, id);
        //        Clients.Others.enters(name);
        //    }
        //}

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
            string s;
            dic.TryRemove(name.Key, out s);
            return Clients.All.disconnected(name.Key);
        }
    }
}