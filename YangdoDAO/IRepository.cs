using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace YangdoDAO
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);

        IQueryable<T> GetListFromRawSql(string rawSql, List<SqlParameter> sqlParameters);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        void SaveChanges();
    }
}
