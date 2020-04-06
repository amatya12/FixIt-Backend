using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Dto.Dto;
using FixIt_Model;
using FixIt_Model.Users;
using System;
using System.Linq;

namespace FixIt_Backend.MappingDefinition
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<CategoryForCreateDto, Category>();
            CreateMap<Category, CategoryForCreateDto>();

            CreateMap<SubCategories, SubCategoriesDto>();
            CreateMap<SubCategoriesDto, SubCategories>();

             CreateMap<Issue,IssueForListDto>().ForMember(x => x.Coords, opt => opt.MapFrom(issue => new CoordsDto
              {
                  Latitude = issue.Latitude,
                  Longitude = issue.Longitude,
              }))
              .ForMember(x => x.CategoryId, opt => opt.MapFrom(category => category.IssueCategories.Select(x => x.CategoryId).ToList()))
              .ForMember(x => x.SubCategoryId, opt => opt.MapFrom(subCategory => subCategory.IssueSubCategories.Select(x => x.SubCategoryId).ToList()));

            CreateMap<IssueDto, Issue>().ForMember(x => x.Status, opt => opt.MapFrom(d => "pending"))
                                        .ForMember(x => x.DateCreated, opt => opt.MapFrom(d => DateTime.Now.ToString()));
            CreateMap<Issue, IssueDto>();
            CreateMap<IssueForCreateDto, Issue>().ForMember(x => x.Status, opt => opt.MapFrom(d => "pending"))
                                        .ForMember(x => x.DateCreated, opt => opt.MapFrom(d => DateTime.Now.ToString()));

            CreateMap<IssueForEditDto, Issue>();

            CreateMap<RoleDto, Role>().ForMember(x => x.NormalizedName, opt => opt.MapFrom(dto => dto.Name.ToUpperInvariant()));
            CreateMap<Role, RoleDto>();

            CreateMap<DepartmentDto, Department>();
            CreateMap<Department, DepartmentDto>();

            CreateMap<Issue, DamageDto>().ForMember(x => x.Coordinates, opt => opt.MapFrom(o => new CoordsDto
            {
                Latitude = o.Latitude,
                Longitude = o.Longitude
            }))
            .ForMember(x => x.DateCreated, opt => opt.MapFrom(o => o.DateCreated.ToShortDateString().ToString()));


        }
    }
}
