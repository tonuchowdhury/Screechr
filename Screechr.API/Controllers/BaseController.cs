using Microsoft.AspNetCore.Mvc;
using Screechr.Service.API;

namespace Screechr.API.Controllers
{
    public abstract class BaseController<TService, TEntity> : ControllerBase
        where TEntity : class
        where TService : IBaseService<TEntity>
    {
        protected readonly ILogger _logger;
        protected readonly TService _service;

        public BaseController(ILogger logger, TService service)
        {
            _logger = logger;
            _service = service;
        }

        protected bool IsAuthorized(IUserService userService, out long userId)
        {
            var token = Request.Headers.Authorization;
            userId = 0;

            if (string.IsNullOrEmpty(token))
                return false;

            var user = userService.GetBySecretToken(token);

            if (user == null)
                return false;

            userId = user.Id;

            return true;
        }
    }
}
