using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Backend.Extensions;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model;
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

        public IssueController(DataContext context, IMapper mapper, ICrudService<Issue> issueService, ICustomFilterService<Issue> filterService)
        {
            this.context = context;
            this.mapper = mapper;
            this.issueService = issueService;
            this.filterService = filterService;
        }

        [Route("/api/issues")]
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAllIssues()
        {
            var filters = this.GetFilters();

            var issues = filters.Q != null ? filterService.GetAllByFilterQ(filters.Q) : issueService.GetAll().AsQueryable();

            issues = filters.Id != null ? filterService.GetAllByFilterId(issues, filters.Id) : issues;

            issues = filters.CustomFilters.Count > 0 && filters.CustomFilters != null
                       ? filterService.GetAllByFilterReferenceId(issues, filters.ReferenceId)
                       : issues;

            var totalElems = issues.Count();
            issues = issues.Skip(filters.BeginIndex).Take(filters.Limit);
            var issueDto = mapper.Map<IEnumerable<IssueDto>>(issues);
            HttpContext.Response.Headers.Add("Content-Range", $"issues {filters.BeginIndex} - {issueDto.Count() - 1}/ {totalElems}");
            return Ok(new DtoOutput<IEnumerable<IssueDto>>(issueDto));
        }



        [HttpPost]
        [Route("/api/issue")]
        public IActionResult CreateIssue([FromBody]IssueDto model)
        {

            var issueEntity = mapper.Map<Issue>(model);
            issueService.Save(issueEntity);
            try
            {
                context.SaveChanges();
                return Ok(new DtoOutput<IssueDto>(null, "Issue Created Successfully", 0));
            }
            catch(Exception ex)
            {
                return BadRequest(new DtoOutput<IssueDto>(model, "Unable to create issue.", ErrorCode.ISSUE_EDIT_FAILED));
            }
        }
    }
}