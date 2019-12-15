using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;
using NoteOnGraph.Services;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/schemes")]
    public class SchemeController : ControllerBase
    {
        private IMongoCollection<Scheme> _schemes;
        private ISchemeService _schemeService;

        public SchemeController(IMongoCollection<Scheme> schemes, ISchemeService schemeService)
        {
            _schemes = schemes;
            _schemeService = schemeService;
        }
        
        [HttpGet]
        [Route("getSchemes")]
        public async Task<IList<Scheme>> GetSchemes()
        {
            var schemes = await _schemeService.GetSchemes();

            return schemes;
        }
    }
}