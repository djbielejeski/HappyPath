using HappyPath.Services.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Data.Storage
{
    public interface ISession : IDisposable
    {
        // Commit
        void CommitChanges();
        Task CommitChangesAsync();

        // Get
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity, new();
        IQueryable<T> All<T>() where T : class, IBaseEntity, new();

        // Add or update
        void AddOrUpdate<T>(T item) where T : class, IBaseEntity, new();
        void AddOrUpdate<T>(IEnumerable<T> items) where T : class, IBaseEntity, new();

        // Delete
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity, new();
        void Delete<T>(T item) where T : class, IBaseEntity, new();
        void DeleteAll<T>() where T : class, IBaseEntity, new();
    }

    public class Session : ISession
    {
        readonly DbContext _context;
        public Session(DbContext context)
        {
            _context = context;
        }

        // Commit
        public void CommitChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // log the exception

                // rethrow
                throw ex;
            }
        }

        public async Task CommitChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Log the exception

                // rethrow
                throw ex;
            }
        }

        // Get
        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity, new()
        {
            IQueryable<T> all = All<T>();
            if (all == null || !all.Any())
            {
                return null;
            }

            return all.FirstOrDefault(expression);
        }

        public IQueryable<T> All<T>() where T : class, IBaseEntity, new()
        {
            return _context.Set<T>().AsQueryable<T>();
        }

        // Add or update
        public void AddOrUpdate<T>(T item) where T : class, IBaseEntity, new()
        {
            if (item.Id == 0)
            {
                AddItem(item);
            }
            else
            {
                var originalEntity = Single<T>(x => x.Id == item.Id);
                UpdateItem(item, originalEntity);
            }
        }

        public void AddOrUpdate<T>(IEnumerable<T> items) where T : class, IBaseEntity, new()
        {
            AddItems<T>(items.Where(x => x.Id == 0));
            UpdateItems<T>(items.Where(x => x.Id != 0));
        }


        // Private add or update functions
        private void AddItem<T>(T entity) where T : class, IBaseEntity
        {
            entity.CreateDateTime = DateTime.Now;
            TrackNew(entity);
        }

        private void AddItems<T>(IEnumerable<T> entities) where T : class, IBaseEntity
        {
            entities.ToList().ForEach(x => x.CreateDateTime = DateTime.Now);
            TrackNew(entities);
        }

        private void TrackNew<T>(T entity) where T : class, IBaseEntity
        {
            _context.Set<T>().Add(entity);
        }

        private void TrackNew<T>(IEnumerable<T> entities) where T : class, IBaseEntity
        {
            _context.Set<T>().AddRange(entities);
        }

        private void UpdateItem<T>(T entity, T originalEntity) where T : class, IBaseEntity
        {
            TrackChanged(entity);
        }

        private void UpdateItems<T>(IEnumerable<T> entities) where T : class, IBaseEntity, new()
        {
            TrackChanged(entities);
        }

        private void TrackChanged<T>(IEnumerable<T> entities) where T : class, IBaseEntity
        {
            var localSetIds = entities.Select(y => y.Id).ToList();
            var localSet = _context.Set<T>().Where<T>(x => localSetIds.Contains(x.Id)).ToList();

            entities.ToList().ForEach((entity) =>
            {
                var entry = _context.Entry<T>(entity);

                if (entry.State == EntityState.Detached)
                {
                    T attachedEntity = localSet.FirstOrDefault(x => x.Id == entity.Id);

                    if (attachedEntity != null)
                    {
                        var attachedEntry = _context.Entry<T>(attachedEntity);

                        entity.CreateDateTime = attachedEntity.CreateDateTime;
                        entity.UpdateDateTime = DateTime.Now;

                        attachedEntry.CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }

                }
            });
        }

        private void TrackChanged<T>(T entity) where T : class, IBaseEntity
        {
            var entry = _context.Entry<T>(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<T>();
                T attachedEntity = set.Find(entity.Id);

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry<T>(attachedEntity);

                    entity.CreateDateTime = attachedEntity.CreateDateTime;
                    entity.UpdateDateTime = DateTime.Now;

                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }

            }
        }


        // Delete
        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity, new()
        {
            var query = All<T>().Where(expression);
            _context.Set<T>().RemoveRange(query);
        }

        public void Delete<T>(T item) where T : class, IBaseEntity, new()
        {
            _context.Set<T>().Remove(item);
        }

        public void DeleteAll<T>() where T : class, IBaseEntity, new()
        {
            var query = All<T>();
            _context.Set<T>().RemoveRange(query);
        }



        // IDisposable
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
