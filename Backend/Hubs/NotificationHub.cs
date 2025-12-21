 using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    namespace ProductManager.API.Hubs
    {
        [Authorize]
        public class NotificationHub : Hub
        {
        public override async Task OnConnectedAsync()
        {
            // Rien à faire, SignalR utilise UserId automatiquement
            await base.OnConnectedAsync();
        }
    }

    }

