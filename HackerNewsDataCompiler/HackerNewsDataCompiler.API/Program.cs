using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Services;
using HackerNewsDataCompiler.API.Infra;
using HackerNewsDataCompiler.API.Middleware;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMemoryCache();
builder.Services.Configure<HackerNewsCacheOptions>(
    builder.Configuration.GetSection(HackerNewsCacheOptions.SectionName));

builder.Services.AddHttpClient(StoryRepository.HttpClientName, client =>
{
    var baseAddress = builder.Configuration["HackerNewsBaseAddressV0"]!.TrimEnd('/') + "/";
    client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddScoped<StoryRepository>();
builder.Services.AddScoped<IStoryRepository, CachedStoryRepository>(sp =>
    new CachedStoryRepository(
        sp.GetRequiredService<StoryRepository>(),
        sp.GetRequiredService<IMemoryCache>(),
        sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<HackerNewsCacheOptions>>()));

builder.Services.AddScoped<IStoryService, StoriesService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HackerNewsDataCompiler API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
