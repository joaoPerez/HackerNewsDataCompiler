using HackerNewsDataCompiler.API.Domain.Dtos;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Services;
using Moq;

namespace HackerNewsDataCompiler.Tests.Domain.Services
{
    public class StoriesServiceTests
    {
        private readonly Mock<IStoryRepository> _repositoryMock;
        private readonly StoriesService _service;

        public StoriesServiceTests()
        {
            _repositoryMock = new Mock<IStoryRepository>();
            _service = new StoriesService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetBestStoriesIDs_ReturnsSameListFromRepository()
        {
            var expectedIds = new List<int> { 1, 2, 3 };
            _repositoryMock.Setup(r => r.GetBestStoriesIds()).ReturnsAsync(expectedIds);

            var result = await _service.GetBestStoriesIDs();

            Assert.Equal(expectedIds, result);
        }

        [Fact]
        public async Task GetBestStoriesIDs_WhenRepositoryReturnsEmpty_ReturnsEmptyList()
        {
            _repositoryMock.Setup(r => r.GetBestStoriesIds()).ReturnsAsync([]);

            var result = await _service.GetBestStoriesIDs();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBestStoriesDetails_ReturnsMappedStoriesForEachId()
        {
            var ids = new[] { 10, 20 };
            var dto1 = new StoryDto { Id = 10, Title = "Story One", By = "user1", Score = 100, Descendants = 5, Time = 1_700_000_000, Url = "https://one.com", Type = "story", Kids = [] };
            var dto2 = new StoryDto { Id = 20, Title = "Story Two", By = "user2", Score = 200, Descendants = 10, Time = 1_700_001_000, Url = "https://two.com", Type = "story", Kids = [] };

            _repositoryMock.Setup(r => r.GetStoryDetails(10)).ReturnsAsync(dto1);
            _repositoryMock.Setup(r => r.GetStoryDetails(20)).ReturnsAsync(dto2);

            var result = (await _service.GetBestStoriesDetails(ids)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Story One", result[0].Title);
            Assert.Equal("https://one.com", result[0].Uri);
            Assert.Equal("user1", result[0].PostedBy);
            Assert.Equal(100, result[0].Score);
            Assert.Equal(5, result[0].CommentCount);

            Assert.Equal("Story Two", result[1].Title);
            Assert.Equal("user2", result[1].PostedBy);
        }

        [Fact]
        public async Task GetBestStoriesDetails_WithEmptyArray_ReturnsEmptyList()
        {
            var result = await _service.GetBestStoriesDetails([]);

            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetStoryDetails(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetBestStoriesDetails_CallsRepositoryOncePerStoryId()
        {
            var ids = new[] { 1, 2, 3 };
            var dto = new StoryDto { Id = 1, Title = "T", By = "u", Score = 1, Descendants = 0, Time = 0, Url = "", Type = "story", Kids = [] };
            _repositoryMock.Setup(r => r.GetStoryDetails(It.IsAny<int>())).ReturnsAsync(dto);

            await _service.GetBestStoriesDetails(ids);

            _repositoryMock.Verify(r => r.GetStoryDetails(It.IsAny<int>()), Times.Exactly(ids.Length));
        }
    }
}
