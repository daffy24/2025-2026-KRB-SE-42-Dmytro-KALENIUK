using FluentValidation;

namespace EducationPlatform.Modules.Courses.UpdateCourse;

public sealed class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => x.Trim().Length >= 2 && x.Trim().Length <= 200)
            .WithMessage("Course name must be between 2 and 200 characters.");

        RuleFor(x => x.Summary)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Course summary must be less than 500 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(10_000)
            .WithMessage("Course description must be less than 10000 characters.");

        RuleFor(x => x.Language)
            .NotEmpty()
            .MaximumLength(32)
            .WithMessage("Course language must be less than 32 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Course price must be greater than or equal to 0.");

        RuleFor(x => x.Tags)
            .Must(tags => tags is null || tags.Count <= 20)
            .WithMessage("Course tags must contain 20 items or fewer.");

        RuleForEach(x => x.Tags)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Course tag must be less than 50 characters.");

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
