using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class TazakorKatbiEntity: BaseDataEntryTypeEntity
    {
        public DateTime GheraatSahnDate { get; set; }

        public string PasokhNo { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
        public DateTime PasokhDate { get; set; }

        public string ShomareName { get; set; }

        public DateTime PishnevisDate { get; set; }
    }

    public class TazakorKatbiConfig : IEntityTypeConfiguration<TazakorKatbiEntity>
    {
        public void Configure(EntityTypeBuilder<TazakorKatbiEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.TazakorKatbi);
            builder.Property(x => x.GheraatSahnDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.PishnevisDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
            builder.Property(x => x.ShomareName)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
