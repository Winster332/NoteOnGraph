using System;
using Autofac.Integration.WebApi;
using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/schemeGraph")]
    [AutofacControllerConfiguration]
    public class SchemeGraphController : ControllerBase
    {
        private IRepository _repository;

        public SchemeGraphController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Route("getSchemeById/{schemeId}")]
        public SchemeGraph GetSchemeById(Guid schemeId)
        {
            var scheme = _repository.Read<SchemeGraph>(schemeId);

            return scheme;
        }
    }
}