using HackerNewsDataCompiler.API.Infra;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace HackerNewsDataCompiler.Tests.Infra
{
    public class StoryRepositoryTests
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        private static HttpClient CreateHttpClient(HttpResponseMessage response)
        {
            var baseAddress = Configuration["HackerNews:BaseAddress"]
                ?? throw new InvalidOperationException("HackerNews:BaseAddress is not configured.");

            var handler = new MockHttpMessageHandler(response);
            return new HttpClient(handler) { BaseAddress = new Uri(baseAddress) };
        }

        private static Mock<IHttpClientFactory> CreateFactoryMock(HttpClient client)
        {
            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient(StoryRepository.HttpClientName)).Returns(client);
            return factoryMock;
        }

        [Fact]
        public async Task GetBestStoriesIds_WhenResponseIsSuccess_ReturnsSuccessModel()
        {
            var ids = new List<int> { 1, 2, 3 };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(ids)
            };
            var repository = new StoryRepository(CreateFactoryMock(CreateHttpClient(response)).Object);

            var result = await repository.GetBestStoriesIds();

            Assert.True(result.IsSuccess);
            Assert.Equal(ids, result.Data);
        }

        [Fact]
        public async Task GetBestStoriesIds_WhenResponseIsNotSuccess_ReturnsFailureModel()
        {
            var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            var repository = new StoryRepository(CreateFactoryMock(CreateHttpClient(response)).Object);

            var result = await repository.GetBestStoriesIds();

            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, result.ErrorStatusCode);
        }

        [Fact]
        public async Task GetStoryDetails_WhenResponseIsSuccess_ReturnsSuccessModel()
        {
            var dto = new { id = 42, title = "Test", by = "user", score = 10, descendants = 2, time = 1_700_000_000, url = "https://x.com", type = "story", kids = Array.Empty<int>() };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(dto)
            };
            var repository = new StoryRepository(CreateFactoryMock(CreateHttpClient(response)).Object);

            var result = await repository.GetStoryDetails(42);

            Assert.True(result.IsSuccess);
            Assert.Equal("Test", result.Data!.Title);
            Assert.Equal("user", result.Data.By);
        }

        [Fact]
        public async Task GetStoryDetails_WhenResponseIsNotSuccess_ReturnsFailureModel()
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            var repository = new StoryRepository(CreateFactoryMock(CreateHttpClient(response)).Object);

            var result = await repository.GetStoryDetails(1);

            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.ErrorStatusCode);
        }

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public MockHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(_response);
        }
    }
}
