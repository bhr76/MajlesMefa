using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class MokatebeEntity: BaseDataEntryTypeEntity
    {
        public MokatebeKonandeEnum MokatebeKonande { get; set; }

        public MokatebeTypeEnum MokatebeType { get;set; }
        public ContactEnum Contact { get;set; }

        public ResponseStatusEnum VaziatPasokh { get;set; }

        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }

        public string ShomareDabirkhane { get; set; }
        public string ShomareDabirkhaneMarkazi { get; set; }
        public DateTime TarikhDabirKhane { get; set; }
        public DateTime TarikhDabirKhaneMarkazi { get; set; }
        public decimal? Amount { get; set; }
    }

    public class MokatebeConfig : IEntityTypeConfiguration<MokatebeEntity>
    {
        public void Configure(EntityTypeBuilder<MokatebeEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Mokatebe);

            builder.Property(c => c.ShomareDabirkhane)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.ShomareDabirkhaneMarkazi)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
            builder.Property(c => c.Amount)
                .HasColumnType("decimal(19,4)");
            builder.Property(c => c.TarikhDabirKhane)
                .HasColumnType("datetime2");
            builder.Property(c => c.TarikhDabirKhaneMarkazi)
                .HasColumnType("datetime2");

        }
    }
}
