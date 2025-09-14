using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddCityCommand
{
    public class AddCityCommand:IRequest<Guid>, IMapping 
    {
        public string Name { get; set; }

        public Guid? ParentCityId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddCityCommand, CityEntity>();
        }
    }
}
