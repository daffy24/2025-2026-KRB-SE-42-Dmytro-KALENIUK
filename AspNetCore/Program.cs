using Application.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using PostgreSql.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddApplication();
services.AddDbContext();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();