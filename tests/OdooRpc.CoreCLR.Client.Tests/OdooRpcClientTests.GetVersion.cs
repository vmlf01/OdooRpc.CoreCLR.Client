using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Internals;
using JsonRpc.CoreCLR.Client.Models;
using Xunit;
using Newtonsoft.Json.Linq;
using System;
using OdooRpc.CoreCLR.Client.Tests.Helpers;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public partial class OdooRpcClientTests
    {
        [Fact]
        public async Task GetVersion_ShouldCallRpcWithCorrectParameters()
        {
            var validatorCalled = false;
            this.JsonRpcClient.SetNextRequestValidator((req) =>
            {
                validatorCalled = true;
                Assert.Equal("call", req.Method);
                var p = req.Params as OdooRpcRequest;

                Assert.NotNull(p);
                Assert.Equal("common", p.service);
                Assert.Equal("version", p.method);
                Assert.NotNull(p.args);
                Assert.NotNull(p.context);
            });

            var response = new JsonRpcResponse<OdooVersionInfo>();
            response.Id = 1;
            response.Result = new OdooVersionInfo()
            {
                ServerVersion = "9.0c",
                ProtocolVersion = 1
            };
            this.JsonRpcClient.SetNextResponse(response);

            var version = await RpcClient.GetOdooVersion();

            Assert.True(validatorCalled, "Validator was not called");
            Assert.True(this.JsonRpcClient.WasCalled, "Rpc was not called");
        }

        [Fact]
        public async Task GetVersion_ShouldReturnVersion()
        {
            var response = new JsonRpcResponse<OdooVersionInfo>();
            response.Id = 1;
            response.Result = new OdooVersionInfo()
            {
                ServerVersion = "9.0c",
                ProtocolVersion = 1
            };
            this.JsonRpcClient.SetNextResponse(response);

            var version = await RpcClient.GetOdooVersion();

            Assert.Equal("9.0c", version.ServerVersion);
            Assert.Equal(1, version.ProtocolVersion);
        }
    }
}
