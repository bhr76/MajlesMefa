using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.UserDtos
{
    public class UserDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Mobile { get; set; }

        public bool IsActive { get; set; } = true;

        public string AvatarFileName { get; set; }

        public Guid UserId { get; set; }

        public Guid? CityId { get; set; }
        public string? CityName { get; set; }

        public Guid? OrganizationId { get; set; }
        public Guid? OrganizationParentId { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationParentName { get; set; }



        public string Username { get; set; }

        public string Email { get; set; }

        public RoleTypeEnum Role { get; set; }

    }
}
