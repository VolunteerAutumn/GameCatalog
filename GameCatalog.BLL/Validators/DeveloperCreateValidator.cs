using FluentValidation;
using GameCatalog.BLL.DTOs;

namespace GameCatalog.BLL.Validators;

public class DeveloperCreateValidator : AbstractValidator<DeveloperCreateDto>
{
    public DeveloperCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}