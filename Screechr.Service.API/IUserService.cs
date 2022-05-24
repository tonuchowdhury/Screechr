using Screechr.Model;

namespace Screechr.Service.API
{
    public interface IUserService : IBaseService<User>
    {
        User? GetByUserName(string username);
        User? GetBySecretToken(string secretToken);
    }
}
