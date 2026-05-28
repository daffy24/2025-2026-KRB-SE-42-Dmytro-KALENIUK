using System;
using System.Collections.Generic;
using Application.Modules.Courses.Requests;
using Common.Models;

namespace EducationPlatform.Modules.Courses.AddCourse;

public sealed record AddCourseRequest(
    string Name,
    string Summary,
    string Description,
    string Language,
    decimal Price,
    List<string>? Tags,
    PublicationStatus Status = PublicationStatus.Draft)
{
    public AddCourseCommand ToRequest(Guid creatorId)
    {
        return new AddCourseCommand(
            creatorId,
            Name,
            Summary,
            Description,
            Language,
            Price,
            Tags ?? [],
            Status);
    }
}
