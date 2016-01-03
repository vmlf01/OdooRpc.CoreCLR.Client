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
