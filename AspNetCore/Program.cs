using Application.DependencyInjection;
using EducationPlatform.Authentication;
using EducationPlatform.DependencyInjection;
using EducationPlatform.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using PostgreSql.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplication();
services.AddDbContext(configuration);
services.AddEndpoints();
services.AddEducationPlatformAuthentication(configuration, builder.Environment);
services.AddEducationPlatformAuthorization();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EducationPlatform API",
        Version = "v1",
        Description = "API for courses, lessons, subscriptions, and protected lesson media.",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste a JWT access token.",
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, externalResource: null)] = [],
    });
});

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationPlatform API v1");
    options.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapEndpoints();

await app.RunAsync();

public partial class Program;
