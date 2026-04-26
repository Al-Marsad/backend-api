using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppRole : IdentityRole
    {
        public virtual List<AppUserRole> UserRoles { get; set; } = new();
    }
}
