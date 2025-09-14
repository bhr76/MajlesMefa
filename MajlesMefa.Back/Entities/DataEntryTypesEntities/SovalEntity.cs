using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Enums.Soval;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class SovalEntity: BaseDataEntryTypeEntity
    {
        public Guid Commission { get; set; }
        
        public DateTime KarbargDate { get; set; }

        public string DabirkhaneMakaziNo { get; set; }

        public QuestionStatusEnum QuestionStatus { get; set; }

        public string JalasatDakheli { get; set; }
        
        public string BarresiDarCommission { get; set; }
        
        public string BarresiDarSahn { get; set; }

    }


    public class SovalConfig : IEntityTypeConfiguration<SovalEntity>
    {
        public void Configure(EntityTypeBuilder<SovalEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Soval);
            builder.Property(x => x.KarbargDate)
                .HasColumnType("datetime2");
            builder.Property(x => x.Commission)
                .IsRequired();
            builder.Property(x => x.DabirkhaneMakaziNo)
                .HasMaxLength(32);
            builder.Property(x => x.JalasatDakheli)
                .HasMaxLength(2048);
            builder.Property(x => x.BarresiDarCommission)
                .HasMaxLength(2048);
            builder.Property(x => x.BarresiDarSahn)
                .HasMaxLength(1024);
        }
    }
}
