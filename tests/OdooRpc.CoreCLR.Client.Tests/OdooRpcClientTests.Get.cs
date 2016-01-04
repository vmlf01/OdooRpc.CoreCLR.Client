using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using OdooRpc.CoreCLR.Client.Internals;
using JsonRpc.CoreCLR.Client.Models;
using Xunit;
using Newtonsoft.Json.Linq;
using System;
using OdooRpc.CoreCLR.Client.Tests.Helpers;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public partial class OdooRpcClientTests
    {
        [Fact]
        public async Task Get_WithIds_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooGetParameters("res.partner", new long[] { 7 });

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
                    Assert.Equal(6, p.args.Length);
                    dynamic args = p.args[5];

                    Assert.Equal(1, args.Length);
                    Assert.Equal(requestParameters.Ids, args[0]);
                },
                Model = requestParameters.Model,
                Method = "read",
                ExecuteRpcCall = () => RpcClient.Get<TestPartner[]>(requestParameters),
                TestResponse = response
            });
        }

        [Fact]
        public async Task Get_WithIds_And_WithFields_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooGetParameters("res.partner", new long[] { 7 });

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
                    Assert.Equal(7, p.args.Length);
                    dynamic args = p.args[5];

                    Assert.Equal(1, args.Length);
                    Assert.Equal(requestParameters.Ids, args[0]);

                    dynamic fieldsArgs = p.args[6];
                    Assert.Equal(new string[] { "name", "country_id", "comment" }, fieldsArgs.fields);
                },
                Model = requestParameters.Model,
                Method = "read",
                ExecuteRpcCall = () =>
                {
                    return RpcClient.Get<TestPartner[]>(
                        requestParameters,
                        new OdooFieldParameters(new string[] { "name", "country_id", "comment" })
                    );
                },
                TestResponse = response
            });
        }

        [Fact]
        public async Task Get_WithSearch_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true)
            );

            var testPartners = new TestPartner[]
            {
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 7,
                    name = "Agrolait"
                },
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 8,
                    name = "Parmalait"
                }
            };

            var response = new JsonRpcResponse<TestPartner[]>();
            response.Id = 1;
            response.Result = testPartners;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<TestPartner[]>()
            {
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
                Model = requestParameters.Model,
                Method = "search_read",
                ExecuteRpcCall = () => RpcClient.Get<TestPartner[]>(requestParameters),
                TestResponse = response
            });
        }

        [Fact]
        public async Task Get_WithSearch_And_Pagination_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true)
            );

            var testPartners = new TestPartner[]
            {
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 7,
                    name = "Agrolait"
                },
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 8,
                    name = "Parmalait"
                }
            };

            var response = new JsonRpcResponse<TestPartner[]>();
            response.Id = 1;
            response.Result = testPartners;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<TestPartner[]>()
            {
                Validator = (p) =>
                {
                    Assert.Equal(7, p.args.Length);

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
                    dynamic pagArgs = p.args[6];
                    Assert.Equal(0, pagArgs.offset);
                    Assert.Equal(5, pagArgs.limit);
                },
                Model = requestParameters.Model,
                Method = "search_read",
                ExecuteRpcCall = () => RpcClient.Get<TestPartner[]>(requestParameters, new OdooPaginationParameters(0, 5)),
                TestResponse = response
            });
        }

        [Fact]
        public async Task Get_WithSearch_And_Fields_And_Pagination_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooSearchParameters(
                "res.partner",
                new OdooDomainFilter()
                    .Filter("is_company", "=", true)
                    .Filter("customer", "=", true)
            );

            var testPartners = new TestPartner[]
            {
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 7,
                    name = "Agrolait"
                },
                new TestPartner()
                {
                    comment = false,
                    country_id = new object[] { 21, "Belgium" },
                    id = 8,
                    name = "Parmalait"
                }
            };

            var response = new JsonRpcResponse<TestPartner[]>();
            response.Id = 1;
            response.Result = testPartners;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<TestPartner[]>()
            {
                Validator = (p) =>
                {
                    Assert.Equal(7, p.args.Length);

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
                    dynamic searchArgs = p.args[6];
                    Assert.Equal(0, searchArgs.offset);
                    Assert.Equal(5, searchArgs.limit);
                    Assert.Equal(new string[] { "name", "country_id", "comment" }, searchArgs.fields);
                },
                Model = requestParameters.Model,
                Method = "search_read",
                ExecuteRpcCall = () => RpcClient.Get<TestPartner[]>(requestParameters, new OdooFieldParameters(new string[] { "name", "country_id", "comment" }), new OdooPaginationParameters(0, 5)),
                TestResponse = response
            });
        }

    }
}
