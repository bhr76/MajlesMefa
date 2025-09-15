using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums.Layehe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Enums.Tarh;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class TarhEntity : BaseDataEntryTypeEntity
    {
        public string Shenase { get; set; }

        public string NamayandeghanEmzaKonande { get; set; }

        public Guid RelatedComission { get; set; }

        public string Gardeshkar { get; set; }

        public string HamahangiBaraSherkat { get; set; }

        public string NamayandeghanBaraSherkat { get; set; }

        public string GhozareshMozakerat { get; set; }

        public NatijeBarresiEnum NatijeBarresi { get; set; }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }

        public string NazarNamayande { get; set; }

        public NatijeBarresiEnum Kollyat { get; set; }

        public NatijeBarresiEnum MavadTarh { get; set; }

        public NatijeBarresiEnum NatijeBarresiShoraNegahban { get; set; }

        public string NatijeBarresiShoraNegahbanDescription { get; set; }

        public string SavabeghEblagh { get; set; }

        public string GhozarshNahayi { get; set; }

        public string Havashi { get; set; }

        public string ErsalBeVazir { get; set; }
    }


    public class TarhConfig : IEntityTypeConfiguration<TarhEntity>
    {
        public void Configure(EntityTypeBuilder<TarhEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Tarh);

            builder.Property(c => c.Shenase)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(c => c.NamayandeghanEmzaKonande)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(c => c.RelatedComission)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(c => c.Gardeshkar)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.HamahangiBaraSherkat)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.NamayandeghanBaraSherkat)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(c => c.GhozareshMozakerat)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.NazarNamayande)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.NatijeBarresiShoraNegahbanDescription)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.SavabeghEblagh)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.GhozarshNahayi)
                .IsRequired()
                .HasMaxLength(2048);
            builder.Property(c => c.Havashi)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(c => c.ErsalBeVazir)
                .IsRequired()
                .HasMaxLength(1024);
        }
    }
}
