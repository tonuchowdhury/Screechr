using Screechr.Model;
using Screechr.Repository.API;
using System.Text;

namespace Screechr.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public readonly ScreechrContext _context;
        
        public BaseRepository(ScreechrContext context)
        {
            _context = context;

            // LoadDummyData();
        }

        //private void LoadDummyData()
        //{
        //    var uses = new List<User>
        //    {
        //        GetNewUser(1, "user1", "User", "1", "profile//image1"),
        //        GetNewUser(2, "user2", "User", "2", "profile//image2"),
        //    };

        //    _context.Users.AddRange(uses);
        //    _context.SaveChanges();

        //    var screeches = new List<Screech>
        //    {
        //        GetNewScreech(1, "screech1", _context.Users.Single(u => u.Id == 1)),
        //        GetNewScreech(2, "screech2", _context.Users.Single(u => u.Id == 1)),
        //        GetNewScreech(3, "screech1", _context.Users.Single(u => u.Id == 2)),
        //        GetNewScreech(4, "screech1", _context.Users.Single(u => u.Id == 2)),
        //    };

        //    _context.SaveChanges();
        //    _context.Screeches.AddRange(screeches);
        //}

        public virtual bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity? Get(long id)
        {
            throw new NotImplementedException();
        }

        public virtual IList<TEntity> GetAll(ListOptions options)
        {
            throw new NotImplementedException();
        }

        public virtual bool Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        private User GetNewUser(long id, string userName, string firstName, string lastName, string imageUrl)
        {
            var currentUtcDateTime = DateTime.UtcNow;
            return new User
            {
                Id = id,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                ProfileImageUrl = imageUrl,
                SecretToken = RandomString(32),
                DateCreated = currentUtcDateTime,
                DateModified = currentUtcDateTime,
            };
        }

        private Screech GetNewScreech(long id, string content, User creator)
        {
            var currentUtcDateTime = DateTime.UtcNow;
            return new Screech
            {
                Id=id,
                Content = content,
                Creator = creator,
                DateCreated = currentUtcDateTime,
                DateModified= currentUtcDateTime,
            };
        }

        private static readonly Random _random = new Random((int)DateTime.UtcNow.ToFileTime());

        private static string RandomString(int length = 0, bool includedSpaces = false)
        {
            var bytes = new byte[length > 0
                ? length
                : RandomValue(3, 64)];

            for (var i = 0; i < bytes.Length; i++)
            {
                var randomValue = RandomValue(65, 122);

                while (randomValue > 90 && randomValue < 97 && randomValue != 32)
                    randomValue = includedSpaces
                        ? 32
                        : RandomValue(65, 122);

                bytes[i] = (byte)randomValue;
            }

            return Encoding.ASCII.GetString(bytes);
        }

        private static int RandomValue(int min = 0, int max = 0)
        {
            return min >= max
                ? _random.Next()
                : _random.Next(min, max);
        }
    }
}
