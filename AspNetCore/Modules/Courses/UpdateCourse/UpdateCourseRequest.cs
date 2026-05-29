using System;
using System.Collections.Generic;
using Application.Modules.Courses.Requests;
using Common.Models;

namespace EducationPlatform.Modules.Courses.UpdateCourse;

public sealed record UpdateCourseRequest(
    string Name,
    string Summary,
    string Description,
    string Language,
    decimal Price,
    List<string>? Tags,
    PublicationStatus Status)
{
    public UpdateCourseCommand ToRequest(Guid courseId, Guid userId, bool canManageAllCourses)
    {
        return new UpdateCourseCommand(
            courseId,
            userId,
            canManageAllCourses,
            Name,
            Summary,
            Description,
            Language,
            Price,
            Tags ?? [],
            Status);
    }
}
