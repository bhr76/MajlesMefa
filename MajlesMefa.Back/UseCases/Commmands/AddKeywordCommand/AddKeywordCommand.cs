using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddKeywordCommand
{
    public class AddKeywordCommand: IRequest<Guid>, IMapping
    {
        public Guid DataEntryId { get; set; }

        public string Name { get; set; }

        //can negative
        public int RepeatCount { get; set; }

        public bool AddRepeatIfExist { get; set; } = true;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddKeywordCommand, KeywordEntity>();
        }
    }
}
