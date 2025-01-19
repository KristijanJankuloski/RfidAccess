using RfidAccess.Web.Hub;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace RfidAccess.Web.Services.HubService
{
    public class HubService : IHubService
    {
        private readonly IHubContext<NotificationHub> hub;

        public HubService(IHubContext<NotificationHub> hub)
        {
            this.hub = hub;
        }

        public async Task SendConfirmation(string code)
        {
            NotificationDto dto = new NotificationDto
            {
                Code = code,
                Message = "confirmed",
                Date = DateTime.Now.ToString("HH:mm dd.MM.yyyy")
            };
            await hub.Clients.All.SendAsync(HubMethods.Confirmation, JsonConvert.SerializeObject(dto));
        }

        public async Task SendError(string code, string message)
        {
            NotificationDto dto = new NotificationDto
            {
                Code = code,
                Message = message,
                Date = DateTime.Now.ToString("HH:mm dd.MM.yyyy")
            };

            await hub.Clients.All.SendAsync(HubMethods.Error, JsonConvert.SerializeObject(dto));
        }

        public async Task SendNotification(NotificationDto dto)
        {
            await hub.Clients.All.SendAsync(HubMethods.Notification, JsonConvert.SerializeObject(dto));
        }

        public async Task SendWarning(string message)
        {
            NotificationDto dto = new NotificationDto
            {
                Code = "0",
                Message = message,
                Date = DateTime.Now.ToString("HH:mm dd.MM.yyyy")
            };

            await hub.Clients.All.SendAsync(HubMethods.Warning, JsonConvert.SerializeObject(dto));
        }
    }
}
