using System;
using System.Net.Http;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Endpoints;

namespace NoteOnGraph.Client.Api
{
    public class NoteOnGraphClient : IDisposable
    {
        public SchemeGraphEndpointApi Schemes { get; set; }
        public JointsEndpointApi Joints { get; set; }
        public NodesEndpointApi Nodes { get; set; }
        public ServerEndpointApi Server { get; set; }
        public ProjectsEndpointApi Projects { get; set; }
        
        public RestApi Rest { get; set; }
        
        public NoteOnGraphClient(HttpClient client)
        {
            Rest = new RestApi(client);
            Server = new ServerEndpointApi(Rest);
            Projects = new ProjectsEndpointApi(Rest);
            Nodes = new NodesEndpointApi(Rest);
            Joints = new JointsEndpointApi(Rest);
            Schemes = new SchemeGraphEndpointApi(Rest);
        }

        public void Dispose()
        {
            Rest = null;
            Server = null;
            Projects = null;
            Nodes = null;
            Joints = null;
            Schemes = null;
        }
    }
}