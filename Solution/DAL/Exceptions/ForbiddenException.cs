using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class ForbiddenException : BusinessException
    {
        public ForbiddenException(string message)
        : base(message, 403, "FORBIDDEN") { }
    }
}
