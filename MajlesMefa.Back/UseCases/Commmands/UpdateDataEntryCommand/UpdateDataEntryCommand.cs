using AutoMapper;
using MediatR;
using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand
{
    public class UpdateDataEntryCommand: IRequest, IMapping
    {

        public Guid DataEntryId { get; set; }

        /// <summary>
        /// NotghDto, TazakorKatbiDto, TazakorShafahiDto
        /// </summary>
        public IModifyDataEntryDto DataEntryData { get; set; }

        public DataEntryTypeEnum DataEntryType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }
        
        public List<Guid> Moavenats { get; set; }

        public Guid? SenatorId { get; set; }
        public Guid? SenatorBudgetId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDataEntryCommand, DataEntryEntity>();
        }
    }
}
