using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Midas.Core;
using Midas.Core.Exceptions;
using Midas.Core.Models;
using Midas.Users.Core.Constants;
using Midas.Users.Core.Models;

namespace Midas.Users.Core.Services;

public class CreateUserService
{
    private readonly MidasContext _context;

    private readonly IMapper _mapper;

    public CreateUserService(MidasContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> CreateAsync(CreateUserDto userDto)
    {
        await EnsureUserCanBeCreatedAsync(userDto);

        var user = _mapper.Map<User>(userDto);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private async Task EnsureUserCanBeCreatedAsync(CreateUserDto userDto)
    {
        var duplicateUsers = await _context.Users
            .AsNoTracking()
            .Where(u => u.Email.Equals(userDto.Email)
                        || u.Phone.Equals(userDto.Phone)
                        || u.Pesel.Equals(userDto.Pesel)
            )
            .ToListAsync();

        if (duplicateUsers.Count == 0)
        {
            return;
        }

        if (duplicateUsers.Any(u => u.Email.Equals(userDto.Email)))
        {
            throw new DuplicateException(Errors.EmailIsInUse);
        }

        if (duplicateUsers.Any(u => u.Phone.Equals(userDto.Phone)))
        {
            throw new DuplicateException(Errors.PhoneIsInUse);
        }

        if (duplicateUsers.Any(u => u.Pesel.Equals(userDto.Pesel)))
        {
            throw new DuplicateException(Errors.PeselIsInUse);
        }
    }
}