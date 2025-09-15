using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class NotghEntity : BaseDataEntryTypeEntity
    {
        public DateTime JalaseAlaniDate { get; set; }

        public string Chekide { get; set; }

        public string PasokhNo { get; set; }

        public string GardeshErjaat { get; set; }

        public string AnswerFromProUnitNo { get; set; }
        public DateTime AnswerFromProUnitDate { get; set; }
        public DateTime PasokhDate { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
    }

    public class NotghConfig : IEntityTypeConfiguration<NotghEntity>
    {
        public void Configure(EntityTypeBuilder<NotghEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Notgh);
            builder.Property(x => x.JalaseAlaniDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.AnswerFromProUnitDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
            builder.Property(x => x.Chekide)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(x => x.GardeshErjaat)
                .IsRequired(false)
                .HasMaxLength(512);
            builder.Property(x => x.AnswerFromProUnitNo)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
