
using Microsoft.AspNetCore.SignalR;

namespace DiamondLuxurySolution.AdminCrewApp.Models
{
    public class AdminChatHub : Hub
    {

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }


}
