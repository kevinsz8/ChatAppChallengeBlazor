using ChatCodeChallenge.Models;
using ChatCodeChallenge.RabbitMQ;
using Microsoft.AspNetCore.SignalR;

namespace ChatCodeChallenge.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRabitMQService _rabitMQProducer;

        public ChatHub(IRabitMQService rabitMQProducer)
        {
            _rabitMQProducer = rabitMQProducer;
        }

        public async Task SendMessage(string user, string message,DateTime timeStamp)
        {


            await Clients.All.SendAsync("ReceiveMessage", user, message, timeStamp);
        }


        public async Task PostCommandMessage(string user, string message, DateTime timeStamp)
        {
            if (message.StartsWith("/stock="))
            {
                var stockCode = message.Substring(7);

                if (stockCode != "aapl.us")
                {
                    await Clients.All.SendAsync("ReceiveMessageFromMQ", "Exception", stockCode + " is an invalid command.", timeStamp);
                    return;
                }

                try
                {
                    //this is the Stock API Call
                    var stockInfo = await GetStockInfo(stockCode);
                    var newmessage = stockInfo.StockCode.ToUpper() + " quote is $" + stockInfo.CurrentPrice + " per share.";
                    _rabitMQProducer.SendStockMessage(newmessage);
                }
                catch (HttpRequestException ex)
                {
                    await Clients.All.SendAsync("ReceiveMessageFromMQ", "Exception", ex.Message, timeStamp);
                }
                catch (Exception ex)
                {
                    await Clients.All.SendAsync("ReceiveMessageFromMQ", "Exception", "An error occurred while processing your request. Please try again later.", timeStamp);
                }
            }
        }


        public async Task<StockInfo> GetStockInfo(string stockCode)
        {
            var apiUrl = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var csvData = await response.Content.ReadAsStringAsync();

                var lines = csvData.Split('\n');
                if (lines.Length >= 2)
                {
                    var values = lines[1].Split(',');
                    if (values.Length >= 7 && decimal.TryParse(values[6], out var currentPrice))
                    {
                        return new StockInfo
                        {
                            StockCode = stockCode,
                            CurrentPrice = currentPrice
                        };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while fetching stock data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing stock data.", ex);
            }

            return new StockInfo();
        }

    }
}
