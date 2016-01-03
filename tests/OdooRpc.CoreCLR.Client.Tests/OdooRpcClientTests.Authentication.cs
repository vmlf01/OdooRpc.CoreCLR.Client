using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Internals;
using JsonRpc.CoreCLR.Client.Models;
using Xunit;
using Newtonsoft.Json.Linq;
using System;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public partial class OdooRpcClientTests
    {
        [Fact]
        public void SetUserId_ShouldSetUserLoggedIn()
        {
            RpcClient.SetUserId(1);

            Assert.True(RpcClient.SessionInfo.IsLoggedIn);
            Assert.Equal(1, RpcClient.SessionInfo.UserId);
        }

        [Fact]
        public async Task Authenticate_ShouldCallRpcWithCorrectParameters()
        {
            var validatorCalled = false;
            this.JsonRpcClient.SetNextRequestValidator((req) =>
            {
                validatorCalled = true;
                Assert.Equal("call", req.Method);
                var p = req.Params as OdooRpcRequest;

                Assert.NotNull(p);
                Assert.Equal("common", p.service);
                Assert.Equal("authenticate", p.method);
                Assert.NotNull(p.context);

                Assert.Equal(this.ConnectionInfo.Database, p.args[0]);
                Assert.Equal(this.ConnectionInfo.Username, p.args[1]);
                Assert.Equal(this.ConnectionInfo.Password, p.args[2]);
                Assert.NotNull(p.args[3]);
            });

            var response = new JsonRpcResponse<object>();
            response.Id = 1;
            response.Result = 99;
            this.JsonRpcClient.SetNextResponse(response);

            await RpcClient.Authenticate();

            Assert.True(validatorCalled);
            Assert.True(this.JsonRpcClient.WasCalled);
        }

        [Fact]
        public async Task Authenticate_ShouldSetUserId()
        {
            var response = new JsonRpcResponse<object>();
            response.Id = 1;
            response.Result = 99;
            this.JsonRpcClient.SetNextResponse(response);

            await RpcClient.Authenticate();

            Assert.True(RpcClient.SessionInfo.IsLoggedIn);
            Assert.Equal(99, RpcClient.SessionInfo.UserId);
        }

        [Fact]
        public async Task Authenticate_ShouldNotLoginInvalidUser()
        {
            var response = new JsonRpcResponse<object>();
            response.Id = 1;
            response.Result = "false";
            this.JsonRpcClient.SetNextResponse(response);

            await RpcClient.Authenticate();

            Assert.True(this.JsonRpcClient.WasCalled);
            Assert.False(RpcClient.SessionInfo.IsLoggedIn);
        }

    }
}
