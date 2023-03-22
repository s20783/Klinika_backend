﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class CreateGodzinyPracyCommandValidator : AbstractValidator<CreateGodzinyPracyCommand>
    {
        public CreateGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.requestList).ForEach(y => y.Where(s => s.GodzinaRozpoczecia > new TimeSpan(7, 0, 0)));
        }
    }
}
