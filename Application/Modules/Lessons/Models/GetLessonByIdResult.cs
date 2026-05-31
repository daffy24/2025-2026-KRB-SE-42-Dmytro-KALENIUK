namespace Application.Modules.Lessons.Models;

/// <summary>
/// Represents the get lesson by id result.
/// </summary>
public sealed record GetLessonByIdResult(LessonAccessStatus Status, Lesson? Lesson = null);
