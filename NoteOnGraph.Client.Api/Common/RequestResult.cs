using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Extensions;

namespace NoteOnGraph.Client.Api.Common
{
    public class RequestResult<T>
    {
        public T Result { get; set; }
        public HttpResponseMessage Response { get; set; }
        public HttpStatusCode StatusCode => Response.StatusCode;
        public Exception Exception { get; set; }
    }
}