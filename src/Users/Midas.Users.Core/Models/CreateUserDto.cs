using System.ComponentModel.DataAnnotations;

namespace Midas.Users.Core.Models;

public record CreateUserDto
{
    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, RegularExpression(@"\w{10,14}")]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(250), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, RegularExpression(@"\w{11}")]
    public string Pesel { get; set; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    public ICollection<UserPropertyDto> Properties { get; set; } = new List<UserPropertyDto>();
};