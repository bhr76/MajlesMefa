using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Extensions
{
    public static class IdentityResultErrorGenerator
    {
        public static void ThrowIfFail(this IdentityResult rslt)
        {
            if (!rslt.Succeeded)
            {
                var err = string.Join(Environment.NewLine, rslt.Errors.Select(x => "کد خطا " + x.Code + ":" + x.Description));
                throw new UnauthorizedAccessException(err);
            }

        }
    }
}
