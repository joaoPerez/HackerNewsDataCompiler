using HackerNewsDataCompiler.API.Domain.Entities;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsDataCompiler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("BestStoriesDetails")]
        public async Task<IEnumerable<Story>> GetBestStoriesDetails()
        {
            var bestStories = await _storyService.GetBestStoriesIDs();
            return await _storyService.GetBestStoriesDetails(bestStories.ToArray());
        }
    }
}
