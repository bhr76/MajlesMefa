using System.ComponentModel.DataAnnotations.Schema;
using MajlesMefa.Back.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class LoanEntity: BaseDataEntryTypeEntity
    {
        [ForeignKey(nameof(User))]
        public Guid? UserId { get; set; }
        public virtual UserEntity User { get; set; }

        public Guid? RelatedBankId { get; set; }

        public virtual BankEntity RelatedBank { get; set; }

        public LoanTypeEnum LoanType { get; set; }

        public ResponseStatusEnum VaziatPasokh { get;set; }

        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }

        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string NationalNo { get; set; }
        public long Amount { get; set; }
     


        public Guid? SenatorBudgetId { get; set; }

        public virtual SenatorBudgetEntity SenatorBudget { get; set; }

        public int TrackingCode { get; set; }
    }

    public class LoanConfig : IEntityTypeConfiguration<LoanEntity>
    {
        public void Configure(EntityTypeBuilder<LoanEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Loan);
         
            builder.HasOne(c => c.User)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RelatedBank)
               .WithMany(x => x.LoanRelatedBanks)
               .HasForeignKey(x => x.RelatedBankId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SenatorBudget) 
                .WithMany(x => x.LoanRelatedBanks)
                .HasForeignKey(x => x.SenatorBudgetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(c => c.MobileNo)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(c => c.NationalNo)
                .IsRequired()
                .HasMaxLength(11);
            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(x => x.PasokhDate)
                .HasColumnType("datetime2");
            builder.Property(c => c.PasokhNo)
                .HasMaxLength(32);
           

        }
    }
}
