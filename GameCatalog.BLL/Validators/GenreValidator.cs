using FluentValidation;
using GameCatalog.BLL.DTOs;

namespace GameCatalog.BLL.Validators;

public class GenreCreateValidator : AbstractValidator<GenreCreateDto>
{
    public GenreCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required.")
            .MaximumLength(50);
    }
}

public class GenreUpdateValidator : AbstractValidator<GenreUpdateDto>
{
    public GenreUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required.")
            .MaximumLength(50);
    }
}