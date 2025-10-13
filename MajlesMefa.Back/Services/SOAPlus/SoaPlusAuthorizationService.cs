using MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus
{

    public class SoaPlusAuthorizationService : ISoaPlusAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private string? TokenValue;
        private DateTime? ExpireDateTime;

        public SoaPlusAuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task SendTokenRequest()
        {
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", _configuration["soaPlus:GrantType"]),
                new KeyValuePair<string, string>("client_id", _configuration["soaPlus:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["soaPlus:ClientSecret"])
            };
            var content = new FormUrlEncodedContent(nvc);
            using(var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"{_configuration["soaPlus:baseUrl"]}connect/token", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponse>(jsonResult);
                   
                    
                    
                    //new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                   ExpireDateTime = DateTime.Now.AddSeconds(authorizationResponse.ExpiresIn);
                   TokenValue = authorizationResponse.AccessToken;

                }
            }
        }

        public async Task<string> GetToken()
        {
            if(!ExpireDateTime.HasValue || ExpireDateTime <= DateTime.Now)
                await SendTokenRequest();
            return TokenValue;
        }

    }
}
