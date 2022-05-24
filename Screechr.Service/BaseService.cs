using Screechr.Model;
using Screechr.Repository.API;
using Screechr.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screechr.Service
{
    public abstract class BaseService<TRepository, TEntity> : IBaseService<TEntity>
        where TRepository : IBaseRepository<TEntity>
        where TEntity : class

    {
        protected readonly TRepository _repository; 

        public BaseService(TRepository repository)
        {
            _repository = repository;
        }

        public bool Delete(long id)
        {
            return _repository.Delete(id);
        }

        public TEntity? Get(long id)
        {
            return _repository.Get(id);
        }

        public IList<TEntity> GetAll(ListOptions options)
        {
            return _repository.GetAll(options);
        }

        public bool Insert(TEntity entity)
        {
            if (IsValidData(entity))
                return _repository.Insert(entity);
            
            return false;
        }

        public bool Update(TEntity entity)
        {
            if (IsValidData(entity))
                return _repository.Update(entity);

            return false;
        }

        internal virtual bool IsValidData(TEntity entity)
        {
            return true;
        }
    }
}
