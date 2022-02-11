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
        private readonly ApplicationDbContext _context;

        private DbSet<T> dbset { get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);

            return query.ToList();
        }

        public virtual T Get(int id)
        {
            return dbset.Find(id);
        }

        public virtual T Get(string id)
        {
            return dbset.Find(id);
        }

        public virtual T Get(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);

            return query.FirstOrDefault();
        }

        public void Insert(T entity)
        {
            dbset.Add(entity);
        }

        public void Update(T entity)
        {
            dbset.Update(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = dbset;

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}
