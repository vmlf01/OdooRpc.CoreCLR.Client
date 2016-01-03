using System;
using System.Threading.Tasks;
using JsonRpc.CoreCLR.Client.Interfaces;
using JsonRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class JsonRpcWebClientStub : IJsonRpcClient
    {
        Action<JsonRpcRequest> RequestValidator;
        object NextResponse;
        public bool WasCalled { get; private set; }

        public void SetNextRequestValidator(Action<JsonRpcRequest> validator)
        {
            this.RequestValidator = validator;
        }

        public void SetNextResponse(object nextResponse)
        {
            this.NextResponse = nextResponse;
            this.WasCalled = false;
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(JsonRpcRequest jsonRpc)
        {
            this.WasCalled = true;
            if (this.RequestValidator != null)
            {
                this.RequestValidator(jsonRpc);
            }
            return Task.FromResult(this.NextResponse as JsonRpcResponse<T>);
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(string method, object[] args)
        {
            return this.InvokeAsync<T>(new JsonRpcRequest()
            {
                Method = method,
                Params = args
            });
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(string method, object arg)
        {
            return this.InvokeAsync<T>(new JsonRpcRequest()
            {
                Method = method,
                Params = arg
            });
        }
    }
}
