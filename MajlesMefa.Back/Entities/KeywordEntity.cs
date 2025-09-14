using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;

namespace MajlesMefa.Back.Entities
{
    public class KeywordEntity: AuditEntity<Guid>
    {
        public Guid Id {get;set; }

        public string Name { get; set; }

        public int RepeatCount { get; set; }

        public Guid DataEntryId { get; set; }

        public virtual DataEntryEntity DataEntry { get; set; }
    }

    public class KeywordConfig : IEntityTypeConfiguration<KeywordEntity>
    {
        public void Configure(EntityTypeBuilder<KeywordEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);
            builder.HasOne(x => x.DataEntry)
                .WithMany(x => x.Keywords)
                .HasForeignKey(x => x.DataEntryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(x => new { x.Name, x.DataEntryId })
                .IsUnique();
        }
    }
}
