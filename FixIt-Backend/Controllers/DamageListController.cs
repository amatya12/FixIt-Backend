using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FixIt_Backend.Helpers;
using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FixIt_Backend.Controllers
{
    
    [ApiController]
    public class DamageListController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IDamageService damageService;
        private readonly IMapper mapper;

        public DamageListController(DataContext context, IDamageService damageService, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.damageService = damageService;
        }

        [HttpGet]
        [Route("/api/damage")]
        public IActionResult GetAllDamages()
        {
           var damages =  damageService.GetDamageList();
            var result = mapper.Map<List<DamageDto>>(damages);
            return Ok(new DtoOutput<List<DamageDto>>(result));

        }

        [HttpGet]
        [Route("/api/damage/{categoryName}")]
        public IActionResult GetDamageByCategoryRoad(string categoryName)
        {
            var damages = damageService.GetDamageByCategoryName(categoryName);
            var result = mapper.Map<List<DamageDto>>(damages);
            return Ok(new DtoOutput<List<DamageDto>>(result,$"Damages related to {categoryName} displayed",0));
        }
    }
}