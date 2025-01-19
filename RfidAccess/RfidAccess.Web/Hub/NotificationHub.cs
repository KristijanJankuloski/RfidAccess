using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace RfidAccess.Web.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task Send(string method, NotificationDto message)
        {
            await Clients.All.SendAsync(method, JsonConvert.SerializeObject(message));
        }
    }

    public static class HubMethods
    {
        public static readonly string Notification = "Notification";

        public static readonly string Confirmation = "Confirmation";

        public static readonly string Error = "Error";

        public static readonly string Warning = "Warning";
    }
}
