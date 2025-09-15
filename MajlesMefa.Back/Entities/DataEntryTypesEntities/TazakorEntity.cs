using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class TazakorEntity: BaseDataEntryTypeEntity
    {
        public DateTime GheraatSahnDate { get; set; }

        public string PasokhNo { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
        public DateTime PasokhDate { get; set; }

        public string ShomareName { get; set; }

        public DateTime PishnevisDate { get; set; }

        public DateTime NameVaseleDate { get; set; }
        public string NameVaseleNo { get; set; }
        public string NameVaseleDabirkhaneNo { get; set; }
        public VaseleAzEnum VaseleAz { get; set; } = VaseleAzEnum.None;

        public TazakorEnum TazakorType { get; set; }
    }

    public class TazakorConfig : IEntityTypeConfiguration<TazakorEntity>
    {
        public void Configure(EntityTypeBuilder<TazakorEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Tazakor);
            builder.Property(x => x.GheraatSahnDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.PishnevisDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.NameVaseleDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
            builder.Property(x => x.ShomareName)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
