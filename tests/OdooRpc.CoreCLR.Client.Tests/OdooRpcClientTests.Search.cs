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

            await TestOdooRpcCall(new OdooRpcCallTestParameters<long[]>()
            {
                Model = "res.partner",
                Method = "search",
                Validator = (p) =>
                {
                    Assert.Equal(6, p.args.Length);

                    dynamic args = p.args[5];
                    Assert.Equal(2, args[0].Length);
                    Assert.Equal(
                        new object[]
                        {
                            new object[] { "is_company", "=", true },
                            new object[] { "customer", "=", true }
                        },
                        args[0]
                    );
                },
                ExecuteRpcCall = () => RpcClient.Search<long[]>(requestParameters),
                TestResponse = response
            });
        }

        [Fact]
        public async Task Search_WithPagination_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true),
                new OdooPaginationParameters(0, 5)
            );

            var testResults = new long[] {
                7L, 18L, 12L, 14L, 17L, 19L
            };

            var response = new JsonRpcResponse<long[]>();
            response.Id = 1;
            response.Result = testResults;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<long[]>()
            {
                Model = "res.partner",
                Method = "search",
                Validator = (p) =>
                {
                    Assert.Equal(7, p.args.Length);

                    dynamic domainArgs = p.args[5];
                    Assert.Equal(2, domainArgs[0].Length);
                    Assert.Equal(
                        new object[]
                        {
                            new object[] { "is_company", "=", true },
                            new object[] { "customer", "=", true }
                        },
                        domainArgs[0]
                    );
                    dynamic pagArgs = p.args[6];
                    Assert.Equal(0, pagArgs.offset);
                    Assert.Equal(5, pagArgs.limit);
                },
                ExecuteRpcCall = () => RpcClient.Search<long[]>(requestParameters),
                TestResponse = response
            });
        }

        [Fact]
        public async Task SearchCount_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchCountParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true)
            );

            var testResults = 2;

            var response = new JsonRpcResponse<long>();
            response.Id = 1;
            response.Result = testResults;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<long>()
            {
                Model = "res.partner",
                Method = "search_count",
                Validator = (p) =>
                {
                    Assert.Equal(6, p.args.Length);

                    dynamic domainArgs = p.args[5];
                    Assert.Equal(2, domainArgs[0].Length);
                    Assert.Equal(
                        new object[]
                        {
                            new object[] { "is_company", "=", true },
                            new object[] { "customer", "=", true }
                        },
                        domainArgs[0]
                    );
                },
                ExecuteRpcCall = () => RpcClient.SearchCount(requestParameters),
                TestResponse = response
            });
        }

    }
}
