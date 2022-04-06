using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PokedexAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Helpers
{
    public class TranslationsHelperTests
    {
        private readonly Mock<ILogger<TranslationsHelper>> _logger;
        private readonly Mock<IConfiguration> _configuration;
        private readonly HttpClient _invalidHttpClient;
        private readonly HttpClient _validHttpClient;
        private readonly string _testResponse;

        public TranslationsHelperTests()
        {
            _logger = new Mock<ILogger<TranslationsHelper>>();
            _configuration = new Mock<IConfiguration>();

            _testResponse = "test response";

            var invalidHandlerMock = new Mock<HttpMessageHandler>();
            invalidHandlerMock
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

            _invalidHttpClient = new HttpClient(invalidHandlerMock.Object);

            var validResponseObject = new
            {
                contents = new
                {
                    translated = _testResponse
                }
            };
            var validHandlerMock = new Mock<HttpMessageHandler>();
            validHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(validResponseObject))
                })
                .Verifiable();

            _validHttpClient = new HttpClient(validHandlerMock.Object);

        }

        [Fact]
        public void TranslationsHelper_Should_Throw_When_Logger_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TranslationsHelper(null, _configuration.Object, _validHttpClient));
        }

        [Fact]
        public void TranslationsHelper_Should_Throw_When_Configuration_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TranslationsHelper(_logger.Object, null, _validHttpClient));
        }

        [Fact]
        public void TranslationsHelper_Should_Throw_When_HttpClient_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TranslationsHelper(_logger.Object, _configuration.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TranslationsHelper_Translate_Shakespeare_Should_Throw_When_Description_Is_Null_Or_Whitespace(dynamic description)
        {
            var handler = new TranslationsHelper(_logger.Object, _configuration.Object, _validHttpClient);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.TranslateToShakespeare(description));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TranslationsHelper_Translate_Yoda_Should_Throw_When_Description_Is_Null_Or_Whitespace(dynamic description)
        {
            var handler = new TranslationsHelper(_logger.Object, _configuration.Object, _validHttpClient);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.TranslateToYoda(description));
        }

        [Fact]
        public async Task TranslationsHelper_Should_Return_Response_From_Shakespeare_Translation_Api_With_Valid_Description()
        {
            var testDescription = "test description";
            var testTranslationUrl = "http://test.url";

            _configuration.Setup(x => x["TranslatorApiUrl:Shakespeare"]).Returns(testTranslationUrl);

            var helper = new TranslationsHelper(_logger.Object, _configuration.Object, _validHttpClient);
            var response = await helper.TranslateToShakespeare(testDescription);

            _configuration.Verify(x => x["TranslatorApiUrl:Shakespeare"], Times.Once());

            Assert.Equal(_testResponse, response);
        }

        [Fact]
        public async Task TranslationsHelper_Should_Return_Response_From_Yoda_Translation_Api_With_Valid_Description()
        {
            var testDescription = "test description";
            var testTranslationUrl = "http://test.url";

            _configuration.Setup(x => x["TranslatorApiUrl:Yoda"]).Returns(testTranslationUrl);

            var helper = new TranslationsHelper(_logger.Object, _configuration.Object, _validHttpClient);
            var response = await helper.TranslateToYoda(testDescription);

            _configuration.Verify(x => x["TranslatorApiUrl:Yoda"], Times.Once());

            Assert.Equal(_testResponse, response);
        }

        [Fact]
        public async Task TranslationsHelper_Should_Return_Untranslated_Description_From_Yoda_Translation_If_Translated_Text_Unavailable()
        {
            var testDescription = "test description";
            var testTranslationUrl = "http://test.url";

            _configuration.Setup(x => x["TranslatorApiUrl:Shakespeare"]).Returns(testTranslationUrl);

            var helper = new TranslationsHelper(_logger.Object, _configuration.Object, _invalidHttpClient);
            var response = await helper.TranslateToShakespeare(testDescription);

            _configuration.Verify(x => x["TranslatorApiUrl:Shakespeare"], Times.Once());

            Assert.Equal(testDescription, response);
        }

        [Fact]
        public async Task TranslationsHelper_Should_Return_Untranslated_Description_From_Shakespeare_Translation_If_Translated_Text_Unavailable()
        {
            var testDescription = "test description";
            var testTranslationUrl = "http://test.url";

            _configuration.Setup(x => x["TranslatorApiUrl:Yoda"]).Returns(testTranslationUrl);

            var helper = new TranslationsHelper(_logger.Object, _configuration.Object, _invalidHttpClient);
            var response = await helper.TranslateToYoda(testDescription);

            _configuration.Verify(x => x["TranslatorApiUrl:Yoda"], Times.Once());

            Assert.Equal(testDescription, response);
        }
    }
}
