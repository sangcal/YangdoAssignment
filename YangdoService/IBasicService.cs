using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace YangdoService
{
    public interface IBasicService<T>
    {
        T GetById(int id);

        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);

        IQueryable<T> GetListByFilters(Dictionary<string, string> filters);

        // Insert
        void Insert(T entity);

        // Update
        void Update(T entity);

        // Delete
        void Delete(T entity);
    }
}
