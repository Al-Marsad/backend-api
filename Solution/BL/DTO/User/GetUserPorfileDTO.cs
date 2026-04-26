using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace BL.DTO.User
{
    public class GetUserPorfileDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string AccountStatus { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }

        public string Role { get; set; }

    }
}
