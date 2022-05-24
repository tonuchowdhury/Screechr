using Screechr.Model;
using Screechr.Repository.API;

namespace Screechr.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ScreechrContext context) : base(context)
        {

        }

        public override User? Get(long id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public override IList<User> GetAll(ListOptions options)
        {
            if (options == null)
                return new List<User>();

            if (options != null && options.SortBy == 1)
                return _context.Users.OrderByDescending(x => x.DateCreated)
                    .Skip((options.PageIndex - 1) * options.PageSize)
                    .Take(options.PageSize).ToList();

            return _context.Users.OrderBy(x => x.DateCreated)
                .Skip((options.PageIndex - 1) * options.PageSize)
                .Take(options.PageSize).ToList();
        }

        public override bool Insert(User entity)
        {
            try
            {
                var maxId = _context.Users.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id);
                
                entity.Id = maxId+1;
                _context.Users.Add(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }

        public override bool Update(User entity)
        {
            if (entity == null || entity.Id == 0)
                return false;

            var user = _context.Users.FirstOrDefault(x => x.Id == entity.Id);

            if (user == null)
                return false;

            try
            {
                _context.Users.Update(user);
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
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return false;

            try
            {
                _context.Users.Remove(user);

                return true;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }

        public User? GetByUserName(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == username);
        }

        public User? GetBySecretToken(string secretToken)
        {
            return _context.Users.FirstOrDefault(x => x.SecretToken == secretToken);
        }
    }
}