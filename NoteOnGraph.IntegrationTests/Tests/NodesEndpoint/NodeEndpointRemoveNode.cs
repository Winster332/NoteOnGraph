using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.NodesEndpoint
{
    public class NodeEndpointRemoveNode : NoteOnGraphTest
    {
        [Fact]
        public async Task NodeEndpointTest_RemoveNode_ReturnSuccess()
        {
            var node = new Node
            {
                Id = Guid.NewGuid(),
                Inputs = new List<Guid>(),
                Outputs = new List<Guid>(),
                Title = "node-test",
                X = 12,
                Y = 3.5f
            };
            var responseCreate = await Api.Nodes.CreateNode(node);
            node.Id = responseCreate.Result;
            
            var resGetNode = await Api.Nodes.GetNodeById(node.Id);
            node.DataId = resGetNode.Result.DataId;

            var responseRemove = await Api.Nodes.RemoveNode(node.Id);
            responseRemove.Exception.Should().BeNull();
            responseRemove.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseRemove.Result.Should().BeEquivalentTo(string.Empty);
            
            var responseGet = await Api.Nodes.GetNodeById(node.Id);
            responseGet.Exception.Should().BeNull();
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            responseGet.Result.Should().BeNull();

            var responseGetAll = await Api.Nodes.GetNodes();
            responseGetAll.Exception.Should().BeNull();
            responseGetAll.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGetAll.Result.Should().HaveCount(0);

            var resData = await Api.Nodes.GetDataByNodeId(node.Id);
            resData.Exception.Should().BeNull();
            resData.Result.Should().BeNull();
            resData.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }
    }
}