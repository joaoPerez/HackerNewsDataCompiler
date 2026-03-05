using HackerNewsDataCompiler.API.Domain.Entities;

namespace HackerNewsDataCompiler.API.Domain.Dtos
{
    public record StoryDto
    {
        public string By { get; init; }
        public int Descendants { get; init; }
        public int Id { get; init; }
        public List<int> Kids { get; init; }
        public int Score { get; init; }
        public int Time { get; init; }
        public string Title { get; init; }
        public string Type { get; init; }
        public string Url { get; init; }

        public Story ToStory()
        {
            return new Story
            {
                Title = this.Title,
                Uri = this.Url,
                PostedBy = this.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(this.Time).UtcDateTime,
                Score = this.Score,
                CommentCount = this.Descendants
            };
        }
    }
}