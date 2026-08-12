using FluentValidation;
using MyFirstApi.DTOs;

namespace MyFirstApi.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(255)
                .WithMessage("Title cannot exceed 255 characters.");

            RuleFor(x => x.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required.")
                .MaximumLength(20)
                .WithMessage("ISBN cannot exceed 20 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");
        }
    }
}