namespace Domain.Entities;

public class Roles
{
    public const string Admin = "Admin";
    public const string Editor = "Editor";
    public const string Writer = "Writer";
    public const string Reader = "Reader";
    public string Description { get; set; } = string.Empty;
}
