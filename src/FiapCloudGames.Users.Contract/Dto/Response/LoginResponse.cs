namespace FiapCloudGames.Users.Contract.Dto.Response;
public class LoginResponse
{
    public string IdToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public string Email { get; set; }
}
