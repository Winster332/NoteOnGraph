using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests
{
    public class JointsEndpointTests : NoteOnGraphTest
    {
        public JointsEndpointTests()
        {
        }


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

        [Fact]
        public async Task JointsEndpointTest_RemoveJoint_ReturnSuccess()
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
            

            var responseJoints = await Api.Joints.GetJointById(joint.Id);
            responseJoints.Exception.Should().BeNull();
            responseJoints.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseJoints.Result.Should().NotBeNull();

            var assertionJoint = responseJoints.Result;
            assertionJoint.Id.Should().Be(joint.Id);
            assertionJoint.From.Should().Be(nodeFrom.Id);
            assertionJoint.To.Should().Be(nodeTo.Id);
            
            nodeFrom.Outputs.Add(joint.Id);
            nodeTo.Inputs.Add(joint.Id);

            var assertionNodeFrom = await GetNodeById(nodeFrom.Id);
            var assertionNodeTo = await GetNodeById(nodeTo.Id);
            
            assertionNodeFrom.Should().BeEquivalentTo(nodeFrom);
            assertionNodeTo.Should().BeEquivalentTo(nodeTo);
            
            var responseRemove = await Api.Joints.RemoveJoint(joint.Id);
            responseRemove.Exception.Should().BeNull();
            responseRemove.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            
            var responseGet = await Api.Joints.GetJointById(joint.Id);
            responseGet.Exception.Should().BeNull();
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            responseGet.Result.Should().BeNull();
            
            var aNodeFrom = await GetNodeById(nodeFrom.Id);
            var aNodeTo = await GetNodeById(nodeTo.Id);

            aNodeFrom.Outputs.Should().HaveCount(0);
            aNodeTo.Inputs.Should().HaveCount(0);
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