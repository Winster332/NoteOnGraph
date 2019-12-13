using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/server")]
    public class ServerController : ControllerBase
    {
        [HttpGet]
        [Route("getInfo")]
        public ServerInfo GetInfo()
        {
            return new ServerInfo
            {
                Version = "1.0.0"
            };
        }
    }
}