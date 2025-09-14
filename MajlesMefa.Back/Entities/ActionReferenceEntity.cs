using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;

namespace MajlesMefa.Back.Entities
{
    public class ActionReferenceEntity: AuditEntity<Guid>
    {
        public string Description { get; set; }

        public Guid FromUserId { get; set; }

        public Guid ToUserId{ get; set; }

        public Guid DataEntryId { get; set; }

        public ActRefTypeEnum ActRefType { get; set; }
        
        public RefTypeEnum RefType { get; set; }

        public bool IsVisibleForSenator { get; set; }

        public virtual UserEntity FromUser { get; set; }
        
        public virtual UserEntity ToUser { get; set; }
        
        public virtual DataEntryEntity DataEntry { get; set; }

        public DateTime? SeenDateBySenator { get; set; }
        
        public DateTime? SeenDateByToUser { get; set; }
    }

    public class ActionReferenceConfig : IEntityTypeConfiguration<ActionReferenceEntity>
    {
        public void Configure(EntityTypeBuilder<ActionReferenceEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.SeenDateByToUser)
                .HasColumnType("datetime2");
            builder.Property(c => c.SeenDateBySenator)
                .HasColumnType("datetime2");

            builder.HasOne(x => x.ToUser)
                .WithMany(x => x.ToActionReferences)
                .HasForeignKey(x => x.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.FromUser)
                .WithMany(x => x.FromActionReferences)
                .HasForeignKey(x => x.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DataEntry)
                .WithMany(x => x.ActionReferences)
                .HasForeignKey(x => x.DataEntryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
