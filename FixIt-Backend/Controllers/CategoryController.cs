using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
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
        private readonly IMapper mapper;
       
        public CategoryController(DataContext context, IMapper mapper, ICrudService<Category> categoryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.categoryService = categoryService;
           
        }

        [HttpGet]
        [Route("/api/category")]
        public IActionResult GetCategories()
        {
            var categoriesFromRepo = categoryService.GetAll();
            var categoryDto = mapper.Map<IEnumerable<CategoryDto>>(categoriesFromRepo);
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
        public IActionResult EditCategory([FromBody] CategoryDto category)
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
                return BadRequest(new DtoOutput<CategoryDto>(category, "Unable to edit Category", ErrorCode.CATEGORY_EDIT_FAILED));
            }

        }

    }
}