using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class LayeheEntity : BaseDataEntryTypeEntity
    {
        public LayeheTarhTypeEnum Type { get; set; }

        public string ShomareSabt { get; set; }

        public DateTime ElamVosoolDate { get; set; }

        public DateTime BaresiKoliatDarSahnDate { get; set; }

        public DateTime EblaghDate { get; set; }

        public NatijeBarresiEnum NatijeBarresiCommission { get; set; }

        public NatijeBarresiEnum NatijeBarresiSahn { get; set; }

        public NatijeBarresiShoraEnum NatijeBarresiShora { get; set; }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }

        public string MajorCommissions { get; set; }

        public string MinorCommissions { get; set; }

    }


    public class LayeheConfig : IEntityTypeConfiguration<LayeheEntity>
    {
        public void Configure(EntityTypeBuilder<LayeheEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Layehe);
            builder.Property(c => c.ElamVosoolDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.BaresiKoliatDarSahnDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.EblaghDate)
                .HasColumnType("datetime2");
        }
    }
}
