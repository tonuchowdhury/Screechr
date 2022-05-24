using Screechr.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screechr.Service.API
{
    public interface IBaseService<TEntity>
    {
        IList<TEntity> GetAll(ListOptions options);
        TEntity? Get(long id);
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(long id);
    }
}
