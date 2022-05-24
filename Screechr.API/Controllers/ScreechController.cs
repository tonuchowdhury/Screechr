using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Screechr.Model;
using Screechr.Service.API;

namespace Screechr.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScreechController : BaseController<IScreechService, Screech>
    {
        private IUserService _userService;

        public ScreechController(ILogger<ScreechController> logger, IScreechService service, IUserService userService) : base(logger, service)
        {
            _userService = userService;
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var screech = _service.Get(id);

            if (screech == null)
                return NotFound();

            return Ok(screech);
        }

        [HttpGet]
        public ActionResult Get()
        {
            var options = new ListOptions();

            if (Request.Query != null && Request.Query.Count > 0)
            {
                options.PageIndex = string.IsNullOrEmpty(Request.Query["pageIndex"]) ? 1 : Convert.ToInt32(Request.Query["pageIndex"]);
                options.PageSize = string.IsNullOrEmpty(Request.Query["pageSize"]) ? 50 : Convert.ToInt32(Request.Query["pageSize"]);
                options.SortBy = string.IsNullOrEmpty(Request.Query["sortBy"]) ? 0 : Convert.ToInt32(Request.Query["sortBy"]);
                options.userName = String.IsNullOrEmpty(Request.Query["userName"]) ? string.Empty : Request.Query["userName"];
            }

            var users = _service.GetAll(options);

            return Ok(users);
        }

        [HttpPost]
        public ActionResult Post([FromBody] object json)
        {
            if (!IsAuthorized(_userService, out var userId))
            {
                return Unauthorized();
            }

            var user = _userService.Get(userId);

            if (user == null)
                return Unauthorized();

            if (!Helper.ValidateApiRequestBody(json, out var body))
                return BadRequest();

            var screech = JsonConvert.DeserializeObject<Screech>(body.ToString());

            if (screech != null)
            {
                var currentDateTime = DateTime.UtcNow;
                screech.DateCreated = currentDateTime;
                screech.DateModified = currentDateTime;
                screech.CreatorId = user.Id;
                
                var success = _service.Insert(screech);

                if (success)
                    return Ok(screech);
            }

            return BadRequest();
        }


        [HttpPut("updatecontent")]
        public ActionResult UpdateContent(int id, [FromBody] object json)
        {
            if (!IsAuthorized(_userService, out var userId))
            {
                return Unauthorized();
            }

            var user = _userService.Get(userId);

            if (user == null)
                return Unauthorized();

            if (id <= 0)
                return BadRequest();

            if (!Helper.ValidateApiRequestBody(json, out var body))
                return BadRequest();

            var screech = JsonConvert.DeserializeObject<Screech>(body.ToString());

            if (screech != null)
            {
                var currentScreech = _service.Get(id);

                if (currentScreech == null)
                    return BadRequest();

                if (currentScreech.CreatorId != user.Id)
                    return Unauthorized();

                currentScreech.Content = screech.Content;
                currentScreech.DateModified = DateTime.UtcNow;

                var success = _service.Update(currentScreech);

                if (success)
                    return Ok(screech);
            }

            return BadRequest();
        }
    }
}
