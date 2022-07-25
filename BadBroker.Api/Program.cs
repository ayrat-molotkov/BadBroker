using BadBroker.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();
builder.AddConfiguration();

var app = builder.Build();

app.Configure(builder.Configuration);

app.Run();