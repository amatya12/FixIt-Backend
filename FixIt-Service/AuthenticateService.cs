using FixIt_Backend.Dto;
using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace FixIt_Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ILogger<AuthenticateService> logger;
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public AuthenticateService(ILogger<AuthenticateService> logger, DataContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }
        public User GetUserFromGoogle(CredentialsDto credentials)
        {
            var supportEmail = configuration["supportEmail"];
            string email = LoginServiceLibrary.GettingUserProfileDataFromGoogleApi(credentials.AccessToken);
            logger.LogInformation($"{email} is returned from Google Api.");

            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new UnauthorizedAccessException($"Please contact {supportEmail} to access the beta app.");
            }
            logger.LogInformation($"user is {user}");
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["JwtSecretKey"]);
            var userRole = userManager.GetRolesAsync(user).Result;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),

                }),
                Expires = DateTime.Now.AddDays(150),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            foreach (var role in userRole)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateGuestJwtToken(string Id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["JwtSecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, Id),
                    new Claim(ClaimTypes.Role, UserRolesList.GUEST.Name),
                }),
                Expires = DateTime.Now.AddDays(150),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
