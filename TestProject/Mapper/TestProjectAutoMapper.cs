using AutoMapper;
using Work.ApiModels;
using Work.Database;

namespace AutoMapper
{
    public class TestProjectAutoMapper : Profile
    {
        public TestProjectAutoMapper()
        {
            CreateMap<UserModelDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
