using IdentityContext.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.LoginCommand
{
    public class LoginCommand: IRequest<TokenDto>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
