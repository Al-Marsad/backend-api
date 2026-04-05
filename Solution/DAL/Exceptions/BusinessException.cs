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
        public string Code { get; }
        public object? Fields { get; set; }


        protected BusinessException(string message, int statusCode, string code, object? fields = null)
            : base(message)
        {
            StatusCode = statusCode;
            Code = code;
            Fields = fields;
        }
    }
}
