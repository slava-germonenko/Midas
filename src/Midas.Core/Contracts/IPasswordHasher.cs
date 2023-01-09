namespace Midas.Core.Contracts;

public interface IPasswordHasher
{
    public (byte[] passwordHash, byte[] salt) Hash(string password);

    public byte[] Hash(string password, byte[] salt);
}