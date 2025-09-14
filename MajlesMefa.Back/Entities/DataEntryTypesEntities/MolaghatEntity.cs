using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.Utilities.Date;
using System.ComponentModel.DataAnnotations.Schema;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class MolaghatEntity : BaseDataEntryTypeEntity
    {
        public short Count { get; set; }

        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }

        public ResponseStatusEnum PasokhState { get; set; }

        public MolaghatLocationEnum Mahal { get; set; }

        public DateTime Date { get; set; }

        [NotMapped]
        public string DateMonth => this.Date.ToPersianDate().GetMonth();

        public string Fracsion { get; set; }

        public MolaghatTypeEnum MolaghatType {get;set;}
        public bool IsMolaghatBaVazir { get; set; } = false;
    }


    public class MolaghatConfig : IEntityTypeConfiguration<MolaghatEntity>
    {
        public void Configure(EntityTypeBuilder<MolaghatEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Molaghat);
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
            builder.Property(c => c.Date)
                .HasColumnType("datetime2");
        }
    }
}
