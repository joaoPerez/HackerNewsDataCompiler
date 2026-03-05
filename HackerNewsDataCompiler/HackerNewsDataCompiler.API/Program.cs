using HackerNewsDataCompiler.API.Domain.Interfaces;
using HackerNewsDataCompiler.API.Domain.Services;
using HackerNewsDataCompiler.API.Infra;
using HackerNewsDataCompiler.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpClient(StoryRepository.HttpClientName, client =>
{
    var baseAddress = builder.Configuration["HackerNewsBaseAddressV0"]!.TrimEnd('/') + "/";
    client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddScoped<IStoryRepository, StoryRepository>();
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
