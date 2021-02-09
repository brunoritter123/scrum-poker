using Microsoft.AspNetCore.SignalR;

namespace ScrumPoker.API.Services
{
    public class HubUserIdService : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var httpContext = connection.GetHttpContext();
            return httpContext.Request.Query["participante-id"];
        }
    }
}