using HackerNewsDataCompiler.API.Controllers;
using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Interfaces;
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
        }

        [Fact]
        public async Task GetBestStoriesDetails_ReturnsStoriesFromService()
        {
            var ids = new List<int> { 1, 2 };
            var stories = new List<Story>
            {
                new() { Title = "Story A", Uri = "https://a.com", PostedBy = "alice", Score = 150, CommentCount = 8, Time = DateTime.UtcNow },
                new() { Title = "Story B", Uri = "https://b.com", PostedBy = "bob",   Score = 250, CommentCount = 20, Time = DateTime.UtcNow }
            };

            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ids);
            _serviceMock.Setup(s => s.GetBestStoriesDetails(ids.ToArray())).ReturnsAsync(stories);

            var result = (await _controller.GetBestStoriesDetails()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Story A", result[0].Title);
            Assert.Equal("alice", result[0].PostedBy);
            Assert.Equal("Story B", result[1].Title);
            Assert.Equal("bob", result[1].PostedBy);
        }

        [Fact]
        public async Task GetBestStoriesDetails_WhenNoStoriesExist_ReturnsEmptyCollection()
        {
            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(new List<int>());
            _serviceMock.Setup(s => s.GetBestStoriesDetails(Array.Empty<int>())).ReturnsAsync(new List<Story>());

            var result = await _controller.GetBestStoriesDetails();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBestStoriesDetails_PassesIdsReturnedByGetBestStoriesIDsToGetBestStoriesDetails()
        {
            var ids = new List<int> { 7, 8, 9 };
            _serviceMock.Setup(s => s.GetBestStoriesIDs()).ReturnsAsync(ids);
            _serviceMock.Setup(s => s.GetBestStoriesDetails(ids.ToArray())).ReturnsAsync(new List<Story>());

            await _controller.GetBestStoriesDetails();

            _serviceMock.Verify(s => s.GetBestStoriesIDs(), Times.Once);
            _serviceMock.Verify(s => s.GetBestStoriesDetails(ids.ToArray()), Times.Once);
        }
    }
}
