using Microsoft.AspNetCore.Mvc;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/server")]
    public class ServerController : ControllerBase
    {
        [HttpGet]
        [Route("getVersion")]
        public string GetVersion()
        {
            return "1.0.0";
        }
    }
}