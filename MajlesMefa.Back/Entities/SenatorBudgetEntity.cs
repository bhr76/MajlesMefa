using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public class SenatorBudgetEntity : AuditEntity<Guid>
    {
        [ForeignKey(nameof(Senator))]
        public Guid SenatorId { get; set; }
        public virtual SenatorProfileEntity Senator { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }

        public long Amount { get; set; }

        public string ExecDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public SenatorRequestLoanTypeEnum LoanType { get; set; }
        public virtual ICollection<LoanEntity> LoanRelatedBanks { get; set; } = new HashSet<LoanEntity>();

    }

    public class SenatorBudgetConfig : IEntityTypeConfiguration<SenatorBudgetEntity>
    {
        public void Configure(EntityTypeBuilder<SenatorBudgetEntity> builder)
        {
            builder.AddBaseConfig();
            
            builder.Property(c => c.Amount)
                .IsRequired();
            
            builder.Property(c => c.ExecDate)
                .HasMaxLength(128);
            
            builder.Property(c => c.RegisterDate)
                .HasColumnType("datetime2");

            builder.HasOne(c => c.Senator)
                .WithMany(x => x.SenatorBudgets)
                .HasForeignKey(x => x.SenatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User)
                .WithMany(x => x.SenatorBudgets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.SenatorId);
            builder.HasIndex(x => x.UserId);
        }
    }
}