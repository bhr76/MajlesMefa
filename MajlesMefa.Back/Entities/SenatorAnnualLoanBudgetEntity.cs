using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MajlesMefa.Back.Entities
{
    public class SenatorAnnualLoanBudgetEntity : AuditEntity<Guid>
    {
        [ForeignKey(nameof(Senator))]
        public Guid SenatorId { get; set; }
        public virtual SenatorProfileEntity Senator { get; set; }

        // شمسی (مثلاً 1405)
        public int Year { get; set; }
        public long GharzolHasaneLimit { get; set; }
        public long MorabeheLimit { get; set; }
        public long GharzolHasaneUsed { get; set; }
        public long MorabeheUsed { get; set; }
    }

    public class SenatorAnnualLoanBudgetConfig : IEntityTypeConfiguration<SenatorAnnualLoanBudgetEntity>
    {
        public void Configure(EntityTypeBuilder<SenatorAnnualLoanBudgetEntity> builder)
        {
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.GharzolHasaneLimit).IsRequired();
            builder.Property(x => x.MorabeheLimit).IsRequired();
            builder.Property(x => x.GharzolHasaneUsed).IsRequired();
            builder.Property(x => x.MorabeheUsed).IsRequired();
            builder.HasOne(x => x.Senator)
                .WithMany(x => x.AnnualLoanBudgets)
                .HasForeignKey(x => x.SenatorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.SenatorId, x.Year }).IsUnique();
        }
    }
}
