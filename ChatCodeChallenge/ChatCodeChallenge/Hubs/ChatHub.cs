using Microsoft.AspNetCore.SignalR;

namespace ChatCodeChallenge.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task PostCommandMessage(string user, string message)
        {
            if (message.StartsWith("/stock="))
            {

                var stockCode = message.Substring(7);

                //this is goign to be the Stock API Call
                //var stockInfo = await GetStockInfo(stockCode);


                await Clients.All.SendAsync("ReceiveMessage", user, "message with command El magooooo");
            }
        }

        
    }
}
