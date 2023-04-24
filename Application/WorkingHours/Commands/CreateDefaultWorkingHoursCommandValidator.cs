using FluentValidation;

namespace Application.GodzinaPracy.Commands
{
    public class CreateDefaultWorkingHoursCommandValidator : AbstractValidator<CreateDefaultWorkingHoursCommand>
    {
        public CreateDefaultWorkingHoursCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
