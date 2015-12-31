using System;
using System.Net;
using System.Threading.Tasks;
using JsonRpc.CoreCLR.Client;
using JsonRpc.CoreCLR.Client.Interfaces;
using JsonRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Internals
{
    internal class JsonRpcClientFactory : IJsonRpcClientFactory
    {
        public IJsonRpcClient GetRpcClient(Uri rpcEndpoint)
        {
            return new JsonRpcWebClient(rpcEndpoint);
        }
    }
}
