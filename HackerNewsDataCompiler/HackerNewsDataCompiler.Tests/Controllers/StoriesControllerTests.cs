using HackerNewsDataCompiler.API.Controllers;
using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackerNewsDataCompiler.Tests.Controllers
{
    public class StoriesControllerTests
    {
        private readonly Mock<IStoryService> _serviceMock;
        private readonly StoriesController _controller;

        public StoriesControllerTests()
        {
            _serviceMock = new Mock<IStoryService>();
            _controller = new StoriesController(_serviceMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task GetBestStoriesDetails_ReturnsOkWithStoriesFromService()
        {
            var ids = new List<int> { 1, 2 };
            var stories = new List<Story>
            {
                new() { Title = "Story A", Uri = "https://a.com", PostedBy = "alice", Score = 150, CommentCount = 8, Time = DateTime.UtcNow },
                new() { Title = "Story B", Uri = "https://b.com", PostedBy = "bob",   Score = 250, CommentCount = 20, Time = DateTime.UtcNow }
            };

            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ResponseModel<IEnumerable<int>>.Success(ids));
            _serviceMock.Setup(s => s.GetBestStoriesDetails(ids.ToArray())).ReturnsAsync(ResponseModel<IEnumerable<Story>>.Success(stories));

            var actionResult = await _controller.GetBestStoriesDetails();

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value).ToList();
            Assert.Equal(2, result.Count);
            Assert.Equal("Story A", result[0].Title);
            Assert.Equal("alice", result[0].PostedBy);
            Assert.Equal("Story B", result[1].Title);
            Assert.Equal("bob", result[1].PostedBy);
        }

        [Fact]
        public async Task GetBestStoriesDetails_WhenNoStoriesExist_ReturnsOkWithEmptyCollection()
        {
            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ResponseModel<IEnumerable<int>>.Success([]));
            _serviceMock.Setup(s => s.GetBestStoriesDetails(Array.Empty<int>())).ReturnsAsync(ResponseModel<IEnumerable<Story>>.Success([]));

            var actionResult = await _controller.GetBestStoriesDetails();

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBestStoriesDetails_PassesIdsReturnedByGetBestStoriesIDsToGetBestStoriesDetails()
        {
            var ids = new List<int> { 7, 8, 9 };
            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ResponseModel<IEnumerable<int>>.Success(ids));
            _serviceMock.Setup(s => s.GetBestStoriesDetails(ids.ToArray())).ReturnsAsync(ResponseModel<IEnumerable<Story>>.Success([]));

            await _controller.GetBestStoriesDetails();

            _serviceMock.Verify(s => s.GetBestStoriesIDs(), Times.Once);
            _serviceMock.Verify(s => s.GetBestStoriesDetails(ids.ToArray()), Times.Once);
        }

        [Fact]
        public async Task GetBestStoriesDetails_WhenGetBestStoriesIDsFails_ReturnsResponseModelFailure()
        {
            _serviceMock.Setup(s => s.GetBestStoriesIDs())
                .ReturnsAsync(ResponseModel<IEnumerable<int>>.Failure(503, "Upstream unavailable."));

            var actionResult = await _controller.GetBestStoriesDetails();

            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(503, objectResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(objectResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal(503, response.ErrorStatusCode);
            Assert.Equal("Upstream unavailable.", response.ErrorMessage);
        }

        [Fact]
        public async Task GetBestStoriesDetails_WhenGetBestStoriesDetailsFails_ReturnsResponseModelFailure()
        {
            var ids = new List<int> { 1 };
            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ResponseModel<IEnumerable<int>>.Success(ids));
            _serviceMock.Setup(s => s.GetBestStoriesDetails(ids.ToArray()))
                .ReturnsAsync(ResponseModel<IEnumerable<Story>>.Failure(404, "Story not found."));

            var actionResult = await _controller.GetBestStoriesDetails();

            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(404, objectResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(objectResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal(404, response.ErrorStatusCode);
            Assert.Equal("Story not found.", response.ErrorMessage);
        }
    }
}
