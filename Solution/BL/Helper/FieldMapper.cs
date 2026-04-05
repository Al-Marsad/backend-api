using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    internal class FieldMapper
    {
        public static string MapField(string code)
        {
            if (code.StartsWith("Password"))
                return "Password";

            return code switch
            {
                "DuplicateEmail" => "Email",
                "DuplicateUserName" => "UserName",
                "InvalidEmail" => "Email",
                "InvalidUserName" => "UserName",
                _ => "General"
            };
        }
    }
}
