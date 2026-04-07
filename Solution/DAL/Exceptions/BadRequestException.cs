using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class BadRequestException : BusinessException
    { 
        public BadRequestException(string message)
        : base(message, 400, "BAD_REQUEST") { }
    }
}
