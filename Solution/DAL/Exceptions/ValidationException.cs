using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string message, object? fields)
        : base(message, 422, "VALIDATION_ERROR", fields) {}
    }
}
