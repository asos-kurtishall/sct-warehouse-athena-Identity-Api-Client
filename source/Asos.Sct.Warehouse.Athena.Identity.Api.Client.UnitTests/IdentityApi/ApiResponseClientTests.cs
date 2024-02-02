using Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.UnitTests.IdentityApi
{
    public class ApiResponseClientTests
    {
        private ApiResponseClient _apiResponseClient;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly CancellationToken _cancellationToken = new CancellationToken();

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://testthis.com")
            };

            _apiResponseClient = new(httpClient);
        }

        [Test]
        public async Task GetAsync_WhenHttpClientThrowsException_ShouldReturnErrorResponse()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new Exception());

            var result = await _apiResponseClient.GetAsync<ApiResponse>("testController", "testAction", _cancellationToken);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("500", result.Error.Code);
            Assert.AreEqual("An error occurred when sending the request. Status Code: 500.", result.Error.Message);
        }

        [Test]
        public async Task PostAsync_WhenHttpClientThrowsException_ShouldReturnErrorResponse()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new Exception());

            var result =
                await _apiResponseClient.PostAsync<ApiResponse>("testController", "testAction", new { id = 1 },
                    _cancellationToken);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("500", result.Error.Code);
            Assert.AreEqual("An error occurred when sending the request. Status Code: 500.", result.Error.Message);
        }

        [Test]
        public async Task PutAsync_WhenHttpClientThrowsException_ShouldReturnErrorResponse()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new Exception());

            var result =
                await _apiResponseClient.PutAsync<ApiResponse>("testController", "testAction", new { id = 1 },
                    _cancellationToken);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("500", result.Error.Code);
            Assert.AreEqual("An error occurred when sending the request. Status Code: 500.", result.Error.Message);
        }
    }
}