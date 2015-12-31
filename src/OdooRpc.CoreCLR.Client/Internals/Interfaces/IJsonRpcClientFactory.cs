using System;
using JsonRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Internals.Interfaces
{
    internal interface IJsonRpcClientFactory
    {
        IJsonRpcClient GetRpcClient(Uri rpcEndpoint);
    }
}
