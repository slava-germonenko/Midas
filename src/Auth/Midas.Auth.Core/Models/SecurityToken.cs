namespace Midas.Auth.Core.Models;

public record SecurityToken
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }

    public DateTime IssueDate { get; set; }
};