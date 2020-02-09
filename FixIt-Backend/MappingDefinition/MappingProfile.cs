using AutoMapper;
using FixIt_Backend.Dto;
using FixIt_Model;

namespace FixIt_Backend.MappingDefinition
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
