using Screechr.Model;

namespace Screechr.Repository.API
{
    public interface IBaseRepository<TEntity>
    {
        IList<TEntity> GetAll(ListOptions options);
        TEntity? Get(long id);
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(long id);
    }
}
