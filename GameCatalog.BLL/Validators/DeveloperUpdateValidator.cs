using FluentValidation;
using GameCatalog.BLL.DTOs;

namespace GameCatalog.BLL.Validators;

public class DeveloperUpdateValidator : AbstractValidator<DeveloperUpdateDto>
{
    public DeveloperUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}