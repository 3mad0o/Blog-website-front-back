using Microsoft.EntityFrameworkCore;
using paf.api.Infrastructure;
using paf.api.Interfaces;
using paf.api.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace paf.api.Services
{
    public class GenericRepostiory<T> : IGenericRepostiory<T> where T : BaseEntity
    {
        private readonly AppDbContext appDbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepostiory(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            dbSet = appDbContext.Set<T>();
        }
        public  void Delete(T entiiy)
        {
            if (appDbContext.Entry(entiiy).State == EntityState.Detached)
            {
                appDbContext.Attach(entiiy);
            }
            dbSet.Remove(entiiy);
        }

        public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet; 
            if(skip !=null)
            {
              query =  query.Skip(skip.Value);
            }
            if(take != null)
            {
               query= query.Take(take.Value);
            }
            foreach (var include in includes)
            {
                query=query.Include(include);
            }
            return await query.ToListAsync();
        }

        public Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            query =dbSet.Where(x => x.Id == id);
            if(query != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.SingleOrDefaultAsync();
        }

        public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }
            if (take != null)
            {
                query = query.Take(take.Value);
            }
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public Task GetFilteredAsync(object[] values)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(T entiiy)
        {
           await dbSet.AddAsync(entiiy);
            await appDbContext.SaveChangesAsync();
            return entiiy.Id;
        }

        public async Task saveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async void update(T entiiy)
        {
            dbSet.Attach(entiiy);
            appDbContext.Entry(entiiy).State = EntityState.Modified;
            await saveChangesAsync();
        }
    }
}
