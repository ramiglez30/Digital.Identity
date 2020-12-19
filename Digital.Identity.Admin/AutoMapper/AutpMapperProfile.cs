using AutoMapper;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;

namespace Digital.Identity.Admin.AutoMapper
{
    public class AutpMapperProfile : Profile
    {
        public AutpMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>();

        }
    }
}
