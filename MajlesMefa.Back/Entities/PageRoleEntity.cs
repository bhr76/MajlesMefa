using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public class PageRoleEntity
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public Guid RoleId { get; set; }
        public virtual PageEntity Page { get; set; }
        public virtual BussinessRoleEntity Role { get; set; }
    }

    public class PageRoleConfig : IEntityTypeConfiguration<PageRoleEntity>
    {
        public void Configure(EntityTypeBuilder<PageRoleEntity> builder)
        {
            builder.HasOne(x => x.Page)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.PageId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Role)
                .WithMany(x => x.PageRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
