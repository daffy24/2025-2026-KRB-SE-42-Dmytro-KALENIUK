using EducationPlatform.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EducationPlatform.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public void MapEndpoints()
        {
            var endpoints = app.Services.GetServices<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                endpoint.Map(app);
            }
        }
    }
}
