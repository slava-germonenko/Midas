using System.ComponentModel.DataAnnotations;

namespace Midas.Auth.Core.Models;

public record AuthRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Device { get; set; } = string.Empty;

    [Required]
    public string IpAddress { get; set; } = string.Empty;
};