using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.UserDtos
{
    public class BsUserDto
    {
        public string Name { get; set; }

        public RoleTypeEnum Role { get; set; }

        public EnumDto<RoleTypeEnum> RoleDesc => new EnumDto<RoleTypeEnum>
        {
            Value = this.Role,
            Name = this.Role.GetPersianName()
        };

        public string Mobile { get; set; }

        public bool IsActive { get; set; } = true;

        public string AvatarFileName { get; set; }

        public Guid Id { get; set; }

        public CityDto City { get; set; }

        public OrganizationDto Organization { get; set; }
    }
}
