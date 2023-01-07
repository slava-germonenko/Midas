using Midas.Core.Models;

namespace Midas.Api.Models.Users;

public class UserViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Pesel { get; set; } = string.Empty;

    public bool Active { get; set; }

    public ICollection<UserProperty> DynamicProperties { get; set; } = new List<UserProperty>();
}