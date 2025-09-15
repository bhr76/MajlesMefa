using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateCategoryCommand
{
    public class UpdateCategoryCommand: IRequest, IMapping
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public int Order { get; set; } = 1;

        public bool IsCentralOffice { get; set; }
        /// <summary>
        /// null: for all
        /// </summary>
        public DataEntryTypeEnum? DataEntryType { get; set; } = DataEntryTypeEnum.Mokatebe;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCategoryCommand, CategoryEntity>();
        }
    }
}
