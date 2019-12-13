using Microsoft.AspNetCore.Mvc;

namespace NoteOnGraph.Web.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class FilesController : Controller
    {
        [HttpGet("GetRootFiles")]
        public void GetRootFiles()
        {
        }
    }
}