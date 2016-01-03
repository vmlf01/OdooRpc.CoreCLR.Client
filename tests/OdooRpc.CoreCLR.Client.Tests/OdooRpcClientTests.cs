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

        [Fact]
        public void SetUserId_ShouldSetUserLoggedIn()
        {
            RpcClient.SetUserId(1);

            Assert.True(RpcClient.SessionInfo.IsLoggedIn);
            Assert.Equal(1, RpcClient.SessionInfo.UserId);
        }

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


        private class TestPartner
        {
            public object comment { get; set; }
            public object country_id { get; set; }
            public object id { get; set; }
            public object name { get; set; }
        }

        [Fact]
        public async Task Get_ShouldCallRpcWithCorrectParameters()
        {
            long userId = 1;
            var testPartner = new TestPartner()
            {
                comment = false,
                country_id = new object[] { 21, "Belgium" },
                id = 7,
                name = "Agrolait"
            };

            var requestParameters = new OdooGetParameters("res.partner", new long[] { 7 }, new string[] { "name", "country_id", "comment" });

            var validatorCalled = false;
            this.JsonRpcClient.SetNextRequestValidator((req) =>
            {
                validatorCalled = true;
                Assert.Equal("call", req.Method);
                var p = req.Params as OdooRpcRequest;

                Assert.NotNull(p);
                Assert.Equal("object", p.service);
                Assert.Equal("execute_kw", p.method);
                Assert.NotNull(p.context);

                Assert.Equal(this.ConnectionInfo.Database, p.args[0]);
                Assert.Equal(userId, p.args[1]);
                Assert.Equal(this.ConnectionInfo.Password, p.args[2]);
                Assert.Equal(requestParameters.Model, p.args[3]);
                Assert.Equal("read", p.args[4]);
                Assert.IsAssignableFrom(typeof(object[]), p.args[5]);
                dynamic args = p.args[5];
                Assert.Equal(requestParameters.Ids, args[0]);
                Assert.Equal(requestParameters.Fields, args[1]);
            });

            var response = new JsonRpcResponse<TestPartner[]>();
            response.Id = 1;
            response.Result = new TestPartner[] {
                testPartner
            };

            this.JsonRpcClient.SetNextResponse(response);

            RpcClient.SetUserId(userId);
            var partners = await RpcClient.Get<TestPartner[]>(requestParameters);

            Assert.True(validatorCalled);
            Assert.True(this.JsonRpcClient.WasCalled);
            Assert.Equal(testPartner, partners[0]);
        }

        [Fact]
        public async Task Get_ShouldThrowExceptionWhenUserIsNotLoggedIn()
        {
            Assert.False(RpcClient.SessionInfo.IsLoggedIn);
            await Assert.ThrowsAsync(typeof(InvalidOperationException), () => RpcClient.Get<JObject[]>(new OdooGetParameters("res.partner", new long[] { 7 }, new string[] { "name", "country_id", "comment" })));
        }


    }
}
