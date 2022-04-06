using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PokedexAPI.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Helpers
{
    public class PokeApiHelperTests
    {
        private readonly Mock<ILogger<PokeApiHelper>> _logger;
        private readonly Mock<IConfiguration> _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _testResponse;

        public PokeApiHelperTests()
        {
            _logger = new Mock<ILogger<PokeApiHelper>>();
            _configuration = new Mock<IConfiguration>();
            _testResponse = "test response";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_testResponse)
                })
                .Verifiable();

            _httpClient = new HttpClient(handlerMock.Object);
        }

        [Fact]
        public void PokeApiHelper_Should_Throw_When_Logger_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokeApiHelper(null, _configuration.Object, _httpClient));
        }

        [Fact]
        public void PokeApiHelper_Should_Throw_When_Configuration_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokeApiHelper(_logger.Object, null, _httpClient));
        }

        [Fact]
        public void PokeApiHelper_Should_Throw_When_HttpClient_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokeApiHelper(_logger.Object, _configuration.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task PokeApiHelper_Should_Throw_When_Pokemon_Name_Is_Null_Or_Whitespace(dynamic pokemon)
        {
            var handler = new PokeApiHelper(_logger.Object, _configuration.Object, _httpClient);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.GetPokemonSpeciesResponse(pokemon));
        }

        [Fact]
        public async Task PokeApiHelper_Should_Return_Response_From_PokeApi_With_Valid_Pokemon()
        {
            var testPokemonName = "test-pokemon";
            var testPokeApiUrl = "http://test.url";

            _configuration.Setup(x => x["PokeApiUrl"]).Returns(testPokeApiUrl);

            var helper = new PokeApiHelper(_logger.Object, _configuration.Object, _httpClient);
            var response = await helper.GetPokemonSpeciesResponse(testPokemonName);

            _configuration.Verify(x => x["PokeApiUrl"], Times.Once());

            Assert.Equal(_testResponse, response);
        }
    }
}
