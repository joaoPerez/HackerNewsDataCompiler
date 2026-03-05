namespace HackerNewsDataCompiler.API.Infra
{
    public class HackerNewsCacheOptions
    {
        public const string SectionName = "HackerNewsCache";

        public int BestStoriesIdsTtlMinutes { get; init; } = 5;
        public int StoryDetailsTtlMinutes { get; init; } = 15;
    }
}
