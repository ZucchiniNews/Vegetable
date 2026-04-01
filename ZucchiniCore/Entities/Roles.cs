using Microsoft.AspNetCore.Identity;

namespace Zucchinimvc.Models;

public class Roles : IdentityRole
{
    public const string Admin = "Admin";
    public const string Editor = "Editor";
    public const string Writer = "Writer";
    public const string Reader = "Reader";
    public string Description { get; set; } = string.Empty;
}
    