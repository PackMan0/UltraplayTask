using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AbstractionProvider.Models;

namespace AbstractionProvider.Interfaces.Repositories
{
    public interface IRepository
    {
        void Insert<T>(T entity) where T : Base;

        void InsertCollection<T>(T entityCollection) where T : IEnumerable<Base>;

        void Delete<T>(T entity) where T : Base;

        IQueryable<T> SearchFor<T>(Expression<Func<T, bool>> predicate) where T : Base;

        IQueryable<T> GetAll<T>() where T : Base;

        T GetById<T>(Guid id) where T : Base;
    }
}