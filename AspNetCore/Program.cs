using Application.DependencyInjection;
using EducationPlatform.DependencyInjection;
using EducationPlatform.Extensions;
using Microsoft.AspNetCore.Builder;
using PostgreSql.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplication();
services.AddDbContext(configuration);
services.AddEndpoints();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapEndpoints();

await app.RunAsync();
