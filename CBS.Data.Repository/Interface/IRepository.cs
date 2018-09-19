using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;

namespace CBS.Data.Repository.Interface
{
    public interface IRepository<T>
    {
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        T Insert(T item);
        T Update(T item, Expression<Func<T, bool>> predicate);
        T UpdateSpecificValuesNotNull(T item, Expression<Func<T, bool>> predicate);
        bool Delete(Expression<Func<T, bool>> predicate);
        List<Dictionary<string, object>> Read(DbDataReader reader);
    }
}
