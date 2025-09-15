using IdentityContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Services.Abstraction.External
{
    public interface IOptionService
    {
        TSetting GetConfig<TSetting>(string key);

        TSetting GetConfig<TSetting>() where TSetting : IIdentitySetting;
    }
}
