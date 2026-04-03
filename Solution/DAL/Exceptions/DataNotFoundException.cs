using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class DataNotFoundException : BusinessException
    {
        public DataNotFoundException(string message)
        : base(message, 404, "NOT_FOUND") { }
    }
}
