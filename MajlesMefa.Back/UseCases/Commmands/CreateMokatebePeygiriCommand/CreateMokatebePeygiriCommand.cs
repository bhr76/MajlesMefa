using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand
{
    public class CreateMokatebePeygiriCommand : IRequest<long>, IMapping
    {
        public Guid DataEntryId { get; set; }

        public string PeygiriNumber { get; set; }

        public string Description { get; set; }

        public DateTime PeygiriDate { get; set; }

        public Guid PeygiriKonandeId { get; set; }

    public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMokatebePeygiriCommand, PeygiriEntity>();
        }
    }
}
