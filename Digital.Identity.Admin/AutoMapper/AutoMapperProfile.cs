using AutoMapper;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;

namespace Digital.Identity.Admin.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<EditUserInput, ApplicationUser>();
        }
    }
}
