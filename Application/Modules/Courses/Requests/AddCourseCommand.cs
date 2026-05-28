using Application.Modules.Courses.Models;
using Common.Models;
using MediatR;

namespace Application.Modules.Courses.Requests;

public sealed record AddCourseCommand(
    Guid CreatorId,
    string Name,
    string Summary,
    string Description,
    string Language,
    decimal Price,
    IEnumerable<string> Tags,
    PublicationStatus Status) : IRequest<AddCourseResponse>;
