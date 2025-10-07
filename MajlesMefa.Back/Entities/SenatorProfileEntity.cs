using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;

namespace MajlesMefa.Back.Entities
{
    public class SenatorProfileEntity
    {
        public Guid UserId { get; set; }

        public string SenatorUID { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public Guid ComissionMembership { get; set; }

        public string SocailActivity { get; set; }

        public string SenaHistory { get; set; }

        public string JobHistory { get; set; }

        public string PoliticalTending { get; set; }

        public string PersonalFavorites { get; set; }

        public string FractionMembership { get; set; }

        public string SabegheHeyatReise { get; set; }

        public string SabegheEmzaEstizah { get; set; }

        public string Reshte { get; set; }

        public DateTime BirthDate { get; set; }

        public HozeEntekhabiEnum HozeEntekhabi { get; set; }

        public ShoghleGhalebEnum ShoghleGhaleb { get; set; }

        public MadrakTahsiliEnum MadrakTahsili { get; set; }

        public MahaleTahsilEnum MahaleTahsil { get; set; }

        public GerayeshSiasiEnum GerayeshSiasi { get; set; }

        public Guid? HozeCityId { get; set; }

        public Guid BirthCityId { get; set; }

        public Guid CityId { get; set; }

        public string ProfilePhoto { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual CityEntity City { get; set; }

        public virtual DastoorJalasatComissionEntity Commission { get; set; }

        public virtual CityEntity HozeCity { get; set; }

        public virtual CityEntity BirthCity { get; set; }

        public ICollection<TahghighTafahosSenatorEntity> TahghighTafahosSenators { get; set; } = new HashSet<TahghighTafahosSenatorEntity>();

        public virtual ICollection<SenatorBudgetEntity> SenatorBudgets { get; set; } = new HashSet<SenatorBudgetEntity>();
    }

    public class SenatorProfileConfig : IEntityTypeConfiguration<SenatorProfileEntity>
    {
        public void Configure(EntityTypeBuilder<SenatorProfileEntity> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(c => c.ProfilePhoto)
                .HasMaxLength(256);

            builder.Property(c => c.Mobile)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(128);

            builder.Property(c => c.ComissionMembership);

            builder.Property(c => c.SocailActivity)
                .HasMaxLength(2048);
            builder.Property(c => c.SenaHistory)
                .HasMaxLength(2048);
            builder.Property(c => c.JobHistory)
                .HasMaxLength(2048);
            builder.Property(c => c.PoliticalTending)
                .HasMaxLength(2048);
            builder.Property(c => c.PersonalFavorites)
                .HasMaxLength(2048);
            builder.Property(c => c.FractionMembership)
                .HasMaxLength(2048);
            builder.Property(c => c.SabegheHeyatReise)
                .HasMaxLength(2048);
            builder.Property(c => c.SabegheEmzaEstizah)
                .HasMaxLength(2048);
            builder.Property(c => c.Reshte)
                .HasMaxLength(2048);
            builder.Property(c => c.BirthDate)
                .HasColumnType("datetime2");


            builder.HasOne(x => x.User)
                .WithOne(x => x.SenatorProfile)
                .HasForeignKey<SenatorProfileEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.City)
                .WithMany(x => x.SenatorCities)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.BirthCity)
                .WithMany(x => x.SenatorBirthCities)
                .HasForeignKey(x => x.BirthCityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.HozeCity)
                .WithMany(x => x.SenatorHozeCities)
                .HasForeignKey(x => x.HozeCityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Commission)
               .WithMany(x => x.SenatorCommissions)
               .HasForeignKey(x => x.ComissionMembership)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
