using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class EzhaaratResaneeeEntity: BaseDataEntryTypeEntity
    {
        public string Manba { get; set; }

        public DateTime Date { get; set; }
    }


    public class EzhaaratResaneeeConfig : IEntityTypeConfiguration<EzhaaratResaneeeEntity>
    {
        public void Configure(EntityTypeBuilder<EzhaaratResaneeeEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.EzhaaratResaneee);

            builder.Property(c => c.Manba)
                .IsRequired()
                .HasMaxLength(128);
          
            builder.Property(c => c.Date)
                .HasColumnType("datetime2");
        }
    }
}
