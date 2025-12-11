using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class AccessException : Exception
    {
        public AccessException(): base("Access denied") {}
    }
}
