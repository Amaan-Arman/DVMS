//using Microsoft.AspNet.SignalR;
//using System;
//using System.Threading.Tasks;

//namespace DVMS
//{
//    public class ChatHub : Hub
//    {
//        public void SendMessage(string userId, string user_name, string message, string type)
//        {
//            Clients.Others.broadcastMessage(userId, user_name, message, type);
//        }
//        public override Task OnConnected()
//        {
//            string userId = Context.QueryString["userId"];
//            if (!string.IsNullOrEmpty(userId))
//            {
//                // Associate the userId with the connectionId
//                Groups.Add(Context.ConnectionId, userId);
//            }
//            return base.OnConnected();
//        }

//        public override Task OnDisconnected(bool stopCalled)
//        {
//            string userId = Context.QueryString["userId"];
//            if (!string.IsNullOrEmpty(userId))
//            {
//                Groups.Remove(Context.ConnectionId, userId);
//            }
//            return base.OnDisconnected(stopCalled);
//        }
//    }
//}
