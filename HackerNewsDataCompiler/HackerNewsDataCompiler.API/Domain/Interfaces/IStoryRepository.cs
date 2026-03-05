using HackerNewsDataCompiler.API.Domain.Dtos;
using HackerNewsDataCompiler.API.Domain.Models;

namespace HackerNewsDataCompiler.API.Domain.Interfaces
{
    public interface IStoryRepository
    {
        Task<ResponseModel<StoryDto>> GetStoryDetails(int storyId);
        Task<ResponseModel<List<int>>> GetBestStoriesIds();
    }
}
