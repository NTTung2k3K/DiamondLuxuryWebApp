
using Microsoft.AspNetCore.SignalR;

namespace DiamondLuxurySolution.WebApp.Models
{
    public class CustomerChatHub : Hub
    {


        

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user,message);
        }

       
    }

}
