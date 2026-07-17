
namespace Pharmasy.Services;
public interface IPasswordService
{
    public Task<String> HashPasword(string password);
    public Task<bool> VerifyPassword(string password, string passwordHash);
}

public class PasswordService:IPasswordService
{
    public async Task<string> HashPasword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<bool> VerifyPassword(string password, string  passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password,passwordHash);
    }
}