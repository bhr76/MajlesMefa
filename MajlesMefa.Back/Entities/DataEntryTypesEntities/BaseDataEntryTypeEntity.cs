using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities.DataEntryTypesEntities
{
    public abstract class BaseDataEntryTypeEntity
    {
        public Guid DataEntryId { get; set; }

        public virtual DataEntryEntity DataEntry { get; set; }

        
    }

    public static class BaseDataEntryTypeConfig
    {
        public static void AddRelationDataEntryConfig<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<DataEntryEntity, TEntity?>>? navigationExpression) where TEntity : BaseDataEntryTypeEntity
        {
            builder.HasKey(x => x.DataEntryId);
            builder.HasOne(x => x.DataEntry)
                .WithOne(navigationExpression)
                .HasForeignKey<TEntity>(x => x.DataEntryId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void AddRelationDataEntryConfig<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<DataEntryEntity, IEnumerable<TEntity>?>>? navigationExpression) where TEntity : BaseDataEntryTypeEntity
        {
            builder.HasKey(x => x.DataEntryId);
            builder.HasOne(x => x.DataEntry)
                .WithOne(typeof(TEntity).Name.Replace("Entity", ""))
                .HasForeignKey("DataEntryId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
