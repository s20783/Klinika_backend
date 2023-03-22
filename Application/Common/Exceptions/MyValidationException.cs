using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class MyValidationException : Exception
    {
        public MyValidationException(string message)
        : base(message)
        {
           
        }

        public MyValidationException(IEnumerable<ValidationFailure> failures) : this(failures.First().ErrorMessage)
        {

        }
    }
}
