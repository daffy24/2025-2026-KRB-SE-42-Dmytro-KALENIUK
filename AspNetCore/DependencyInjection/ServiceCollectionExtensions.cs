using Application.Modules.Courses.Requests;
using EducationPlatform.Modules;
using EducationPlatform.Modules.Courses.AddCourse;
using EducationPlatform.Modules.Courses.DeleteCourse;
using EducationPlatform.Modules.Courses.GetCoursePreviewImage;
using EducationPlatform.Modules.Courses.GetCourseById;
using EducationPlatform.Modules.Courses.GetCourses;
using EducationPlatform.Modules.Courses.UpdateCourse;
using EducationPlatform.Modules.Courses.UploadCoursePreviewImage;
using EducationPlatform.Files;
using EducationPlatform.Modules.Lessons.AddLesson;
using EducationPlatform.Modules.Lessons.DeleteLesson;
using EducationPlatform.Modules.Lessons.GetLessonById;
using EducationPlatform.Modules.Lessons.GetLessons;
using EducationPlatform.Modules.Lessons.GetLessonVideo;
using EducationPlatform.Modules.Lessons.UploadLessonVideo;
using EducationPlatform.Modules.Subscriptions.AddSubscription;
using EducationPlatform.Modules.Subscriptions.GetSubscriptions;
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
        services.AddSingleton<IEndpoint, GetCoursePreviewImageEndpoint>();
        services.AddSingleton<IEndpoint, DeleteCourseEndpoint>();
        services.AddSingleton<IEndpoint, UpdateCourseEndpoint>();
        services.AddSingleton<IEndpoint, UploadCoursePreviewImageEndpoint>();

        services.AddSingleton<IEndpoint, AddLessonEndpoint>();
        services.AddSingleton<IEndpoint, GetLessonsEndpoint>();
        services.AddSingleton<IEndpoint, GetLessonByIdEndpoint>();
        services.AddSingleton<IEndpoint, GetLessonVideoEndpoint>();
        services.AddSingleton<IEndpoint, DeleteLessonEndpoint>();
        services.AddSingleton<IEndpoint, UploadLessonVideoEndpoint>();

        services.AddSingleton<IEndpoint, AddSubscriptionEndpoint>();
        services.AddSingleton<IEndpoint, GetSubscriptionsEndpoint>();

        services.AddScoped<IValidator<AddCourseRequest>, AddCourseRequestValidator>();
        services.AddScoped<IValidator<UpdateCourseRequest>, UpdateCourseRequestValidator>();
        services.AddScoped<IValidator<AddLessonRequest>, AddLessonRequestValidator>();
        services.AddScoped<IFileStorage, LocalFileStorage>();

        return services;
    }
}
