using Application.Modules.Courses.Requests;
using EducationPlatform.Modules;
using EducationPlatform.Modules.Courses.AddCourse;
using EducationPlatform.Modules.Courses.DeleteCourse;
using EducationPlatform.Modules.Courses.GetCourseById;
using EducationPlatform.Modules.Courses.GetCourses;
using EducationPlatform.Modules.Lessons.AddLesson;
using EducationPlatform.Modules.Lessons.DeleteLesson;
using EducationPlatform.Modules.Lessons.GetLessonById;
using EducationPlatform.Modules.Lessons.GetLessons;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EducationPlatform.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(AddCourseCommand).Assembly));

        services.AddSingleton<IEndpoint, AddCourseEndpoint>();
        services.AddSingleton<IEndpoint, GetCoursesEndpoint>();
        services.AddSingleton<IEndpoint, GetCourseByIdEndpoint>();
        services.AddSingleton<IEndpoint, DeleteCourseEndpoint>();

        services.AddSingleton<IEndpoint, AddLessonEndpoint>();
        services.AddSingleton<IEndpoint, GetLessonsEndpoint>();
        services.AddSingleton<IEndpoint, GetLessonByIdEndpoint>();
        services.AddSingleton<IEndpoint, DeleteLessonEndpoint>();

        services.AddScoped<IValidator<AddCourseRequest>, AddCourseRequestValidator>();
        services.AddScoped<IValidator<AddLessonRequest>, AddLessonRequestValidator>();

        return services;
    }
}
