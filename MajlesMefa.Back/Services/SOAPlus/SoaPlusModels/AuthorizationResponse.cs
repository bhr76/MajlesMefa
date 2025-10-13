using Newtonsoft.Json;

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;

public class AuthorizationResponse
{
    public AuthorizationResponse(string accessToken, double expiresIn)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
    }

    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public double ExpiresIn { get; set; }
}