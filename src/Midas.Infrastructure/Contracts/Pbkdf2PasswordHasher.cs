using Midas.Core.Contracts;

namespace Midas.Infrastructure.Contracts;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    public (byte[] passwordHash, byte[] salt) Hash(string password)
    {
        throw new NotImplementedException();
    }

    public byte[] Hash(string password, byte[] salt)
    {
        throw new NotImplementedException();
    }
}