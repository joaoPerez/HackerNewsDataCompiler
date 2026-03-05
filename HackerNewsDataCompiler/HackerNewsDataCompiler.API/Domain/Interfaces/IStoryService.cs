using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Models;

namespace HackerNewsDataCompiler.API.Domain.Interfaces
{
    public interface IStoryService
    {
        Task<ResponseModel<IEnumerable<int>>> GetBestStoriesIDs();
        Task<ResponseModel<IEnumerable<Story>>> GetBestStoriesDetails(int[] storiesIds);
    }
}
