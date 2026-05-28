using System;
using FluentValidation;

namespace EducationPlatform.Modules.Lessons.AddLesson;

public sealed class AddLessonRequestValidator : AbstractValidator<AddLessonRequest>
{
    public AddLessonRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => x.Trim().Length >= 2 && x.Trim().Length <= 200)
            .WithMessage("Lesson name must be between 2 and 200 characters.");

        RuleFor(x => x.Summary)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Lesson summary must be less than 500 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(10_000)
            .WithMessage("Lesson description must be less than 10000 characters.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .WithMessage("Lesson duration must be greater than zero.");

        RuleFor(x => x.MediaType)
            .IsInEnum();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
