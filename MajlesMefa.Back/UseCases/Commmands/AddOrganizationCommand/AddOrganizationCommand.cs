using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddOrganizationCommand
{
    public class AddOrganizationCommand: IRequest<Guid>, IMapping
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddOrganizationCommand, OrganizationEntity>();
        }
    }
}
