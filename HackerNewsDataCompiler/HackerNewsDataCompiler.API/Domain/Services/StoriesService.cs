using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Interfaces;

namespace HackerNewsDataCompiler.API.Domain.Services
{
    public class StoriesService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoriesService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<IEnumerable<int>> GetBestStoriesIDs()
        {
            var response = await _storyRepository.GetBestStoriesIds();
            return response;
        }

        public async Task<IEnumerable<Story>> GetBestStoriesDetails(int[] storiesIds)
        {
            var storiesDetails = new List<Story>();
            foreach (var storyId in storiesIds)
            {
               var storyDto = await _storyRepository.GetStoryDetails(storyId);
               storiesDetails.Add(storyDto.ToStory());
            }

            return storiesDetails;
        }
    }
}
