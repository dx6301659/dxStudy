using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dxStudyJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("value1")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value1" };
        }

        [HttpGet]
        [Route("value2")]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get2()
        {
            var auth = HttpContext.AuthenticateAsync();
            var userName = auth.Result.Principal?.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return new string[] { "value1", "value2", $"userName: {userName}" };
        }
    }
}
