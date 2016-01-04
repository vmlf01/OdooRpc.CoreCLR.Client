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
        public async Task Create_ShouldCallRpcWithCorrectParameters()
        {
            var testPartner = new {
                name = "New Partner"
            };

            var response = new JsonRpcResponse<long>();
            response.Id = 1;
            response.Result = 78;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<long>()
            {
                Model = "res.partner",
                Method = "create",
                Validator = (p) =>
                {
                    Assert.Equal(6, p.args.Length);

                    dynamic args = p.args[5];
                    Assert.Equal(1, args.Length);
                    Assert.Equal(
                        new object[]
                        {
                            testPartner
                        },
                        args
                    );
                },
                ExecuteRpcCall = () => RpcClient.Create<dynamic>("res.partner", testPartner),
                TestResponse = response
            });
        }
    }
}
