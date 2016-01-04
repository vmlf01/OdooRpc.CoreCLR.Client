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
using System.Dynamic;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public partial class OdooRpcClientTests
    {
        [Fact]
        public async Task GetModelFields_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooGetModelFieldsParameters(
                "res.partner",
                new OdooFieldParameters(new string[] { "string", "help", "type" })
            );

            Dictionary<string, dynamic> testResults = new Dictionary<string, dynamic>();
            testResults["ean13"] = new {
                type = "char",
                help = "BarCode",
                @string = "EAN13"
            };
            testResults["property_account_position"] = new {
                type = "many2one",
                help = "The fiscal position will determine taxes and accounts used for the partner.",
                @string = "Fiscal Position"
            };

            var response = new JsonRpcResponse<Dictionary<string, dynamic>>();
            response.Id = 1;
            response.Result = testResults;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<Dictionary<string, dynamic>>()
            {
                Model = "res.partner",
                Method = "fields_get",
                Validator = (p) =>
                {
                    Assert.Equal(7, p.args.Length);

                    dynamic args = p.args[5];
                    Assert.Equal(0, args.Length);
                },
                ExecuteRpcCall = () => RpcClient.GetModelFields<dynamic>(requestParameters),
                TestResponse = response
            });
        }

    }
}
