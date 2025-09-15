using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using System.Linq.Expressions;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class LoanEntity: BaseDataEntryTypeEntity
    {
        public Guid? SuggestedBankId { get; set; }

        public virtual BankEntity SuggestedBank { get; set; }

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
    }

    public class LoanConfig : IEntityTypeConfiguration<LoanEntity>
    {
        public void Configure(EntityTypeBuilder<LoanEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.Loan);
            builder.HasOne(x => x.SuggestedBank)
               .WithMany(x => x.LoanSuggestedBanks)
               .HasForeignKey(x => x.SuggestedBankId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RelatedBank)
               .WithMany(x => x.LoanRelatedBanks)
               .HasForeignKey(x => x.RelatedBankId)
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
