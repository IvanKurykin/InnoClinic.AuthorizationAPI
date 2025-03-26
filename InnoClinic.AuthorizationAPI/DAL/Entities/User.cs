namespace DAL.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; }

    public User(Guid id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}