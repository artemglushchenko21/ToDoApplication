using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.Data;

namespace ToDoMvc.Models.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        protected DbSet<T> _dbset { get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);

            return query.ToList();
        }

        public virtual async Task<T> Get(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual async Task<T> Get(string id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual T Get(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);

            return query.FirstOrDefault();
        }

        public virtual async Task<List<T>> GetList(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);

            var result = await query.ToListAsync();
            return result;
        }

        public async Task Insert(T entity)
        {
           await _dbset.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await _dbset.FindAsync(id);

            if (entity == null)
            {
                return;
            }

            _dbset.Remove(entity);
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }

        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = _dbset;

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            if (options.HasWhere)
            {
                foreach (var clause in options.WhereClauses)
                {
                    query = query.Where(clause);
                }
            }

            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }

            return query;
        }
    }
}
