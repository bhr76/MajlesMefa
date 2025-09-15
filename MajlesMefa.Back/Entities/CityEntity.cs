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
    public class CityEntity: AuditEntity<Guid>
    {   
        public string Name {get;set;}

        public Guid? ParentCityId { get; set; }

        public virtual CityEntity Parent { get; set; }
     
        public virtual ICollection<CityEntity> SubCities { get; set; } = new HashSet<CityEntity>();

        public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
        
        public virtual ICollection<SenatorProfileEntity> SenatorCities { get; set; } = new HashSet<SenatorProfileEntity>();
        
        public virtual ICollection<SenatorProfileEntity> SenatorBirthCities { get; set; } = new HashSet<SenatorProfileEntity>();
        
        public virtual ICollection<SenatorProfileEntity> SenatorHozeCities { get; set; } = new HashSet<SenatorProfileEntity>();
    }

    public class CityConfig : IEntityTypeConfiguration<CityEntity>
    {
        public void Configure(EntityTypeBuilder<CityEntity> builder)
        {
            builder.AddBaseConfig();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.SubCities)
                .HasForeignKey(x => x.ParentCityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
