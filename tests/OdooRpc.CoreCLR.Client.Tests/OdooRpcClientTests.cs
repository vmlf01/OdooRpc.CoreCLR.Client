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

        private class OdooRpcCallTestParameters<T>
        {
            public Action<OdooRpcRequest> Validator { get; set; }
            public string Model { get; set; }
            public string Method { get; set; }
            public Func<Task<T>> ExecuteRpcCall { get; set; }
            public JsonRpcResponse<T> TestResponse { get; set; }
        }

        private async Task TestOdooRpcCall<T>(OdooRpcCallTestParameters<T> testParams)
        {
            await TestNonAuthenticatedOdooRpcCall(testParams);
            await TestAuthenticatedOdooRpcCall(testParams);
        }

        private async Task TestAuthenticatedOdooRpcCall<T>(OdooRpcCallTestParameters<T> testParams)
        {
            long userId = 1;

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
                Assert.Equal(testParams.Model, p.args[3]);
                Assert.Equal(testParams.Method, p.args[4]);
                Assert.IsAssignableFrom(typeof(object[]), p.args[5]);

                testParams.Validator(p);
            });

            this.JsonRpcClient.SetNextResponse(testParams.TestResponse);

            RpcClient.SetUserId(userId);
            var result = await testParams.ExecuteRpcCall();

            Assert.True(validatorCalled);
            Assert.True(this.JsonRpcClient.WasCalled);
            Assert.Equal(testParams.TestResponse.Result, result);
        }

        private async Task TestNonAuthenticatedOdooRpcCall<T>(OdooRpcCallTestParameters<T> testParams)
        {
            this.RpcClient.SessionInfo.Reset();
            Assert.False(RpcClient.SessionInfo.IsLoggedIn);
            await Assert.ThrowsAsync(
                typeof(InvalidOperationException),
                () => testParams.ExecuteRpcCall()
            );
        }
    }
}
