using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Extensions;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Model.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FixIt_Backend.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserController(DataContext context,UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Route("/api/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAllUsers()
        {
           var filters = this.GetFilters();

            var userWithRoles = filters.Q != null ? (from user in context.Users
                                                     select new
                                                     {
                                                         UserId = user.Id,
                                                         Email = user.Email,
                                                         RoleNames = (from userRole in user.UserRoles
                                                                      join role in context.Roles on userRole.RoleId
                                                                      equals role.Id
                                                                      select new RoleDto() {Id = role.Id, Name = role.Name, Description = role.Description }).ToList()
                                                     }).ToList().Select(p => new User_In_Role_Dto()
                                                     {
                                                         Id = p.UserId,
                                                         Email = p.Email,
                                                         Role = p.RoleNames,

                                                     }).Where(u => u.Email.Contains(filters.Q))

                                                      :

                                                      (from user in context.Users
                                                       select new
                                                       {
                                                           UserId = user.Id,
                                                           Email = user.Email,
                                                           RoleNames = (from userRole in user.UserRoles
                                                                        join role in context.Roles on userRole.RoleId
                                                                        equals role.Id
                                                                        select new RoleDto() {Id = role.Id, Name = role.Name, Description = role.Description }).ToList()
                                                       }).ToList().Select(p => new User_In_Role_Dto()
                                                       {
                                                           Id = p.UserId,
                                                           Email = p.Email,
                                                           Role = p.RoleNames,


                                                       });

            userWithRoles = filters.Id != null ? userWithRoles.Where(x => filters.Id.Contains(x.Id)) : userWithRoles;

            userWithRoles = filters.CustomFilters.Count > 0 && filters.CustomFilters != null ? userWithRoles.Where(f => f.Role.Any(x => x.Id == filters.ReferenceId)) : userWithRoles;

            var totalElems = userWithRoles.Count();

            userWithRoles = userWithRoles.Skip(filters.BeginIndex).Take(filters.Limit);
            HttpContext.Response.Headers.Add("Content-Range", $"users {filters.BeginIndex} - {userWithRoles.Count() - 1}/{totalElems}");

            return Ok(new DtoOutput<IEnumerable<User_In_Role_Dto>>(userWithRoles,"List of Users",0));
        }

        [HttpGet("/api/user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetUser(int id)
        {
            var userWithRoles = (from user in context.Users
                                 select new
                                 {
                                     Name = user.Name,
                                     UserId = user.Id,
                                     Email = user.Email,
                                     RoleNames = (from userRole in user.UserRoles
                                                  join role in context.Roles on userRole.RoleId
                                                  equals role.Id
                                                  select new RoleDto() { Name = role.Name, Description = role.Description }).ToList()
                                 }).FirstOrDefault(x => x.UserId == id);

            var outPutDto = new User_In_Role_Dto()
            {
                Name = userWithRoles.Name,
                Id = userWithRoles.UserId,
                Email = userWithRoles.Email,
                Role = userWithRoles.RoleNames
            };

            return Ok(new DtoOutput<User_In_Role_Dto>(outPutDto));
        }


        [HttpPost]
        [Route("/api/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> CreateUser([FromBody]UserForCreateDto userDto)
        {
            try
            {
                var user = new User
                {
                    Name = userDto.Name,
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var doesUserNameAlreadyExist = await userManager.FindByEmailAsync(userDto.Email);
                if(doesUserNameAlreadyExist == null)
                {
                    await userManager.CreateAsync(user);
                    foreach (var r in userDto.RoleId)
                    {
                        var role = await roleManager.FindByIdAsync(r.ToString());
                        var result = await userManager.AddToRoleAsync(user, role.Name.ToUpperInvariant());

                    }
                }
                else
                {
                    return Ok(new DtoOutput<int>(0, "Email already exists. UnSuccessful.", 0));
                }

               
               
            }
            
            catch(Exception e)
            {
                throw new Exception("Could not create USer with ROle");
            }
            return Ok(new DtoOutput<int>(0,"user with role created",0));


        }

        [HttpPut]
        [Route("/api/user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UpdateUser([FromBody]UserForCreateDto userDto)
        {
            try
            {
                var user = new User
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var doesUserNameAlreadyExist = await userManager.FindByEmailAsync(userDto.Email);
                if (doesUserNameAlreadyExist == null)
                {
                    await userManager.CreateAsync(user);
                    foreach (var r in userDto.RoleId)
                    {
                        var role = await roleManager.FindByIdAsync(r.ToString());
                        var result = await userManager.AddToRoleAsync(user, role.Name.ToUpperInvariant());

                    }
                }
                else
                {
                    return Ok(new DtoOutput<int>(0, "Email already exists. UnSuccessful.", 0));
                }



            }

            catch (Exception e)
            {
                throw new Exception("Could not create USer with ROle");
            }
            return Ok(new DtoOutput<int>(0, "user with role created", 0));


        }

    }
}