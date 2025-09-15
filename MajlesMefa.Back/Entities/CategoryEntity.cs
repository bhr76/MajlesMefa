using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;

namespace MajlesMefa.Back.Entities
{
    public class CategoryEntity: AuditEntity<Guid>
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        
        public virtual CategoryEntity Parent { get; set; }

        public bool IsCentralOffice { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// null: for all
        /// </summary>
        public DataEntryTypeEnum? DataEntryType { get; set; }

        public virtual ICollection<DataEntryEntity> DataEntries { get; set; } = new HashSet<DataEntryEntity>();
        public virtual ICollection<CategoryEntity> Categories { get; set; } = new HashSet<CategoryEntity>();
    }

    public class CategoryConfig : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(124);
            builder.Property(c => c.IsCentralOffice)
                .HasDefaultValueSql("0");

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Order);
        }
    }
}
