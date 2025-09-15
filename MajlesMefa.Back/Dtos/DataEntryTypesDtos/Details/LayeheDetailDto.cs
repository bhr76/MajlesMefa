using AutoMapper;
using Duende.IdentityServer.Models;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class LayeheDetailDto : IMapping, IModifyDataEntryDto
    {
        public LayeheTarhTypeEnum Type { get; set; }

        public string ShomareSabt { get; set; }

        public DateTime ElamVosoolDate { get; set; }

        public List<Guid> MajorCommissions { get; set; }


        public List<string> MajorCommissionsStr { get; set; }
        public List<string> MinorCommissionsStr { get; set; }

        public List<Guid> MinorCommissions { get; set; }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }

        public DateTime EblaghDate { get; set; }

        public DateTime BaresiKoliatDarSahnDate { get; set; }

        public NatijeBarresiEnum NatijeBarresiCommission { get; set; }

        public NatijeBarresiEnum NatijeBarresiSahn { get; set; }

        public NatijeBarresiShoraEnum NatijeBarresiShora { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LayeheDetailDto, LayeheEntity>()
                .ReverseMap();
        }
    }
}
