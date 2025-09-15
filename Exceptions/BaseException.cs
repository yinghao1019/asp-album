using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Exceptions
{
    public class BaseException : Exception
    {
        protected BaseException()
        {
        }

        protected BaseException(string? message) : base(message)
        {
        }

        protected BaseException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}