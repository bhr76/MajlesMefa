using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class ActionRefrenceDto
    {
        public Guid Id { get; set; }

        public string RefUserName { get; set; }
        public string RefCityName { get; set; }
        public string RefParentCityName { get; set; }
        public string RefOrgName { get; set; }
        public string RefParentOrgName { get; set; }
        
        public string ActionerName { get; set; }
        public string ActionerCityName { get; set; }
        public string ActionerParentCityName { get; set; }
        public string ActionerOrgName { get; set; }
        public string ActionerParentOrgName { get; set; }

        public string Created { get; set; }
        public ActRefTypeEnum Action { get; set; }
        public RefTypeEnum RefType { get; set; }
        public string RefTypeDesc { get; set; }
        public string ActionDesc { get; set; }
        public string Description { get; set; }
    }
}
