using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Dto;
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

        public IssueController(DataContext context, IMapper mapper, ICrudService<Issue> issueService)
        {
            this.context = context;
            this.mapper = mapper;
            this.issueService = issueService;
        }

        [Route("/api/issues")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAllIssues()
        {
            var issuesFromRepo = issueService.GetAll();
            var outputDto = mapper.Map<IssueDto>(issuesFromRepo);
            return Ok(new DtoOutput<IssueDto>(outputDto));
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