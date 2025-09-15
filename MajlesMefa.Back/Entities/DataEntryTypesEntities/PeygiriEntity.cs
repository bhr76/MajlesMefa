using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class PeygiriEntity : AuditEntity<long>
    {
        public Guid DataEntryId { get; set; }
        public Guid PeygiriKonandeId { get; set; }
        public virtual OrganizationEntity PeygiriKonandeInfo { get; set; }

        public string Description { get; set; }

        public string PeygiriNumber { get; set; }

        public DateTime PeygiriDate { get; set; }

        public virtual DataEntryEntity DataEntry { get; set; }
    }

    public class PeygiriConfig : IEntityTypeConfiguration<PeygiriEntity>
    {
        public void Configure(EntityTypeBuilder<PeygiriEntity> builder)
        {
            builder.AddBaseConfig();

            builder.HasOne(x => x.PeygiriKonandeInfo)
                .WithMany(x => x.Peygiries)
                .HasForeignKey(x => x.PeygiriKonandeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.Description)
                .HasMaxLength(1024);
            builder.Property(c => c.PeygiriNumber)
               .HasMaxLength(128);
            builder.Property(c => c.PeygiriDate)
                .HasColumnType("datetime2");

            builder.HasOne(x => x.DataEntry)
                .WithMany(x => x.Peygiries)
                .HasForeignKey(x => x.DataEntryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}