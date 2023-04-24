using FluentValidation;

namespace Application.Uslugi.Queries
{
    public class ServiceDetailsQueryValidator : AbstractValidator<ServiceDetailsQuery>
    {
        public ServiceDetailsQueryValidator()
        {
            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}