using HackerNewsDataCompiler.API.Domain.Dtos;

namespace HackerNewsDataCompiler.Tests.Domain.Dtos
{
    public class StoryDtoTests
    {
        [Fact]
        public void ToStory_MapsAllFieldsCorrectly()
        {
            var unixTime = 1_700_000_000;
            var dto = new StoryDto
            {
                By = "user1",
                Descendants = 42,
                Id = 99,
                Kids = [1, 2, 3],
                Score = 300,
                Time = unixTime,
                Title = "Test Story",
                Type = "story",
                Url = "https://example.com"
            };

            var story = dto.ToStory();

            Assert.Equal(dto.Title, story.Title);
            Assert.Equal(dto.Url, story.Uri);
            Assert.Equal(dto.By, story.PostedBy);
            Assert.Equal(dto.Score, story.Score);
            Assert.Equal(dto.Descendants, story.CommentCount);
            Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime, story.Time);
        }

        [Fact]
        public void ToStory_WithZeroTime_ReturnsUnixEpoch()
        {
            var dto = new StoryDto
            {
                By = "author",
                Descendants = 0,
                Id = 1,
                Kids = [],
                Score = 1,
                Time = 0,
                Title = "Epoch Story",
                Type = "story",
                Url = "https://example.com"
            };

            var story = dto.ToStory();

            Assert.Equal(DateTime.UnixEpoch, story.Time);
        }
    }
}
