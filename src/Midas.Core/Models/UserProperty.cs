using System.ComponentModel.DataAnnotations;

namespace Midas.Core.Models;

public class UserProperty
{
    [Required(AllowEmptyStrings = false), StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(200)]
    public string Value { get; set; } = string.Empty;
}