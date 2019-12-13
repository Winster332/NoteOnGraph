using System;
using System.Net.Http;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Endpoints;

namespace NoteOnGraph.Client.Api
{
    public class NoteOnGraphClient : IDisposable
    {
        public ServerEndpointApi Server { get; set; }
        public ProjectsEndpointApi Projects { get; set; }
        
        public RestApi Rest { get; set; }
        
        public NoteOnGraphClient(HttpClient client)
        {
            Rest = new RestApi(client);
            Server = new ServerEndpointApi(Rest);
            Projects = new ProjectsEndpointApi(Rest);
        }

        public void Dispose()
        {
            Rest = null;
            Server = null;
            Projects = null;
        }
    }
}