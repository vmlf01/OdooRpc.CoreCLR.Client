using System.Linq;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
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
        public async Task Search_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true)
            );

            var testResults = new long[] {
                7L, 18L, 12L, 14L, 17L, 19L
            };

            var response = new JsonRpcResponse<long[]>();
            response.Id = 1;
            response.Result = testResults;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<long[]>() {
                    Model = "res.partner",
                    Method = "search",
                    Validator = (p) => {
                        dynamic args = p.args[5];
                        Assert.Equal(1, args.Length);
                        Assert.Equal(new object[] {
                                new object[] { "is_company", "=", true },
                                new object[] { "customer", "=", true }
                            },
                            args[0]
                        );
                    },
                    ExecuteRpcCall = () => RpcClient.Search<long[]>(requestParameters),
                    TestResponse = response
                }
            );
        }
    }
}
