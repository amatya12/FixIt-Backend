using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using FixIt_Backend.Dto;
using FixIt_Interface;
using FixIt_Data.Context;
using FixIt_Model.Users;
using FixIt_Backend.Helpers;
using FixIt_Dto.Dto;

namespace SeluApplication.Web.Controllers
{
    /// <summary>
    /// It retrieves the User Information by calling LDap API.
    /// It has Login function which returns JWt after successful Login.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService authenticateService;
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthController> logger;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authenticateService">Service for Authentication.</param>
        /// <param name="context"> Instance of Database context.</param>
        /// <param name="configuration">Instance of IConfiguration.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="userService">userservices.</param>
        /// <param name="userManager">usermanager is a class provided by Identity Library which has bunch of in built methods.</param>
        public AuthController(IAuthenticateService authenticateService, ILogger<AuthController> logger, DataContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            this.authenticateService = authenticateService;
            this.context = context;
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <summary>
        /// This api requests LDAP server, which first verifies that the credentials match and then provides us the user information.
        /// </summary>
        /// <param name="credentials">we sent the token from frontend.</param>
        /// <returns> JWT token to the user.</returns>
        [HttpPost]
        [Route("/api/login")]
        public IActionResult Login(CredentialsDto credentials)
        {
            logger.LogInformation($"User attempting to log in using token {credentials.AccessToken}...");

            var id = "guest-" + Guid.NewGuid().ToString("n");

            if (credentials.AccessToken == null)
            {
                // login as guest
                var result = authenticateService.GenerateGuestJwtToken(id);
                logger.LogInformation("Visitor successfully logged in.");

                return Ok(new DtoOutput<LoginResponseDto>(
                    new LoginResponseDto
                    {
                        Jwt = result,
                        Role = new List<string>() { UserRolesList.GUEST.Name },
                        Id = id,
                    }));
            }
            else
            {
                // var user = GetUserFromLdap(credentials);
                var user = authenticateService.GetUserFromGoogle(credentials);

                var token = authenticateService.GenerateJwtToken(user);

                logger.LogInformation("User successfully logged in.");
                // return credential modal

                return this.Ok(new DtoOutput<LoginResponseDto>(
                    new LoginResponseDto
                    {
                        Jwt = token,
                        Email = user.Email,
                        Role = (List<string>)userManager.GetRolesAsync(user).Result,
                        Id = user.Id.ToString(),
                        Name = user.Name,
                    }, "Successfully authorized user."));
            }
        }
    }
}
