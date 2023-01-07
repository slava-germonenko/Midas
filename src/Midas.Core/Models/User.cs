using System.ComponentModel.DataAnnotations;

namespace Midas.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

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

    public bool Active { get; set; }

    [Required, StringLength(400)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, StringLength(400)]
    public string PasswordSalt { get; set; } = string.Empty;

    public ICollection<UserProperty> DynamicProperties { get; set; } = new List<UserProperty>();
}