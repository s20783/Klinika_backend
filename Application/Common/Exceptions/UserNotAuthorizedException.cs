using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException()
        {
        }

        public UserNotAuthorizedException(string message) : base(message)
        {
        }

        public UserNotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
