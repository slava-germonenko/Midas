using System.ComponentModel.DataAnnotations;

namespace Midas.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), RegularExpression(@"\w{10,14}")]
    public string Phone { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(250), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), RegularExpression(@"\w{11}")]
    public string Pesel { get; set; } = string.Empty;

    public bool Active { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(400)]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    [Required(AllowEmptyStrings = false), StringLength(400)]
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public ICollection<UserProperty> DynamicProperties { get; set; } = new List<UserProperty>();
}