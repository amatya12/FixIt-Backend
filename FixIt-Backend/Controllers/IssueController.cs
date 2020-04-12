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
using FixIt_Interface;
using FixIt_Model;
using FixIt_Service.CrudServices;
using FixIt_Service.HelperFunctions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FixIt_Backend.Controllers
{
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ICrudService<Issue> issueService;
        private readonly ICustomFilterService<Issue> filterService;
        private readonly IssueHelperFunction issueHelperFunction;
        private readonly ICrudService<SubCategories> subCategoryService;

        public IssueController(DataContext context, IMapper mapper, ICrudService<Issue> issueService, ICustomFilterService<Issue> filterService, IssueHelperFunction issueHelperFunction, ICrudService<SubCategories> subCategoryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.issueService = issueService;
            this.filterService = filterService;
            this.issueHelperFunction = issueHelperFunction;
            this.subCategoryService = subCategoryService;
        }

        [Route("/api/issue")]
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAllIssues()
        {
            var filters = this.GetFilters();

            var issues = filters.Q != null ? filterService.GetAllByFilterQ(filters.Q) : issueService.GetAll().AsQueryable();

            issues = filters.Id != null ? filterService.GetAllByFilterId(issues, filters.Id) : issues;

            issues = filters.Status != null ? issueHelperFunction.GetAllByCustomFilterStatus(issues, filters.Status) : issues;

            issues = filters.Priority != null ? issueHelperFunction.GetAllByCustomFilterPriority(issues, filters.Priority) : issues;

            issues = (filters.FromDate != null && filters.ToDate != null) ? issueHelperFunction.GetAllByStartAndEndDate(issues, filters.FromDate, filters.ToDate) : issues;
            issues = (filters.FromDate != null && filters.ToDate == null) ? issueHelperFunction.GetAllByStartAndEndDate(issues, filters.FromDate) : issues;
            issues = (filters.ToDate != null && filters.ToDate == null) ? issueHelperFunction.GetAllByEndDate(issues, filters.ToDate) : issues;
            issues = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
                       ? filterService.GetAllByFilterReferenceId(issues, filters.ReferenceId)
                       : issues;
           

            var totalElems = issues.Count();
            issues = issues.Skip(filters.BeginIndex).Take(filters.Limit);

            var issueDto = mapper.Map<IEnumerable<IssueForListDto>>(issues);
            HttpContext.Response.Headers.Add("Content-Range", $"issue {filters.BeginIndex} - {issueDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<IssueForListDto>>(issueDto));
        }

        [HttpGet]
        [Route("/api/issue/{id}")]
        public IActionResult GetIssue(int id)
        {
            var issue =  issueService.GetById(id);
            var issueDto = mapper.Map<IssueDto>(issue);
            return Ok(new DtoOutput<IssueDto>(issueDto));
        }


        [HttpPost]
        [Route("/api/issue")]
        public IActionResult CreateIssue([FromBody]IssueForCreateDto model)
        {
            var issueEntity = new Issue
            {
                Issues = model.Issues,
                Latitude = model.latitude,
                Longitude = model.longitude,
                Location = model.Location,
                Status = "pending",
                Priority = model.Priority,
                ImageUrl = model.ImageUrl,
                DateCreated = DateTime.Now,
                IsDeleted = false,
            };
            var result = context.Issues.Add(issueEntity);

            /*iterating through the list of subcategory Id we get from frontend.
             * For eg. potholes,oneway, donotenter
             * we get subCategoryId=[10,11,2]
            */
            foreach (var id in model.SubCategoryId)
            {
                //Creating new instance of intermediate table.
                var issueSubCategory = new IssueSubCategory();

                // Retrieving subCategory object based on subCategory Id.
                var subCategory = subCategoryService.GetById(id);

                //Mapping the SubCategory to Issue.
                issueSubCategory.Issue = issueEntity;
                issueSubCategory.SubCategory = subCategory;

                context.IssueSubCategories.Add(issueSubCategory);
            }

            //retrieving list of unique Category for the given subCategory Ids.
            var categories = issueHelperFunction.GetCategoryFromSubCategory(model.SubCategoryId);

            //iterating through unique Category.
            foreach( var category in categories.GroupBy(x => x.Id).Select(y => y.First()))
            {
                //creating a new instance of intermediate table.
                var issueCategory = new IssueCategory();

                //Mapping Category to Issue.
                issueCategory.Category = category;
                issueCategory.Issue = issueEntity;

                context.IssueCategories.Add(issueCategory);
            }
            try
            {
                context.SaveChanges();
                return Ok(new DtoOutput<IssueDto>(null, "Issue Created Successfully", 0));
            }
            catch(Exception ex)
            {
                return BadRequest(new DtoOutput<IssueForCreateDto>(model, "Unable to create issue.", ErrorCode.ISSUE_CREATE_FAILED));
            }
        }

        [HttpPut]
        [Route("/api/issue/{id}")]
        public IActionResult EditIssue(int id , [FromBody]IssueForEditDto issue)
        {
            issue.Id = id;
            
            var issueEntity = mapper.Map<Issue>(issue);
            issueEntity.DateModified = DateTime.Now;
            issueService.Save(issueEntity);
            try
            {
                context.SaveChanges();
                return Ok(new DtoOutput<IssueForEditDto>(issue, "issue edited successfully", 0));
            }
            catch(Exception ex)
            {
                return BadRequest(new DtoOutput<IssueForEditDto>(issue, " Unable to edit issue.", ErrorCode.ISSUE_EDIT_FAILED));
            }
        }

        [HttpDelete]
        [Route("/api/issue/{id}")]
        public IActionResult DeleteIssue(int id)
        {
            issueService.DeleteById(id);
            try
            {
                context.SaveChanges();
                return Ok(new DtoOutput<int>(0, "Deleted successfully", 0));

            }
            catch (Exception ex)
            {
                return BadRequest(new DtoOutput<int>(0, "Cannot delete the issue", ErrorCode.ISSUE_DELETE_FAILED));
            }

        }


    }
}