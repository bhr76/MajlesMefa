using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Enums.TahghighTafahos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public class TahghighTafahosEntity : BaseDataEntryTypeEntity
    {
        //private readonly RefahMajlesDbContext _context;

        //public TahghighTafahosEntity(RefahMajlesDbContext context) 
        //{
        //    _context= context;
        //}
        public DateTime Date { get; set; }

        public string ShomareName { get; set; }

        public string ShomareDaryaft { get; set; }

        public Guid Commission { get; set; }

        public string Mokhatab { get; set; }

        public TahghighTafahosVazyatEnum TahghighTafahosVazyat { get; set; }

        public virtual DastoorJalasatComissionEntity DastoorJalasatComission { get; set; }

        public ICollection<TahghighTafahosSenatorEntity> TahghighTafahosSenators { get; set; } = new HashSet<TahghighTafahosSenatorEntity>();


        //public void AddSenator(TahghighTafahosEntity tahghighTafahos, Guid senatorId)
        //{
        //    _context.TahghighTafahosSenators.Add(new TahghighTafahosSenatorEntity()
        //    {
        //        TahghighTafahos = tahghighTafahos,
        //        SenatorId = senatorId
        //    });
        //}

        //public async Task RemoveSenator(Guid tahghighTafahosId, Guid senatorId)
        //{

        //    var entity = await _context.TahghighTafahosSenators.SingleOrDefaultAsync(x => x.SenatorId == senatorId && x.TahghighTafahosId == tahghighTafahosId);
        //    _context.TahghighTafahosSenators.Remove(entity);
        //}

    }



    public class TahghighTafahosConfig : IEntityTypeConfiguration<TahghighTafahosEntity>
    {
        public void Configure(EntityTypeBuilder<TahghighTafahosEntity> builder)
        {
            builder.AddRelationDataEntryConfig(x => x.TahghighTafahos);

            builder.Property(c => c.ShomareName)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(c => c.ShomareDaryaft)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(c => c.Commission)
                .IsRequired();
            builder.Property(c => c.Mokhatab)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(c => c.Date)
                .HasColumnType("datetime2");
            builder.HasOne(c => c.DastoorJalasatComission)
                .WithMany(a => a.TahghighTafahos)
                .HasForeignKey("Commission")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
