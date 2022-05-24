using Microsoft.AspNetCore.Mvc;
using Screechr.Model;
using Screechr.Repository.API;

namespace Screechr.API.Controllers
{
    // Not implemented.
    public class TokenController : ControllerBase
    {
        
        private readonly ILogger<TokenController> _logger;
        private readonly IUserRepository _userRepository;

        public TokenController(ILogger<TokenController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public Token? GetToken(string userName, string secretToken)
        {
            // For future enhancement
            return null;
        }

        

        
    }
}
