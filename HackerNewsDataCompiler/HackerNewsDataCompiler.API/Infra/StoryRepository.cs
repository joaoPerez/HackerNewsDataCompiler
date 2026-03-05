using System.Net.Http.Json;
using HackerNewsDataCompiler.API.Domain.Dtos;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Models;

namespace HackerNewsDataCompiler.API.Infra
{
    public class StoryRepository : IStoryRepository
    {
        public const string HttpClientName = "HackerNewsV0";

        private readonly HttpClient _httpClient;

        public StoryRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientName);
        }

        public async Task<ResponseModel<List<int>>> GetBestStoriesIds()
        {
            var response = await _httpClient.GetAsync("beststories.json");

            if (!response.IsSuccessStatusCode)
                return ResponseModel<List<int>>.Failure(
                    (int)response.StatusCode,
                    $"Failed to retrieve best story IDs. HackerNews API responded with {(int)response.StatusCode}.");

            var data = await response.Content.ReadFromJsonAsync<List<int>>();
            if (data is null)
                return ResponseModel<List<int>>.Failure(500, "Failed to deserialize best story IDs from HackerNews API.");

            return ResponseModel<List<int>>.Success(data);
        }

        public async Task<ResponseModel<StoryDto>> GetStoryDetails(int storyId)
        {
            var response = await _httpClient.GetAsync($"item/{storyId}.json");

            if (!response.IsSuccessStatusCode)
                return ResponseModel<StoryDto>.Failure(
                    (int)response.StatusCode,
                    $"Failed to retrieve details for story {storyId}. HackerNews API responded with {(int)response.StatusCode}.");

            var data = await response.Content.ReadFromJsonAsync<StoryDto>();
            if (data is null)
                return ResponseModel<StoryDto>.Failure(500, $"Failed to deserialize details for story {storyId} from HackerNews API.");

            return ResponseModel<StoryDto>.Success(data);
        }
    }
}
