using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class DastoorJalasatComissionEntity: BaseDataEntryTypeEntity
    {
        public string Ghozareshat { get; set; }

        public DateTime DateAndDay { get; set; }

        public virtual ICollection<SenatorProfileEntity> Senators { get; set; } = new HashSet<SenatorProfileEntity>();
        public virtual ICollection<SenatorProfileEntity> SenatorCommissions { get; set; } = new HashSet<SenatorProfileEntity>();
        public virtual ICollection<TahghighTafahosEntity> TahghighTafahos { get; set; }
    }


    public class DastoorJalasatComissionConfig : IEntityTypeConfiguration<DastoorJalasatComissionEntity>
    {
        public void Configure(EntityTypeBuilder<DastoorJalasatComissionEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.DastoorJalasatComission);

            builder.Property(c => c.Ghozareshat)
                .HasMaxLength(2048);
            builder.Property(c => c.DateAndDay)
                .HasColumnType("datetime2");
        }
    }
}
