using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Extensions;
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
        private readonly ICustomFilterService<SubCategories> filterService;

        public SubCategoryController(DataContext context, IMapper mapper, ICrudService<SubCategories> subCategoryService,ICustomFilterService<SubCategories> filterService)
        {
            this.context = context;
            this.mapper = mapper;
            this.subCategoryService = subCategoryService;
            this.filterService = filterService;
        }

        [HttpGet]
        [Route("/api/subcategories")]
        public IActionResult GetSubCategories()
        {
            var filters = this.GetFilters();
            var subCategories = filters.Q != null ? filterService.GetAllByFilterQ(filters.Q) : subCategoryService.GetAll().AsQueryable();

            subCategories = filters.Id != null ? filterService.GetAllByFilterId(subCategories, filters.Id) : subCategories;

            subCategories = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
                         ? filterService.GetAllByFilterReferenceId(subCategories, filters.ReferenceId)
                         : subCategories;
            var totalElems = subCategories.Count();
            subCategories = subCategories.Skip(filters.BeginIndex).Take(filters.Limit);

            var subCategoryDto = mapper.Map<IEnumerable<SubCategoriesDto>>(subCategories);
            HttpContext.Response.Headers.Add("Content-Range", $"subcategories {filters.BeginIndex} - {subCategoryDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<SubCategoriesDto>>(subCategoryDto));
        }
        [Route("/api/subcategories/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var categoryFromRepo = subCategoryService.GetById(id);
            var outputDto = mapper.Map<SubCategoriesDto>(categoryFromRepo);
            return Ok(new DtoOutput<SubCategoriesDto>(outputDto));
        }
        [HttpPost]
        [Route("/api/subcategories")]
        public IActionResult CreateSubCategory([FromBody] SubCategoriesDto subCategoryDto)
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
        public IActionResult EditSubCategory([FromBody] SubCategoriesDto category)
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