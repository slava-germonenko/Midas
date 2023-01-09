using AutoMapper;
using Midas.Api.Models.Auth;
using Midas.Auth.Core.Models;

namespace Midas.Api.Mapping.Profiles;

public class AuthRequestProfile : Profile
{
    public AuthRequestProfile()
    {
        CreateMap<LoginRequest, AuthRequest>()
            .ForMember(authReq => authReq.IpAddress, opt => opt.Ignore());
    }
}