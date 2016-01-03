using System;
using System.Threading.Tasks;
using JsonRpc.CoreCLR.Client.Interfaces;
using JsonRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class JsonRpcWebClientStub : IJsonRpcClient
    {
        object NextResponse;
        public bool WasCalled { get; private set; }

        public void SetNextResponse(object nextResponse)
        {
            this.NextResponse = nextResponse;
            this.WasCalled = false;
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(JsonRpcRequest jsonRpc)
        {
            this.WasCalled = true;
            return Task.FromResult(this.NextResponse as JsonRpcResponse<T>);
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(string method, object[] args)
        {
            this.WasCalled = true;
            return Task.FromResult(this.NextResponse as JsonRpcResponse<T>);
        }

        public Task<JsonRpcResponse<T>> InvokeAsync<T>(string method, object arg)
        {
            this.WasCalled = true;
            return Task.FromResult(this.NextResponse as JsonRpcResponse<T>);
        }
    }
}
