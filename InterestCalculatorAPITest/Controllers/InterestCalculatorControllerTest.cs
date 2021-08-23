using InterestCalculatorAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InterestCalculatorAPITest.Controllers
{
    public class InterestCalculatorControllerTest
    {
        [Fact]
        public async void ShouldReturnStatusCode200AndInterestEqual10010()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> { { "RateFinderAPIConfig:BaseURL", "https://localhost:44367/" } };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(new decimal(0.01).ToString(CultureInfo.InvariantCulture)) };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var interestCalculatorController = new InterestCalculatorController(configuration, httpClient);

            // Act
            var result = await interestCalculatorController.GetAsync(100, 5);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("105,10", okResult.Value);
        }

        [Fact]
        public async void ShouldReturnStatusCode500WithMessageFailedInConfigurationKey()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string>();
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var httpClient = new HttpClient();
            var interestCalculatorController = new InterestCalculatorController(configuration, httpClient);

            // Act
            var result = await interestCalculatorController.GetAsync(0, 0);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Não foi configurado a chave 'BaseURL' para a API RateFinder", objectResult.Value);
        }

        [Fact]
        public async void ShouldReturnStatusCode500WithMessageUnableCommunicateWithRateFinderAPI()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> { { "RateFinderAPIConfig:BaseURL", "https://localhost:44367/" } };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var interestCalculatorController = new InterestCalculatorController(configuration, httpClient);

            // Act
            var result = await interestCalculatorController.GetAsync(0, 0);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Não foi possível comunicar com API RateFinder", objectResult.Value);
        }
    }
}
