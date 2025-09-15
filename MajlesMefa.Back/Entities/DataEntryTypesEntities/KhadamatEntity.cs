using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class KhadamatEntity : BaseDataEntryTypeEntity
    {
        public DateTime TarikhKhedmat { get; set; }
        public string Khedmat { get; set; }
    }

    public class KhadamatConfig : IEntityTypeConfiguration<KhadamatEntity>
    {
        public void Configure(EntityTypeBuilder<KhadamatEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Khadamat);

            builder.Property(c => c.TarikhKhedmat)
                .HasColumnType("datetime2");

            builder.Property(c => c.Khedmat)
                .HasMaxLength(2048);
        }
    }
}
