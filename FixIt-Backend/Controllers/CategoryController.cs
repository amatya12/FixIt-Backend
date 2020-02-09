using System;
using System.Collections.Generic;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            var categoryEntity = mapper.Map<Category>(categoryDto);
            categoryService.Save(categoryEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<CategoryDto>(categoryEntity);
                return Ok(new DtoOutput<CategoryDto>(outputDto));
            }
            catch(Exception ex)
            {
                string message = ex.InnerException.Message;
                return BadRequest(new DtoOutput<CategoryDto>(null, message, ErrorCode.CATEGORY_CREATE_FAILED));
            }
            
        }

    }
}