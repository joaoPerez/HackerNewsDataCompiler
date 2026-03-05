using HackerNewsDataCompiler.API.Domain.Dtos;
using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNewsDataCompiler.API.Infra
{
    public class CachedStoryRepository : IStoryRepository
    {
        private const string BestStoriesIdsCacheKey = "best_stories_ids";
        private static string StoryDetailsCacheKey(int id) => $"story_{id}";

        private readonly IStoryRepository _inner;
        private readonly IMemoryCache _cache;
        private readonly HackerNewsCacheOptions _options;

        public CachedStoryRepository(IStoryRepository inner, IMemoryCache cache, IOptions<HackerNewsCacheOptions> options)
        {
            _inner = inner;
            _cache = cache;
            _options = options.Value;
        }

        public async Task<ResponseModel<List<int>>> GetBestStoriesIds()
        {
            if (_cache.TryGetValue(BestStoriesIdsCacheKey, out List<int>? cached) && cached is not null)
                return ResponseModel<List<int>>.Success(cached);

            var response = await _inner.GetBestStoriesIds();

            if (response.IsSuccess)
                _cache.Set(BestStoriesIdsCacheKey, response.Data, TimeSpan.FromMinutes(_options.BestStoriesIdsTtlMinutes));

            return response;
        }

        public async Task<ResponseModel<StoryDto>> GetStoryDetails(int storyId)
        {
            var cacheKey = StoryDetailsCacheKey(storyId);

            if (_cache.TryGetValue(cacheKey, out StoryDto? cached) && cached is not null)
                return ResponseModel<StoryDto>.Success(cached);

            var response = await _inner.GetStoryDetails(storyId);

            if (response.IsSuccess)
                _cache.Set(cacheKey, response.Data, TimeSpan.FromMinutes(_options.StoryDetailsTtlMinutes));

            return response;
        }
    }
}
