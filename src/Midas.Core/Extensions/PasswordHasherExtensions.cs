using Midas.Core.Contracts;
using Midas.Core.Models;

namespace Midas.Core.Extensions;

public static class PasswordHasherExtensions
{
    public static void SetPassword(this IPasswordHasher hasher, User user, string password)
    {
        var (hash, salt) = hasher.Hash(password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
    }

    public static bool PasswordIsValid(this IPasswordHasher hasher, User user, string password)
    {
        var hash = hasher.Hash(password, user.PasswordSalt);
        return hash.SequenceEqual(user.PasswordHash);
    }
}