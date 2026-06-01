 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.SignalR;


namespace AXIOCRM.Infrastructure.Hubs
{
   
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }


}

