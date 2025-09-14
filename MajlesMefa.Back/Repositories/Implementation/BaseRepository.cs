using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public abstract class BaseRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        protected readonly RefahMajlesDbContext _context;

        protected BaseRepository(RefahMajlesDbContext context)
        {
            _context = context;
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Entry(entity).State = EntityState.Added;
        }

        public virtual void Delete(TEntity entity)
        {
            try
            {

                _context.Set<TEntity>().Entry(entity).State = EntityState.Deleted;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
           
        }
        
        public virtual void Delete<TKey>(TKey id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Entry(entity).State = EntityState.Deleted;
        }

        public virtual async Task DeleteAsync<TKey>(TKey id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            _context.Set<TEntity>().Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Entry(entity).State = EntityState.Modified;
        }
        
        public virtual async Task<TEntity> FindAsync<Tkey>(Tkey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> Tracking => _context.Set<TEntity>()
            .AsQueryable();
        public IQueryable<TEntity> NoTracking => _context.Set<TEntity>()
            .AsNoTracking()
            .AsQueryable();
    }
}
