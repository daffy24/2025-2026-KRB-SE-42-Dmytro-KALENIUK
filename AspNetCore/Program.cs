using Application.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using PostgreSql.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplication();
services.AddDbContext(configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();