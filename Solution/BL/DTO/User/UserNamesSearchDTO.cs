using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BL.DTO.User
{
    public class UserNamesSearchDTO
    {
        
        public string? FirstName { get; set; }
        public string? SecondName { get; set; } 
        public string? ThirdName { get; set; } 
        public string? LastName { get; set; }
    }
}
