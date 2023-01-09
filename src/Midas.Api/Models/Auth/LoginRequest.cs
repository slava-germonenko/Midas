using System.ComponentModel.DataAnnotations;

namespace Midas.Api.Models.Auth;

public record LoginRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Device { get; set; } = string.Empty;
};