using IdentityContext.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.LoginByRTokenCommand
{
    public class LoginByRTokenCommand: IRequest<TokenDto>
    {
        public string RefreshToken { get; set; }
    }
}
