using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Models;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status502BadGateway)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status504GatewayTimeout)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBestStoriesDetails()
        {
            var idsResponse = await _storyService.GetBestStoriesIDs();
            if (!idsResponse.IsSuccess)
                return StatusCode(idsResponse.ErrorStatusCode!.Value,
                    ResponseModel.Failure(idsResponse.ErrorStatusCode!.Value, idsResponse.ErrorMessage!));

            var storiesResponse = await _storyService.GetBestStoriesDetails(idsResponse.Data!.ToArray());
            if (!storiesResponse.IsSuccess)
                return StatusCode(storiesResponse.ErrorStatusCode!.Value,
                    ResponseModel.Failure(storiesResponse.ErrorStatusCode!.Value, storiesResponse.ErrorMessage!));

            return Ok(storiesResponse.Data);
        }
    }
}
