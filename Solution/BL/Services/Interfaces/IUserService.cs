using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.User;

namespace BL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<GetUserPorfileDTO> GetProfileAsync(string userId);
        public Task<GetUserPorfileDTO> UpdateProfileAsync(UpdateUserProfileDTO profileDTO, string userId);
        public Task<GetUserPorfileDTO> AdminUpdateUserAsync(UpdateFullUserAccountDTO dto, string userId);

        public Task ChangePasswordAsync(ChangePasswordDTO passwordDTO, string userId);
        public Task ChangeAccountStatus(ChangeAccountStatusDTO statusDTO, string userId);
        //public Task DeleteAccount(ChangeAccountStatusDTO statusDTO, string userId);
        //public Task<GetUserPorfileDTO> UpdatePassword(UpdateFullUserAccountDTO userDTO, string userId);


    }
}
