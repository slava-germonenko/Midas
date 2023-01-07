using AutoMapper;
using Midas.Core.Models;
using Midas.Users.Core.Models;

namespace Midas.Users.Core.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(user => user.Active, opt => opt.MapFrom(_ => true));
    }
}