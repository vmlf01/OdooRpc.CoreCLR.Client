using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Models;
using Xunit;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class OdooRpcClientTests
    {
        OdooConnectionInfo ConnectionInfo;
        IOdooRpcClient RpcClient;
        JsonRpcClientFactoryStub JsonRpcClientFactory;
        JsonRpcWebClientStub JsonRpcClient;

        public OdooRpcClientTests()
        {
            this.ConnectionInfo = new OdooConnectionInfo()
            {
                IsSSL = false,
                Host = "test.odoo",
                Port = 1234,
                Username = "aaa",
                Password = "bbb"
            };

            this.JsonRpcClient = new JsonRpcWebClientStub();
            this.JsonRpcClientFactory = new JsonRpcClientFactoryStub(this.JsonRpcClient);
            this.RpcClient = new OdooRpcClient(this.JsonRpcClientFactory, this.ConnectionInfo);
        }

        [Fact]
        public void Constructor_ShouldInitWithSessionInfo()
        {
            Assert.NotNull(RpcClient.SessionInfo);
        }

        [Fact]
        public void Constructor_ShouldInitWithUserLoggedOut()
        {
            Assert.False(RpcClient.SessionInfo.IsLoggedIn);
        }

        [Fact]
        public void SetUserId_ShouldSetUserLoggedIn()
        {
            RpcClient.SetUserId(1);

            Assert.True(RpcClient.SessionInfo.IsLoggedIn);
            Assert.Equal(1, RpcClient.SessionInfo.UserId);
        }

        [Fact]
        public async Task GetVersion_ShouldCallRpcAndReturnVersion()
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

            Assert.True(this.JsonRpcClient.WasCalled);
            Assert.Equal("9.0c", version.ServerVersion);
            Assert.Equal(1, version.ProtocolVersion);
        }

        [Fact]
        public async Task Authenticate_ShouldLoginAndSetUserId()
        {
            var response = new JsonRpcResponse<object>();
            response.Id = 1;
            response.Result = 99;
            this.JsonRpcClient.SetNextResponse(response);

            await RpcClient.Authenticate();

            Assert.True(this.JsonRpcClient.WasCalled);
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
