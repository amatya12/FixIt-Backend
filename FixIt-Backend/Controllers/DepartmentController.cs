using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Extensions;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FixIt_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ICrudService<Department> departmentService;
        private readonly ICustomFilterService<Department> filterService;
        private readonly IMapper mapper;
        public DepartmentController(DataContext context, ICrudService<Department> departmentService, ICustomFilterService<Department> filterService, IMapper mapper)
        {
            this.context = context;
            this.departmentService = departmentService;
            this.filterService = filterService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/api/department")]
        public IActionResult GetDepartments()
        {
            var filters = this.GetFilters();
            var departments = filters.Q != null ? filterService.GetAllByFilterQ(filters.Q) : departmentService.GetAll().AsQueryable();

            departments = filters.Id != null ? filterService.GetAllByFilterId(departments, filters.Id) : departments;

            //categories = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
            //             ? filterService.GetAllByFilterReferenceId(categories, filters.ReferenceId)
            //             : categories;
            var totalElems = departments.Count();
            departments = departments.Skip(filters.BeginIndex).Take(filters.Limit);
            var departmentDto = mapper.Map<IEnumerable<DepartmentDto>>(departments);
            HttpContext.Response.Headers.Add("Content-Range", $"departments {filters.BeginIndex} - {departmentDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<DepartmentDto>>(departmentDto));
        }

        [HttpPost]
        [Route("/api/department")]
        public IActionResult CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            var departmentEntity = mapper.Map<Department>(departmentDto);
            departmentService.Save(departmentEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<DepartmentDto>(departmentEntity);
                return Ok(new DtoOutput<DepartmentDto>(outputDto));
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.Message;
                return BadRequest(new DtoOutput<DepartmentDto>(null, message, ErrorCode.DEPARTMENT_CREATE_FAILED));
            }

        }


        [Route("/api/department/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var departmentFromRepo = departmentService.GetById(id);
            var outputDto = mapper.Map<DepartmentDto>(departmentFromRepo);
            return Ok(new DtoOutput<DepartmentDto>(outputDto));
        }

        [Route("/api/department/{id}")]
        [HttpPut]
        public IActionResult EditDepartment(int id, [FromBody] DepartmentDto department)
        {
            department.Id = id;
            var departmentEntity = mapper.Map<Department>(department);
            departmentService.Save(departmentEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<DepartmentDto>(departmentEntity);
                return Ok(new DtoOutput<DepartmentDto>(outputDto, "Building edited successfully", 0));

            }
            catch (Exception)
            {
                return BadRequest(new DtoOutput<DepartmentDto>(department, "Unable to edit Category", ErrorCode.CATEGORY_EDIT_FAILED));
            }

        }


        [HttpDelete]
        [Route("/api/department/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            departmentService.DeleteById(id);
            context.SaveChanges();
            return Ok(new DtoOutput<int>(0));

        }
    
    }
}