using AutoMapper;
using Midas.Core.Models;
using Midas.Users.Core.Models;

namespace Midas.Users.Core.Mapping.Profiles;

public class UserPropertyProfile : Profile
{
    public UserPropertyProfile()
    {
        CreateMap<UserPropertyDto, UserProperty>();
    }
}