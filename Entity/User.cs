namespace Scente.API.Entity;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = "customer";   

    
}