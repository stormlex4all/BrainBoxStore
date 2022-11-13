using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBox.Core.Exceptions
{
    public class UserRegistrationException : Exception
    {
        public UserRegistrationException(string message) : base(message)
        {
        }
    }
}
