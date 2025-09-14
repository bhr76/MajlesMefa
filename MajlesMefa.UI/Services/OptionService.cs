using IdentityContext.Models;
using IdentityContext.Services.Abstraction.External;

namespace MajlesMefa.UI.Services
{
    public class OptionService : IOptionService
    {
        private readonly IConfiguration _configuration;

        public OptionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TSetting GetConfig<TSetting>(string key)
        {
            var positionOptions = _configuration.GetSection(key)
                .Get<TSetting>();
            return positionOptions;
        }

        public TSetting GetConfig<TSetting>() where TSetting : IIdentitySetting
        {
            var setting = Activator.CreateInstance<TSetting>();
            var positionOptions = _configuration.GetSection(setting.SettingName)
                .Get<TSetting>();
            return positionOptions;
        }
    }
}
