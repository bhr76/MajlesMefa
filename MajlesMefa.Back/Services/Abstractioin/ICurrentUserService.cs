using MajlesMefa.Back.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Services.Abstractioin
{
    public interface ICurrentUserService
    {
        public CurrentUserDto GetCurrentUser();
    }
}
