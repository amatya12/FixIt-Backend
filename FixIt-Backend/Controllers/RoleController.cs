using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Extensions;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
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
    public class RoleController : ControllerBase
    {
        private readonly DataContext context;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public RoleController(DataContext context, IMapper mapper, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //[HttpGet]
        //[Route("/api/roles")]
        //public IActionResult GetAll()
        //{
        //    //var roleStore = new RoleStore<ApplicationRole>(context);
        //    var roles = context.Roles.ToList();
        //    var outPutDto = mapper.Map<IEnumerable<RoleDto>>(roles);
        //    HttpContext.Response.Headers.Add("Content-Range", $"users {1}-{10}/{10}");
        //    return Ok(new DtoOutput<IEnumerable<RoleDto>>(outPutDto));
        //}

        //[HttpPost]
        //[Route("api/role")]
        //public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        //{
        //    roleDto.NormalizedName = roleDto.Name.ToUpperInvariant();
        //    var roleStore = new RoleStore<Role>(context);
        //    var result = await roleStore.FindByNameAsync(roleDto.Name.ToUpperInvariant());
        //    if (result == null)
        //    {
        //        var role = mapper.Map<Role>(roleDto);
        //        await roleStore.CreateAsync(role);
        //        return Ok(new DtoOutput<int>(0, "Role created successfully", 0));
        //    }else
        //    {
        //        return Ok(new DtoOutput<int>(0, "Role already Exists", 1));
        //    }
        //}

        [HttpGet]
        [Route("/api/role/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            var outputDto = mapper.Map<RoleDto>(role);
            return Ok(new DtoOutput<RoleDto>(outputDto));
        }

        [HttpGet]
        [Route("/api/role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAll()
        {
            //var roleStore = new RoleStore<ApplicationRole>(context);
            var filters = this.GetFilters();
            var roles = filters.Q != null ? context.Roles.Where(f => f.Name.Contains(filters.Q)) : context.Roles;
            if (filters.Id != null)
            {
                roles = context.Roles.Where(x => filters.Id.Contains(x.Id));
            }
            var totalElems = roles.Count();
            //foods = foods.OrderBy(filters.SortBy + (filters.SortOrder.Equals("ASC") ? " ascending" : " descending"));

            //roles = roles.ApplySort(filters.SortBy, filters.SortOrder, propertyMappingService.GetPropertyMapping<RoleDto, Role>());

            roles = roles
                .Skip(filters.BeginIndex)
                .Take(filters.Limit);
            var outputDto = mapper.Map<IEnumerable<RoleDto>>(roles);
            HttpContext.Response.Headers.Add("Content-Range", $"roles {filters.BeginIndex} - {outputDto.Count() - 1}/{totalElems}");
            return Ok(new DtoOutput<IEnumerable<RoleDto>>(outputDto));
        }

        [HttpPost]
        [Route("/api/role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult CreateRole([FromBody] RoleDto roleDto)
        {
            var doesRoleExist = context.Roles.FirstOrDefault(r => r.Name == roleDto.Name) != null;
            if (!doesRoleExist)
            {
                var role = mapper.Map<Role>(roleDto);
                context.Roles.Add(role);
                context.SaveChanges();
                return Ok(new DtoOutput<RoleDto>(roleDto, "Role created successfully", 0));
            }
            else
            {
                return Ok(new DtoOutput<RoleDto>(roleDto, "Role already Exists", 1));
            }
        }


        [HttpPut]
        [Route("api/role/{id}")]
        public IActionResult EditRole(int id, [FromBody] RoleDto roleDto)
        {
            bool save = false;
            roleDto.Id = id;

            var role = mapper.Map<Role>(roleDto);
            //var result = context.Entry(role);
            //result.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //context.SaveChanges();
            //var roleFromDatabase = await roleManager.FindByIdAsync(id.ToString());
            //var result = await roleManager.UpdateAsync(role);
            context.Roles.Update(role);
            while (!save)
            {
                try
                {
                    context.SaveChanges();
                    save = true;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Role)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // decide which value should be written to database
                                proposedValues[property] = proposedValue;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }

                }
            }
            return Ok(new DtoOutput<RoleDto>(roleDto, "Role Updated Successfully.", 0));
        }

        [HttpDelete("/api/role/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            await roleManager.DeleteAsync(role);
            return Ok(new DtoOutput<int>(0, "role deleted", 0));
        }
    }
}
