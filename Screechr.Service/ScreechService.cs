using Screechr.Model;
using Screechr.Repository.API;
using Screechr.Service.API;

namespace Screechr.Service
{
    public class ScreechService : BaseService<IScreechRepository, Screech>, IScreechService
    {
        public ScreechService(IScreechRepository repository) : base(repository)
        {
        }

        internal override bool IsValidData(Screech entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Content) || entity.CreatorId == 0)
                return false;

            return true;
        }
    }
}
