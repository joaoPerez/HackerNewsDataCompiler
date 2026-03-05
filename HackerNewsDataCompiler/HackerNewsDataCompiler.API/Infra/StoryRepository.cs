using System.Net.Http.Json;
using HackerNewsDataCompiler.API.Domain.Dtos;
using HackerNewsDataCompiler.API.Domain.Interfaces;

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

        public async Task<List<int>> GetBestStoriesIds()
        {
            var response = await _httpClient.GetAsync("beststories.json");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<int>>();
        }

        public async Task<StoryDto> GetStoryDetails(int storyId)
        {
            var response = await _httpClient.GetAsync($"item/{storyId}.json");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<StoryDto>();
        }
    }
}
