using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models.Parameters;
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
            var requestParameters = new OdooGetParameters("res.partner", new long[] { 7 }, new string[] { "name", "country_id", "comment" });

            var testPartner = new TestPartner()
            {
                comment = false,
                country_id = new object[] { 21, "Belgium" },
                id = 7,
                name = "Agrolait"
            };

            var response = new JsonRpcResponse<TestPartner[]>();
            response.Id = 1;
            response.Result = new TestPartner[] {
                testPartner
            };

            await TestOdooRpcCall(new OdooRpcCallTestParameters<TestPartner[]>()
            {
                Validator = (p) =>
                {
                    dynamic args = p.args[5];
                    Assert.Equal(requestParameters.Ids, args[0]);
                    Assert.Equal(requestParameters.Fields, args[1]);
                },
                Model = requestParameters.Model,
                Method = "read",
                ExecuteRpcCall = () => RpcClient.Get<TestPartner[]>(requestParameters),
                TestResponse = response
            });
        }

    }
}
