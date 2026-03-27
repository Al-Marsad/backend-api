using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; }

        protected BusinessException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
