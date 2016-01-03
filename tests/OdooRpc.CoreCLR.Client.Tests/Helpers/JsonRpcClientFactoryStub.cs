using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using JsonRpc.CoreCLR.Client.Interfaces;
using System;

namespace OdooRpc.CoreCLR.Client.Tests.Helpers
{
    public class JsonRpcClientFactoryStub : IJsonRpcClientFactory
    {
        IJsonRpcClient RpcClient;

        public JsonRpcClientFactoryStub(IJsonRpcClient rpcClient)
        {
            this.RpcClient = rpcClient;
        }

        public IJsonRpcClient GetRpcClient(Uri rpcEndpoint)
        {
            return this.RpcClient;
        }
    }
}
