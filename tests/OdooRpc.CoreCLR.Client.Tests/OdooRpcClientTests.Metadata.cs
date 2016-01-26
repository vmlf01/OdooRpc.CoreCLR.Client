using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Internals;
using JsonRpc.CoreCLR.Client.Models;
using Xunit;
using Newtonsoft.Json.Linq;
using System;
using OdooRpc.CoreCLR.Client.Tests.Helpers;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public partial class OdooRpcClientTests
    {
        [Fact]
        public async Task GetMetadata_ShouldCallRpcWithCorrectParameters()
        {
            var metaParameters = new OdooMetadataParameters(
                "res.groups",
                new List<long>() { 4 }
            );

            TestMetadata testResults = new TestMetadata();
            testResults.xmlid = "base.group_user";
            testResults.id = 4;

            var response = new JsonRpcResponse<dynamic>();
            response.Id = 1;
            response.Result = testResults;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<dynamic>()
            {
                Model = "res.groups",
                Method = "get_metadata",
                Validator = (p) =>
                {
                    Assert.Equal(6, p.args.Length);

                    dynamic args = p.args[5];
                    Assert.Equal(1, args.Length);
                },
                ExecuteRpcCall = () => RpcClient.GetMetadata<dynamic>(metaParameters),
                TestResponse = response
            });
        }
    }
}
