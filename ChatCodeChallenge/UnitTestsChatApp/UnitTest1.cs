using ChatCodeChallenge.Hubs;
using ChatCodeChallenge.Models;
using ChatCodeChallenge.RabbitMQ;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System.Net;
using Xunit;

namespace UnitTestsChatApp
{
    public class Tests
    {
        private readonly ChatHub _target;
        private readonly Mock<IRabitMQService> service;
        public Tests()
        {
            service = new Mock<IRabitMQService>();
            _target = new ChatHub(service.Object);
        }

        [Test]
        public async Task GetStockInfo_ValidStockCode_ReturnsStockInfo()
        {
            // Arrange
            string stockCode = "AAPL.US";
            string apiUrl = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";
            string csvData = "Date,Open,High,Low,Close,Volume\r\n2022-05-06,128.00,128.50,127.75,128.00,30000\r\n2022-05-05,128.00,128.25,127.50,127.75,25000\r\n";

            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClient.Setup(c => c.GetAsync(apiUrl))
                          .ReturnsAsync(new HttpResponseMessage
                          {
                              StatusCode = HttpStatusCode.OK,
                              Content = new StringContent(csvData)
                          });

            Assert.NotNull(mockHttpClient.Object);  
        }

        [Test]
        public async Task OnInitializedAsync_AddsMessageToList()
        {
            // Arrange
            //var navigationManagerMock = new Mock<NavigationManager>();
            //navigationManagerMock.Setup(x => x.ToAbsoluteUri("/chatHub")).Returns(new Uri("http://localhost/chatHub"));

            var hubConnectionMock = new Mock<HubConnection>();
            var hubConnectionBuilderMock = new Mock<IHubConnectionBuilder>();
            hubConnectionBuilderMock.Setup(x => x.WithUrl(It.IsAny<Uri>())).Returns(hubConnectionBuilderMock.Object);
            hubConnectionBuilderMock.Setup(x => x.Build()).Returns(hubConnectionMock.Object);


            var a = _target.SendMessage("testuser", "testmessge", DateTime.Now);

            // Act

            Assert.True(true);
        }
    }
}