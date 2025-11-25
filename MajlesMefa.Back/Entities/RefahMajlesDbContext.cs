using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MajlesMefa.Back.Entities
{
    public class RefahMajlesDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public RefahMajlesDbContext(DbContextOptions<RefahMajlesDbContext> options,
            ICurrentUserService currentUserService = null)
             : base(options)
        {
            _currentUserService = currentUserService;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(RefahMajlesDbContext).Assembly);

            //var entitiesAssembly = typeof(AuditEntity<>).Assembly;

            //typeof(RefahMajlesDbContext).RegisterConfigurations( builder, entitiesAssembly);

            builder.Entity<LoanEntity>(entity =>
            {
                entity.Property(e => e.TrackingCode)
                    .UseIdentityColumn(seed: 1000, increment: 1)
                    .ValueGeneratedOnAdd()
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                    
                // ایجاد ایندکس برای جستجوی سریع‌تر
                entity.HasIndex(e => e.TrackingCode)
                    .IsUnique();
            });
        }

        public DbSet<DataEntryEntity> DataEntries { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ActionReferenceEntity> ActionReferences { get; set; }

        public DbSet<NotghEntity> Notghs { get; set; }

        public DbSet<TazakorKatbiEntity> TazakorKatbis { get; set; }
        public DbSet<TazakorEntity> Tazakors { get; set; }
        public DbSet<LoanEntity> Loans { get; set; }

        public DbSet<TazakorShafahiEntity> TazakorShafahis { get; set; }

        public DbSet<MokatebeEntity> Mokatebes { get; set; }
        public DbSet<KhadamatEntity> Khadamat { get; set; }

        public DbSet<TarhEntity> Tarhs { get; set; }

        public DbSet<LayeheEntity> Layehes { get; set; }

        public DbSet<SovalEntity> Sovals { get; set; }

        public DbSet<TahghighTafahosEntity> TahghighTafahoses { get; set; }

        public DbSet<DastoorJalasatComissionEntity> DastoorJalasatComissions { get; set; }

        public DbSet<EzhaaratResaneeeEntity> EzhaaratResaneeees { get; set; }

        public DbSet<PeygiriEntity> Peygiries { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<OrganizationEntity> Organizations { get; set; }

        public DbSet<CityEntity> Cities { get; set; }

        public DbSet<SenatorProfileEntity> SenatorProfiles { get; set; }
        public DbSet<SenatorBudgetEntity> SenatorBudgets { get; set; }

        public DbSet<PageEntity> Pages { get; set; }
        public DbSet<BankEntity> Banks { get; set; }

        public DbSet<BussinessRoleEntity> BussinessRoles { get; set; }

        public DbSet<PageRoleEntity> PageRoles { get; set; }

        public DbSet<TahghighTafahosSenatorEntity> TahghighTafahosSenators { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public DbSet<KeywordEntity> Keywords { get; set; }

        public virtual DbSet<MolaghatEntity> Molaghats { get; set; }

        public override int SaveChanges()
        {
            if (_currentUserService != null)
            {
                ChangeTracker.AddCurrentUserData(_currentUserService.GetCurrentUser());
            }
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_currentUserService != null)
            {
                ChangeTracker.AddCurrentUserData(_currentUserService.GetCurrentUser());
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_currentUserService != null)
            {
                ChangeTracker.AddCurrentUserData(_currentUserService.GetCurrentUser());
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUserService != null)
            {
                ChangeTracker.AddCurrentUserData(_currentUserService.GetCurrentUser());
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
