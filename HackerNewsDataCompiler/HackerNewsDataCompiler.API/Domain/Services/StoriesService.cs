using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Models;

namespace HackerNewsDataCompiler.API.Domain.Services
{
    public class StoriesService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoriesService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<ResponseModel<IEnumerable<int>>> GetBestStoriesIDs()
        {
            var response = await _storyRepository.GetBestStoriesIds();
            if (!response.IsSuccess)
                return ResponseModel<IEnumerable<int>>.Failure(response.ErrorStatusCode!.Value, response.ErrorMessage!);

            return ResponseModel<IEnumerable<int>>.Success(response.Data!);
        }

        public async Task<ResponseModel<IEnumerable<Story>>> GetBestStoriesDetails(int[] storiesIds)
        {
            var storiesDetails = new List<Story>();

            foreach (var storyId in storiesIds)
            {
                var response = await _storyRepository.GetStoryDetails(storyId);
                if (!response.IsSuccess)
                    return ResponseModel<IEnumerable<Story>>.Failure(response.ErrorStatusCode!.Value, response.ErrorMessage!);

                storiesDetails.Add(response.Data!.ToStory());
            }

            return ResponseModel<IEnumerable<Story>>.Success(storiesDetails);
        }
    }
}
