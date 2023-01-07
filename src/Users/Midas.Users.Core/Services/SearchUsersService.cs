using Midas.Core;
using Midas.Core.Exceptions;
using Midas.Core.Models;
using Midas.Users.Core.Constants;

namespace Midas.Users.Core.Services;

public class SearchUsersService
{
    private readonly MidasContext _context;

    public SearchUsersService(MidasContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _context.Users.FindAsync(userId)
               ?? throw new NotFoundException(Errors.UserNotFound);
    }
}