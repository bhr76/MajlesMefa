using MediatR;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateUserCommand
{
    public class UpdateUserCommand: IRequest
    {
        public Guid UserId { get; set; }

        public string Password { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Guid? OrganizationId { get; set; }
        
        public Guid? OrganizationParentId { get; set; }

        public Guid? CityId { get; set; }

        public RoleTypeEnum Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
