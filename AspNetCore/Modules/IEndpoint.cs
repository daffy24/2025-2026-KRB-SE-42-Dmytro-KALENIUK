using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules;

public interface IEndpoint
{
    RouteHandlerBuilder Map(IEndpointRouteBuilder app);
}
