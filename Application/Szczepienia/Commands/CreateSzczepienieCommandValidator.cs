﻿using FluentValidation;
using System;

namespace Application.Szczepienia.Commands
{
    public class CreateSzczepienieCommandValidator : AbstractValidator<CreateSzczepienieCommand>
    {
        public CreateSzczepienieCommandValidator()
        {
            RuleFor(x => x.request.IdLek).NotEmpty();

            RuleFor(x => x.request.IdPacjent).NotEmpty();

            RuleFor(x => x.request.Data).LessThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.request.Dawka).GreaterThan(0).LessThan(1000);
        }
    }
}