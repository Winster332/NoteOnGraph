using System;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Extensions;
using NoteOnGraph.Models;

namespace NoteOnGraph.Client.Api.Endpoints
{
    public class ServerEndpointApi
    {
        private RestApi _rest;
        
        public ServerEndpointApi(RestApi rest)
        {
            _rest = rest;
        }

        /// <summary>
        /// Return server information
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult<ServerInfo>> GetInfo()
        {
            var response = await _rest.GetAsync<ServerInfo>("/api/server/getInfo", x => x.GetResponseData<ServerInfo>());

            return response;
        }
    }
}