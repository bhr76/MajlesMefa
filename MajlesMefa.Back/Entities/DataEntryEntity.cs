using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Extensions;
using System.Reflection.Emit;

namespace MajlesMefa.Back.Entities
{
    public class DataEntryEntity : AuditEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DataEntryTypeEnum DataEntryType { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? SenatorId { get; set; }
        
        public Guid CreatorId { get; set; }

        public virtual UserEntity Senator { get; set; }
        
        public virtual UserEntity Creator { get; set; }

        public virtual ICollection<ActionReferenceEntity> ActionReferences { get; set; } = new HashSet<ActionReferenceEntity>();

        public virtual NotghEntity Notgh {get;set;}
        
        public virtual TazakorKatbiEntity TazakorKatbi {get;set;}

        public virtual TazakorEntity Tazakor {get;set;}
        
        public virtual TazakorShafahiEntity TazakorShafahi {get;set;}
        public virtual LoanEntity Loan {get;set;}
        
        public virtual MokatebeEntity Mokatebe { get;set;}
        
        public virtual TarhEntity Tarh { get;set;}

        public virtual LayeheEntity Layehe { get;set;}
        
        public virtual SovalEntity Soval { get;set;}
        
        public virtual EzhaaratResaneeeEntity EzhaaratResaneee { get;set;}
        
        public virtual DastoorJalasatComissionEntity DastoorJalasatComission { get;set;}

        public virtual TahghighTafahosEntity TahghighTafahos { get;set;}

        public virtual CategoryEntity Category { get; set; }
        
        public virtual MolaghatEntity Molaghat { get; set; }
        public virtual KhadamatEntity Khadamat { get; set; }

        public virtual string Moavenats { get; set; }

        public virtual ICollection<PeygiriEntity> Peygiries { get; set; } = new HashSet<PeygiriEntity>();

        public virtual ICollection<KeywordEntity> Keywords { get; set; } = new HashSet<KeywordEntity>();
    }

    public class DataEntryConfig : IEntityTypeConfiguration<DataEntryEntity>
    {
        public void Configure(EntityTypeBuilder<DataEntryEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(4096);
            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasMaxLength(2048);
            builder.Property(c => c.CategoryId)
                .IsRequired(false);
            builder.Property(c => c.Moavenats)
                .HasMaxLength(1024);
            builder.HasOne(x => x.Creator)
                .WithMany(x => x.CreatorDataEntries)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Senator)
                .WithMany(x => x.SenatorsDataEntries)
                .HasForeignKey(x => x.SenatorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.DataEntries)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.SenatorId);
            builder.HasIndex(x => x.CreatorId);

            //builder.Entity<DataEntryEntity>()
            //    .HasIndex(entity => new { entity.Title, entity.SenatorId }).IsUnique();
        }
    }
}
