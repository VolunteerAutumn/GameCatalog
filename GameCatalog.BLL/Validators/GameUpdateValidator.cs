using FluentValidation;
using GameCatalog.BLL.DTOs;

namespace GameCatalog.BLL.Validators;

public class GameUpdateValidator : AbstractValidator<GameUpdateDto>
{
    public GameUpdateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.DeveloperId)
            .GreaterThan(0).WithMessage("A valid DeveloperId is required.");
    }
}