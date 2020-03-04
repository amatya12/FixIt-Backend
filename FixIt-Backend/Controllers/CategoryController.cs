using System;
using System.Collections.Generic;
<<<<<<<<< Temporary merge branch 1
using System.Threading.Tasks;
=========
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Extensions;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace FixIt_Backend.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ICrudService<Category> categoryService;
        private readonly ICustomFilterService<Category> filterService;
        private readonly IMapper mapper;
<<<<<<<<< Temporary merge branch 1
       
        public CategoryController(DataContext context, IMapper mapper, ICrudService<Category> categoryService)
=========

        public CategoryController(DataContext context, IMapper mapper, ICrudService<Category> categoryService, ICustomFilterService<Category> filterService)
        {
            this.context = context;
            this.mapper = mapper;
            this.categoryService = categoryService;
            this.filterService = filterService;
        }

        [HttpGet]
        [Route("/api/category")]
        public IActionResult GetCategories()
        {
            var filters = this.GetFilters();
            var categories = filters.Q != null ? filterService.GetAllByFilterQ(filters.Q) : categoryService.GetAll().AsQueryable();

            categories = filters.Id != null ? filterService.GetAllByFilterId(categories,filters.Id) : categories;

            categories = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
                         ? filterService.GetAllByFilterReferenceId(categories, filters.ReferenceId)
                         : categories;
            var totalElems = categories.Count();
            categories = categories.Skip(filters.BeginIndex).Take(filters.Limit);
            var categoryDto = mapper.Map<IEnumerable<CategoryDto>>(categories);
            HttpContext.Response.Headers.Add("Content-Range", $"categories {filters.BeginIndex} - {categoryDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<CategoryDto>>(categoryDto));
        }

        [HttpPost]
        [Route("/api/category")]
        public IActionResult CreateCategory([FromBody] CategoryForCreateDto categoryDto)
        {
            var categoryEntity = mapper.Map<Category>(categoryDto);
            categoryService.Save(categoryEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<CategoryForCreateDto>(categoryEntity);
                return Ok(new DtoOutput<CategoryForCreateDto>(outputDto));
            }
            catch(Exception ex)
            {
                string message = ex.InnerException.Message;
                return BadRequest(new DtoOutput<CategoryDto>(null, message, ErrorCode.CATEGORY_CREATE_FAILED));
            }
            
        }


        [Route("/api/category/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var categoryFromRepo = categoryService.GetById(id);
            var outputDto = mapper.Map<CategoryDto>(categoryFromRepo);
            return Ok(new DtoOutput<CategoryDto>(outputDto));
        }

        [Route("/api/category")]
        [HttpPut]
        public IActionResult EditCategory([FromBody] CategoryForCreateDto category)
        {
            var categoryEntity = mapper.Map<Category>(category);
            categoryService.Save(categoryEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<CategoryDto>(categoryEntity);
                return Ok(new DtoOutput<CategoryDto>(outputDto, "Building edited successfully", 0));

            }
            catch(Exception)
            {
                return BadRequest(new DtoOutput<CategoryForCreateDto>(category, "Unable to edit Category", ErrorCode.CATEGORY_EDIT_FAILED));
            }

        }

    }
}