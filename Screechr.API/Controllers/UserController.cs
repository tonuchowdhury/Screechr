using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Screechr.Model;
using Screechr.Service.API;

namespace Screechr.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController<IUserService, User>
    {
        public UserController(ILogger<UserController> logger, IUserService service) : base(logger, service)
        {
            
        }

        [HttpGet("{id:long}")]
        public ActionResult Get(long id)
        {
            if (!IsAuthorized(_service, out var userId))
            {
                return Unauthorized();
            }

            var user = _service.Get(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public ActionResult Get()
        {
            if (!IsAuthorized(_service, out var userId))
            {
                return Unauthorized();
            }

            var options = new ListOptions();

            if (Request.Query != null && Request.Query.Count > 0)
            {
                options.PageIndex = string.IsNullOrEmpty(Request.Query["pageIndex"]) ? 1 : Convert.ToInt32(Request.Query["pageIndex"]);
                options.PageSize = string.IsNullOrEmpty(Request.Query["pageSize"]) ? 50 : Convert.ToInt32(Request.Query["pageSize"]);
                options.SortBy = string.IsNullOrEmpty(Request.Query["sortBy"]) ? 1 : Convert.ToInt32(Request.Query["sortBy"]);
            }

            var users = _service.GetAll(options);

            return Ok(users);
        }

        [HttpPost(Name = "AddUser")]
        public ActionResult Post([FromBody] object json)
        {
            if (!Helper.ValidateApiRequestBody(json, out var body)) 
                return BadRequest();

            var user = JsonConvert.DeserializeObject<User>(body.ToString());

            if (user != null)
            {
                var currentDateTime = DateTime.UtcNow;
                user.DateCreated = currentDateTime;
                user.DateModified = currentDateTime;
                user.SecretToken = Helper.RandomString(32);

                var success = _service.Insert(user);

                if (success)
                    return Ok(user);
            }

            return BadRequest();
        }

        [HttpPut]
        public ActionResult Put(int id, [FromBody] object json)
        {
            if (!IsAuthorized(_service, out var userId))
            {
                return Unauthorized();
            }

            var user = _service.Get(userId);

            if (user == null)
                return Unauthorized();

            if (id <= 0)
                return BadRequest();

            if (id != user.Id)
                return Unauthorized();

            if (!Helper.ValidateApiRequestBody(json, out var body))
                return BadRequest();

            var userToUpdate = JsonConvert.DeserializeObject<User>(body.ToString());

            if (userToUpdate != null)
            {
                userToUpdate.Id = id;
                userToUpdate.DateModified = DateTime.UtcNow;

                var success = _service.Update(userToUpdate);

                if (success)
                    return Ok(userToUpdate);
            }

            return BadRequest();
        }

        [HttpPut("updateprofileimage")]
        public ActionResult UpdateProfileImage(int id, [FromBody] object json)
        {
            if (!IsAuthorized(_service, out var userId))
            {
                return Unauthorized();
            }

            var user = _service.Get(userId);

            if (user == null)
                return Unauthorized();

            if (id <= 0)
                return BadRequest();

            if (id != user.Id)
                return Unauthorized();

            if (!Helper.ValidateApiRequestBody(json, out var body))
                return BadRequest();

            var userToUpdate = JsonConvert.DeserializeObject<User>(body.ToString());

            if (userToUpdate != null)
            {
                var existingUser = _service.Get(id);

                if (existingUser == null)
                    return BadRequest();

                existingUser.ProfileImageUrl = userToUpdate.ProfileImageUrl;
                existingUser.DateModified = DateTime.UtcNow;

                var success = _service.Update(existingUser);

                if (success)
                    return Ok(existingUser);
            }

            return BadRequest();
        }

    }
}
