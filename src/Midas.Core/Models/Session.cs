using System.ComponentModel.DataAnnotations;

namespace Midas.Core.Models;

public class Session
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])")]
    public string IpAddress { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }
}