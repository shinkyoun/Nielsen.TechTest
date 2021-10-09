using System;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q1.Common
{
    public class CannotMoveException : ApplicationException
    {
        public CannotMoveException() : base()
        {
        }

        public CannotMoveException(string message) : base(message)
        {
        }

        public CannotMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
