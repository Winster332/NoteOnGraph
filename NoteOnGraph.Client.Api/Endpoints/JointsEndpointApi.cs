using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Extensions;
using NoteOnGraph.Models;

namespace NoteOnGraph.Client.Api.Endpoints
{
    public class JointsEndpointApi
    {
        private RestApi _rest;
        
        public JointsEndpointApi(RestApi rest)
        {
            _rest = rest;
        }
        
        /// <summary>
        /// Create joint
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public async Task<RequestResult<Guid>> CreateJoint(Joint joint) =>
            await _rest.PutAsync<Guid, Joint>($"/api/joints/createJoint", joint, x => x.GetContentAsGuid());
        
        /// <summary>
        /// Get all joints
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult<List<Joint>>> GetJoints() =>
            await _rest.GetAsync<List<Joint>>("/api/joints/getJoints", x => x.GetResponseData<List<Joint>>());
        
        /// <summary>
        /// Get joint by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RequestResult<Joint>> GetJointById(Guid id) =>
            await _rest.GetAsync<Joint>($"/api/joints/getJointById/{id}", x => x.GetResponseData<Joint>());
        
        /// <summary>
        /// Remove joint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RequestResult<string>> RemoveJoint(Guid id) =>
            await _rest.DeleteAsync<string>($"/api/joints/removeJoint", id, x => string.Empty);
    }
}