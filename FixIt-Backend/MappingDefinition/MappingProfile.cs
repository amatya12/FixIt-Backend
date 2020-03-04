using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Model;
using FixIt_Model.Users;

namespace FixIt_Backend.MappingDefinition
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<IssueDto, Issue>();
            CreateMap<Issue, IssueDto>();

            CreateMap<RoleDto, Role>().ForMember(x => x.NormalizedName, opt => opt.MapFrom(dto => dto.Name.ToUpperInvariant()));
            CreateMap<Role, RoleDto>();

        }
    }
}
