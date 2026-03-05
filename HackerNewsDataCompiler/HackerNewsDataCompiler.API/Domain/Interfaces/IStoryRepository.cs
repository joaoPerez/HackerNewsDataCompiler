using HackerNewsDataCompiler.API.Domain.Dtos;

namespace HackerNewsDataCompiler.API.Domain.Interfaces
{
    public interface IStoryRepository
    {
        Task<StoryDto> GetStoryDetails(int storyId);
        Task<List<int>> GetBestStoriesIds();
    }
}
