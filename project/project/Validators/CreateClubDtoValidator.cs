using FluentValidation;
using Project.Models.DTO;

namespace Project.Validators;

public class CreateClubDtoValidator : AbstractValidator<CreateClubDto>
{
    public CreateClubDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type must be Football or Basketball");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.Founded)
            .NotEmpty().WithMessage("Founded date is required")
            .LessThan(DateTime.UtcNow).WithMessage("Founded date must be in the past");
    }
}

public class UpdateClubDtoValidator : AbstractValidator<UpdateClubDto>
{
    public UpdateClubDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type must be Football or Basketball");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.Founded)
            .NotEmpty().WithMessage("Founded date is required")
            .LessThan(DateTime.UtcNow).WithMessage("Founded date must be in the past");
    }
}

