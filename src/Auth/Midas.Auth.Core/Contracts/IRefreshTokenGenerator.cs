using Midas.Auth.Core.Models;

namespace Midas.Auth.Core.Contracts;

public interface IRefreshTokenGenerator
{
    public SecurityToken Generate(int userId);
}