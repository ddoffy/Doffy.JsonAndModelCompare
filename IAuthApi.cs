using Refit;

namespace Doffy.JsonAndModelCompare;

public interface IAuthApi
{
    [Post("/api/v1/identity/login")]
    Task<LoginResponse> Login([Body] LoginRequest request);
}

public class LoginResponse
{
    public string AccessToken { get; set; }
    public string ExpiresIn { get; set; }
    public string TokenType { get; set; }
}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}