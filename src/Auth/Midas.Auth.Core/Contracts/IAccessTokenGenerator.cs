using Midas.Auth.Core.Models;

namespace Midas.Auth.Core.Contracts;

public interface IAccessTokenGenerator
{
    public SecurityToken Generate(int userId);
}