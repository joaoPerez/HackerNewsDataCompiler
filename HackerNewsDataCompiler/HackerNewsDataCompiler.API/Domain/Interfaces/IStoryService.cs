using HackerNewsDataCompiler.API.Domain.Entities;

namespace HackerNewsDataCompiler.API.Domain.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<int>> GetBestStoriesIDs();
        Task<IEnumerable<Story>> GetBestStoriesDetails(int[] storiesIds);
    }
}
