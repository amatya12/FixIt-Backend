﻿using System;
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

            //categories = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
            //             ? filterService.GetAllByFilterReferenceId(categories, filters.ReferenceId)
            //             : categories;
            var totalElems = categories.Count();
            categories = categories.Skip(filters.BeginIndex).Take(filters.Limit);
            var categoryDto = mapper.Map<IEnumerable<CategoryDto>>(categories);
            HttpContext.Response.Headers.Add("Content-Range", $"categories {filters.BeginIndex} - {categoryDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<CategoryDto>>(categoryDto));
        }

        [HttpGet]
        [Route("/api/categoryfordropdown")]
        public IActionResult GetCategoriesForDropDown()
        {
            var categoriesWithSubCategories = categoryService.GetAll();
            List<CategoryWithSubCategoryDto> result = new List<CategoryWithSubCategoryDto>();
           
            foreach( var value in categoriesWithSubCategories)
            {
                List<Children> child = new List<Children>();
                var output = new CategoryWithSubCategoryDto
                {
                    Name = value.CategoryName,
                    Id = value.Id,
                };
                foreach(var subValue in value.SubCategories)
                {
                    
                    var children = new Children
                    {
                        Id = subValue.Id,
                        Name = subValue.SubCategoryName,
                    };
                    child.Add(children);
                }
                output.Children = child;
                result.Add(output);
                
            }
            return Ok(new DtoOutput<IEnumerable<CategoryWithSubCategoryDto>>(result));
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

        [Route("/api/category/{id}")]
        [HttpPut]
        public IActionResult EditCategory( int id, [FromBody] CategoryForCreateDto category)
        {
            category.Id = id;
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

        
        [HttpDelete]
        [Route("/api/Category/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            categoryService.DeleteById(id);
            context.SaveChanges();
            return Ok(new DtoOutput<int>(0));

        }
    }
}