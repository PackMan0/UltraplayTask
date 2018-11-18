using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AbstractionProvider.Interfaces.Repositories;
using AbstractionProvider.Models;

namespace DataAccessLayer
{
    public class Repository : IRepository, IDisposable
    {
        private readonly UltraplayTaskDbContext _context;

        public Repository(UltraplayTaskDbContext context)
        {
            this._context = context;
        }

        public void Insert<T>(T entity) where T : Base
        {
            this._context.Add(entity);
        }

        public void InsertCollection<T>(T entityCollection) where T : IEnumerable<Base>
        {
            this._context.AddRange(entityCollection);
        }

        public void Delete<T>(T entity) where T : Base
        {
            this._context.Remove(entity);
        }

        public IQueryable<T> SearchFor<T>(Expression<Func<T, bool>> predicate) where T : Base
        {
            return this._context.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAll<T>() where T : Base
        {
            return this._context.Set<T>();
        }

        public T GetById<T>(Guid id) where T : Base
        {
            // Sidenote: the == operator throws NotSupported Exception!
            // 'The Mapping of Interface Member is not supported'
            // Use .Equals() instead
            return this._context.Set<T>().FirstOrDefault(e => e.ID == id);
        }

        public void Dispose()
        {
            this._context.SaveChanges();
        }
    }
}
