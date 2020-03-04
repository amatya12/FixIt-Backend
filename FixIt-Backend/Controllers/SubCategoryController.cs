using System;
using System.Collections.Generic;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.AspNetCore.Mvc;

namespace FixIt_Backend.Controllers
{
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ICrudService<SubCategories> subCategoryService;
        private readonly IMapper mapper;

        public SubCategoryController(DataContext context, IMapper mapper, ICrudService<SubCategories> subCategoryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.subCategoryService = subCategoryService;
        }

        [HttpGet]
        [Route("/api/subcategories")]
        public IActionResult GetCategories()
        {
            var subCategoriesFromRepo = subCategoryService.GetAll();
            var subCategoryDto = mapper.Map<IEnumerable<SubCategoriesDto>>(subCategoriesFromRepo);
            return Ok(new DtoOutput<IEnumerable<SubCategoriesDto>>(subCategoryDto));
        }

        [HttpPost]
        [Route("/api/subcategories")]
        public IActionResult CreateCategory([FromBody] SubCategoriesDto subCategoryDto)
        {
            var subCategoryEntity = mapper.Map<SubCategories>(subCategoryDto);
            subCategoryService.Save(subCategoryEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<SubCategoriesDto>(subCategoryEntity);
                return Ok(new DtoOutput<SubCategoriesDto>(outputDto));
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.Message;
                return BadRequest(new DtoOutput<CategoryDto>(null, message, ErrorCode.SUBCATEGORY_CREATE_FAILED));
            }

        }

        [Route("/api/subcategories")]
        [HttpPut]
        public IActionResult EditCategory([FromBody] SubCategoriesDto category)
        {
            var categoryEntity = mapper.Map<SubCategories>(category);
            subCategoryService.Save(categoryEntity);
            try
            {
                context.SaveChanges();
                var outputDto = mapper.Map<SubCategoriesDto>(categoryEntity);
                return Ok(new DtoOutput<SubCategoriesDto>(outputDto, "Building edited successfully", 0));

            }
            catch (Exception)
            {
                return BadRequest(new DtoOutput<SubCategoriesDto>(category, "Unable to edit Category", ErrorCode.SUBCATEGORY_EDIT_FAILED));
            }

        }

    }
}