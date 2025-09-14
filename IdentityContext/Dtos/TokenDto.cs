using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdentityContext.Dtos
{
    public class TokenDto
    {
        public string RefreshToken { get; set; }

        public int RefresshTokenExpiresInSeconds { get; set; }

        public string AccessToken { get; set; }
        public string SessionToken { get; set; }

        public int AccessTokenExpiresInSeconds { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }

        public string TokenType { get; set; } = "beare";

    }
}
