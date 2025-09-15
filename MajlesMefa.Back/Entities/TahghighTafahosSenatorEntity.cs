using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;

namespace MajlesMefa.Back.Entities
{
    public class TahghighTafahosSenatorEntity
    {
        public Guid Id { get; set; }
        public Guid SenatorId { get; set; }
        public Guid TahghighTafahosId { get; set; }
        public virtual SenatorProfileEntity Senator { get; set; }
        public virtual TahghighTafahosEntity TahghighTafahos { get; set; }
    }

    public class TahghighTafahosSenatorConfig : IEntityTypeConfiguration<TahghighTafahosSenatorEntity>
    {
        public void Configure(EntityTypeBuilder<TahghighTafahosSenatorEntity> builder)
        {
            builder.HasOne(x => x.Senator)
                .WithMany(x => x.TahghighTafahosSenators)
                .HasForeignKey(x => x.SenatorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.TahghighTafahos)
                .WithMany(x => x.TahghighTafahosSenators)
                .HasForeignKey(x => x.TahghighTafahosId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
