using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace YangdoDAO
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private YangdoDbContext dbContext;
        private DbSet<T> entities;

        public Repository(YangdoDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entities = this.dbContext.Set<T>();
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return entities.AsQueryable();
            else
                return entities.Where(predicate);
        }

        public IQueryable<T> GetListFromRawSql(string rawSql, List<SqlParameter> sqlParameters)
        {
            return entities.FromSqlRaw(rawSql, sqlParameters.ToArray());
        }

        public void Delete(T entity)
        {
            this.dbContext.Remove(entity);
            this.SaveChanges();
        }

        public void Insert(T entity)
        {
            entities.Add(entity);
            this.SaveChanges();
        }

        public void Update(T entity)
        {
            entities.Update(entity);
            this.SaveChanges();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        
    }
}
