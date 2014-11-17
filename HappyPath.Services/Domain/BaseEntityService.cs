using HappyPath.Services.Data.Context;
using HappyPath.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Domain
{
    public interface IBaseEntityService<T> where T : class, IBaseEntity, new()
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression, int count = 0, int pageOffset = -1);
        T Get(long id);
        void AddOrUpdate(T item);
        void AddOrUpdate(List<T> items);
        void Delete(long id);
        int Count(Expression<Func<T, bool>> expression);
    }

    public class BaseEntityService<TEntity> : IBaseEntityService<TEntity> where TEntity : class, IBaseEntity, new()
    {
        readonly IHappyPathSession _session;
        public BaseEntityService(IHappyPathSession session)
        {
            _session = session;
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, int count = 0, int pageOffset = -1)
        {
            var query = _session.All<TEntity>().OrderBy(x => x.Id).Where(expression);

            if (count > 0 && pageOffset >= 0)
            {
                query = query.Skip(pageOffset * count).Take(count);
            }

            return query;
        }

        public virtual TEntity Get(long id)
        {
            return _session.Single<TEntity>(x => x.Id == id);
        }

        public virtual void AddOrUpdate(TEntity item)
        {
            _session.AddOrUpdate<TEntity>(item);
            _session.CommitChanges();
        }

        public virtual void AddOrUpdate(List<TEntity> items)
        {
            //Batch this in 5000 chunks
            int batchSize = 5000;
            for (var i = 0; i < items.Count; i += batchSize)
            {
                var batch = items.Skip(i).Take(batchSize).ToList();

                _session.AddOrUpdate<TEntity>(batch);
                _session.CommitChanges();
            }
        }

        public virtual void Delete(long id)
        {
            _session.Delete<TEntity>(x => x.Id == id);
            _session.CommitChanges();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return _session.All<TEntity>().Where(expression).Count();
        }
    }
}
