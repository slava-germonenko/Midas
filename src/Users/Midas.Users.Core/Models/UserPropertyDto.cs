using System.ComponentModel.DataAnnotations;

namespace Midas.Users.Core.Models;

public class UserPropertyDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Value { get; set; } = string.Empty;
}