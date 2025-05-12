using FluentValidation;

public class CreateEmployeePermissionDtoValidator : AbstractValidator<CreateEmployeePermissionDto>
{
    public CreateEmployeePermissionDtoValidator()
    {
        RuleFor(x => x.PermissionDescription)
            .NotEmpty().WithMessage("Permission description is a must.")
            .MaximumLength(200).WithMessage("Permission description cannot exceed 200 words.");
    }
}