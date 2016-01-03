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
                Database = "odoo_test",
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
    }
}
