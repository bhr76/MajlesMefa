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
    public class PageEntity: AuditEntity<Guid>
    {
        public string Url { get; set; }

        public string Controller { get; set; }
        
        public string Action { get; set; }

        public string Title { get; set; }

        public string IconUrl { get; set; }

        public ICollection<PageRoleEntity> Roles { get; set; } = new HashSet<PageRoleEntity>();
    }

    public class PageConfig : IEntityTypeConfiguration<PageEntity>
    {
        public void Configure(EntityTypeBuilder<PageEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(c => c.IconUrl)
                .HasMaxLength(64);
            builder.Property(c => c.Url)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(128);
            builder.Property(c => c.Controller)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(64);
            builder.Property(c => c.Action)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(64);
        }
    }
}
