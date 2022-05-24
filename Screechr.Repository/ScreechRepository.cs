using Screechr.Model;
using Screechr.Repository.API;

namespace Screechr.Repository
{
    public class ScreechRepository : BaseRepository<Screech>, IScreechRepository
    {
        public ScreechRepository(ScreechrContext context) : base(context)
        {
        }

        public override Screech? Get(long id)
        {
            return _context.Screeches.FirstOrDefault(x => x.Id == id);
        }

        public override IList<Screech> GetAll(ListOptions options)
        {
            IEnumerable<Screech> screeches;

            if (options == null)
                return new List<Screech>();

            if (!string.IsNullOrEmpty(options.userName))
                screeches = _context.Screeches.Where(x => x.Creator.UserName == options.userName);
            else
                screeches = _context.Screeches;
            
            return options.SortBy == 1 
                ? screeches.OrderByDescending(s => s.DateCreated)
                    .Skip((options.PageIndex - 1) * options.PageSize)
                    .Take(options.PageSize).ToList() 
                : screeches.OrderBy(s => s.DateCreated)
                    .Skip((options.PageIndex - 1) * options.PageSize)
                    .Take(options.PageSize).ToList();
        }

        public override bool Insert(Screech entity)
        {
            try
            {
                var maxId = _context.Screeches.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id);

                entity.Id = maxId + 1;
                _context.Screeches.Add(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }

        public override bool Update(Screech entity)
        {
            if (entity == null || entity.Id == 0)
                return false;

            var screech = _context.Screeches.FirstOrDefault(x => x.Id == entity.Id);

            if (screech == null)
                return false;

            try
            {
                _context.Screeches.Update(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }

        public override bool Delete(long id)
        {
            var screech = _context.Screeches.FirstOrDefault(x => x.Id == id);

            if (screech == null)
                return false;

            try
            {
                _context.Screeches.Remove(screech);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }
    }
}
