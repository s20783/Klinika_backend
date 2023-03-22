using System;

namespace Application.Common.Exceptions
{
    public class ConstraintException : Exception
    {
        public string ConstraintValue { set; get; }

        public ConstraintException(string message, int value) : base(message)
        {
            ConstraintValue = value.ToString();
        }

        public ConstraintException(string message, string value) : base(message)
        {
            ConstraintValue = value;
        }
    }
}