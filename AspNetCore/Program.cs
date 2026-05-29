using Application.DependencyInjection;
using EducationPlatform.Authentication;
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
services.AddEducationPlatformAuthentication(configuration, builder.Environment);
services.AddEducationPlatformAuthorization();

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapEndpoints();

await app.RunAsync();
