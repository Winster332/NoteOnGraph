using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.JointsEndpoint
{
    public class JointEndpointCreateJoint : NoteOnGraphTest
    {
        [Fact]
        public async Task JointsEndpointTest_CreateGetJoint_ReturnSuccess()
        {
            var nodeFrom = new Node
            {
                Id = Guid.NewGuid(),
                Inputs = new List<Guid>(),
                Outputs = new List<Guid>(),
                Title = "node-test-from",
                X = 12,
                Y = 3.5f
            };
            var nodeTo = new Node
            {
                Id = Guid.NewGuid(),
                Inputs = new List<Guid>(),
                Outputs = new List<Guid>(),
                Title = "node-test-to",
                X = 1f,
                Y = 9f
            };
            
            nodeFrom = await CreateNode(nodeFrom);
            nodeTo = await CreateNode(nodeTo);
            
            var joint = new Joint
            {
                Id = Guid.Empty,
                From = nodeFrom.Id,
                To = nodeTo.Id
            };

            var responseCreate = await Api.Joints.CreateJoint(joint);
            responseCreate.Exception.Should().BeNull();
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            joint.Id = responseCreate.Result;
            

            var responseJoints = await Api.Joints.GetJoints();
            responseJoints.Exception.Should().BeNull();
            responseJoints.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseJoints.Result.Should().HaveCount(1);

            var assertionJoint = responseJoints.Result.FirstOrDefault();
            assertionJoint.Id.Should().Be(joint.Id);
            assertionJoint.From.Should().Be(nodeFrom.Id);
            assertionJoint.To.Should().Be(nodeTo.Id);
            
            nodeFrom.Outputs.Add(joint.Id);
            nodeTo.Inputs.Add(joint.Id);

            var assertionNodeFrom = await GetNodeById(nodeFrom.Id);
            var assertionNodeTo = await GetNodeById(nodeTo.Id);
            
            assertionNodeFrom.Should().BeEquivalentTo(nodeFrom);
            assertionNodeTo.Should().BeEquivalentTo(nodeTo);
        }
        
        private async Task<Node> GetNodeById(Guid id)
        {
            var response = await Api.Nodes.GetNodeById(id);

            return response.Result;
        }
        
        private async Task<Node> CreateNode(Node node)
        {
            var response = await Api.Nodes.CreateNode(node);
            node.Id = response.Result;

            return node;
        }
    }
}