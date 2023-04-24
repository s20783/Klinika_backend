using FluentValidation;

namespace Application.GodzinaPracy.Commands
{
    public class DeleteWorkingHoursCommandValidator : AbstractValidator<DeleteWorkingHoursCommand>
    {
        public DeleteWorkingHoursCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Day).NotEmpty();
        }
    }
}
