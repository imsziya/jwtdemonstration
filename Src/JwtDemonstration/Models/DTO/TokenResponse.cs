namespace JwtDemonstration.Models.DTO;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public double ExpiresOn { get; set; }
}
