using FixIt_Backend.Dto;
using FixIt_Model.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Interface
{
    public interface IAuthenticateService
    {
         User GetUserFromGoogle(CredentialsDto credential);
         string GenerateJwtToken(User user);
         string GenerateGuestJwtToken(string id);
    }
}
