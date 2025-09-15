using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;

namespace MajlesMefa.Back.Entities
{
    public class OrganizationEntity: AuditEntity<Guid>
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public virtual OrganizationEntity Parent { get; set; }
        
        public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
        
        public virtual ICollection<OrganizationEntity> SubOrganizations { get; set; } = new HashSet<OrganizationEntity>();

        public virtual ICollection<PeygiriEntity> Peygiries { get; set; } = new HashSet<PeygiriEntity>();

    }

    public class OrganizationConfig : IEntityTypeConfiguration<OrganizationEntity>
    {
        public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);
            builder.HasOne(c => c.Parent)
                .WithMany(x => x.SubOrganizations)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
