using Screechr.Model;
using Screechr.Repository.API;
using Screechr.Service.API;

namespace Screechr.Service
{
    public class UserService : BaseService<IUserRepository, User>, IUserService
    {
        public UserService(IUserRepository repository) : base(repository)
        {
        }

        public User? GetBySecretToken(string secretToken)
        {
            return _repository.GetBySecretToken(secretToken);
        }

        public User? GetByUserName(string username)
        {
            return _repository.GetByUserName(username);
        }

        internal override bool IsValidData(User entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.FirstName) ||
                string.IsNullOrEmpty(entity.LastName) || string.IsNullOrEmpty(entity.SecretToken) ||
                entity.DateCreated == DateTime.MinValue)
                return false;

            // Check unique user name and secret token
            if (entity.Id == 0)
            {
                if (_repository.GetByUserName(entity.UserName) != null)
                    return false;

                if (_repository.GetBySecretToken(entity.SecretToken) != null)
                    return false;
            }

            return true;
        }
    }
}
