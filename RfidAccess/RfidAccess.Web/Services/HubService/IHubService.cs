using RfidAccess.Web.Hub;

namespace RfidAccess.Web.Services.HubService
{
    public interface IHubService
    {
        Task SendNotification(NotificationDto dto);

        Task SendConfirmation(string code);
    }
}
