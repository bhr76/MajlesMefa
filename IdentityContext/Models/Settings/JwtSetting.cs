using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Models.Settings
{
    public class JwtSetting: IIdentitySetting
    {
        public string ValidAudience { get; set; }

        public string ValidIssuer { get; set; }

        public string Secret { get; set; }

        public int AccessTokenExpirationTimeSeconds { get; set; }

        public int RefreshTokenExpirationTimeSeconds { get; set; } 

        public string SettingName => "JWT";
    }
}
