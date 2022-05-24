using Screechr.Model;

namespace Screechr.Repository.API
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetByUserName(string username);
        User? GetBySecretToken(string secretToken);
    }
}
