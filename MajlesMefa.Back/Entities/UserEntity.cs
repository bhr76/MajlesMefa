using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Extensions;

namespace MajlesMefa.Back.Entities
{
    public class UserEntity: AuditEntity<Guid>
    {
        public string Name { get; set; }
        
        public string Mobile { get; set; }

        public bool IsActive { get; set; } = true;

        public string AvatarFileName { get; set; }

        public Guid UserId { get; set; }

        /// <summary>
        /// null : *
        /// </summary>
        public Guid? CityId { get; set; }

        /// <summary>
        /// null : *
        /// </summary>
        public Guid? OrganizationId { get; set; }

        public virtual CityEntity City { get; set; }

        public virtual OrganizationEntity Organization { get; set; }

        public virtual SenatorProfileEntity SenatorProfile { get; set; }

        public virtual ICollection<ActionReferenceEntity> FromActionReferences { get; set; } = new HashSet<ActionReferenceEntity>();
        
        public virtual ICollection<ActionReferenceEntity> ToActionReferences { get; set; } = new HashSet<ActionReferenceEntity>();

        public virtual ICollection<DataEntryEntity> SenatorsDataEntries { get; set; } = new HashSet<DataEntryEntity>();
        
        public virtual ICollection<DataEntryEntity> CreatorDataEntries { get; set; } = new HashSet<DataEntryEntity>();

        public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new HashSet<UserRoleEntity>();

        public virtual ICollection<SenatorBudgetEntity> SenatorBudgets { get; set; } = new HashSet<SenatorBudgetEntity>();
        public virtual ICollection<LoanEntity> Loans { get; set; } = new HashSet<LoanEntity>();
    }

    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(c => c.Mobile)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(c => c.IsActive)
                .HasDefaultValueSql("1");

            builder.HasOne(x => x.City)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Organization)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
