using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Extensions;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;

namespace MajlesMefa.Back.Entities
{
    public class BankEntity: AuditEntity<Guid>
    {   
        public string Name {get;set;}

        
        public virtual ICollection<LoanEntity> LoanSuggestedBanks { get; set; } = new HashSet<LoanEntity>();
        public virtual ICollection<LoanEntity> LoanRelatedBanks { get; set; } = new HashSet<LoanEntity>();
    }

    public class BankConfig : IEntityTypeConfiguration<BankEntity>
    {
        public void Configure(EntityTypeBuilder<BankEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(256);

        }
    }
}
