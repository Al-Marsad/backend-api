using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace BL.DTO.User
{
    public class ChangeAccountStatusDTO
    {
        [Required(ErrorMessage = "Account status is required")]
        [AllowedValues(AccountStatus.Active, AccountStatus.Inactive)]
        public AccountStatus? Status { get; set; }
    }
}
