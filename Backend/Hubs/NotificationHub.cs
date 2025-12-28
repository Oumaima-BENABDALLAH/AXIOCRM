 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.SignalR;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Hubs
{
   
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }


}

