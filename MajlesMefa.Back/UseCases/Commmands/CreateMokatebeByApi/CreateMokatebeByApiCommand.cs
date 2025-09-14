using AutoMapper;
using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;

namespace MajlesMefa.Back.UseCases.Commmands.CreateMokatebeByApi
{
    public class CreateMokatebeByApiCommand : IRequest<Guid>, IMapping
    {
        public IModifyDataEntryDto DataEntryData { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? SenatorId { get; set; }


        public string SenatorUUID { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMokatebeByApiCommand,DataEntryEntity>();
        }
    }
}
