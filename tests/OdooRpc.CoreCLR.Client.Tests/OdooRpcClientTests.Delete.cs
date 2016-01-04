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
        public async Task Delete_ShouldCallRpcWithCorrectParameters()
        {
            var requestParameters = new OdooDeleteParameters("res.partner", new long [] {
                6, 7
            });

            var response = new JsonRpcResponse<object>();
            response.Id = 1;
            response.Result = null;

            await TestOdooRpcCall(new OdooRpcCallTestParameters<object>()
            {
                Model = "res.partner",
                Method = "unlink",
                Validator = (p) =>
                {
                    Assert.Equal(6, p.args.Length);

                    dynamic args = p.args[5];
                    Assert.Equal(1, args.Length);
                    Assert.Equal(
                        new long[] { 6, 7 },
                        args[0]
                    );
                },
                ExecuteRpcCall = async () => {
                    await RpcClient.Delete(requestParameters);
                    return null;
                },
                TestResponse = response
            });
        }
    }
}
