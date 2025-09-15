using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete<TKey>(TKey id);
        Task DeleteAsync<TKey>(TKey id);
        Task<TEntity> FindAsync<Tkey>(Tkey id);
        IQueryable<TEntity> Tracking { get; }
        IQueryable<TEntity> NoTracking { get; }
    }
}
