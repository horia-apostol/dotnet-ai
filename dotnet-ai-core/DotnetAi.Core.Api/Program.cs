using DotnetAi.Core.Api.Clients.Claude;
using DotnetAi.Core.Api.Clients.Deepseek;
using DotnetAi.Core.Api.Clients.OpenAi;
using DotnetAi.Core.Api.Interfaces;
using DotnetAi.Core.Api.Providers;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IAiClient, OpenAiClient>();
builder.Services.AddHttpClient<IAiClient, ClaudeClient>();
builder.Services.AddHttpClient<IAiClient, DeepseekClient>();

builder.Services.AddSingleton<AiProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
