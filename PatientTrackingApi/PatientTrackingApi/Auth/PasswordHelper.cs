using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static string GenerateSalt() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

    public static string HashPassword(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(combined));
    }
}