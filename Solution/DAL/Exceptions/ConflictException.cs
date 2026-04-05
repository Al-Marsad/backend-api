using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class ConflictException : BusinessException
    {

        public ConflictException(string message, object? fields)
        : base(message, 409, "CONFLICT", fields) {}
    }
}
