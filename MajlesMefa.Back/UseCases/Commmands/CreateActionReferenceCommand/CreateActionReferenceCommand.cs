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

namespace MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand
{
    public class CreateActionReferenceCommand: IRequest<Guid>, IMapping
    {
        public ActRefTypeEnum Action { get; set; } 
        public RefTypeEnum RefType { get; set; }

        public Guid? RefrenceUserId { get; set; }

        public Guid DataEntryId { get; set; }

        public string Description { get; set; }

        public bool SetVisibilityForSenator { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateActionReferenceCommand, ActionReferenceEntity>()
                .ForMember(x => x.ActRefType, x => x.MapFrom(y => y.Action))
                .ForMember(x => x.RefType, x => x.MapFrom(y => y.RefType))
                .ForMember(x => x.ToUserId, x => x.MapFrom(y => y.RefrenceUserId))
                .ForMember(x => x.IsVisibleForSenator, x => x.MapFrom(y => y.SetVisibilityForSenator));
        }
    }
}
