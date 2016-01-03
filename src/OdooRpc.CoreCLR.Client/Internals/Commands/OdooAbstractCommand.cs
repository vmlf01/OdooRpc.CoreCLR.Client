using System;
using System.Threading.Tasks;
using JsonRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal abstract class OdooAbstractCommand
    {
        protected IJsonRpcClient RpcClient { get; private set; }
        protected bool IsLoginRequired { get; private set; }

        public OdooAbstractCommand(IJsonRpcClient rpcClient, bool isLoginRequired = true)
        {
            this.RpcClient = rpcClient;
            this.IsLoginRequired = isLoginRequired;
        }

        protected async Task<T> InvokeRpc<T>(OdooSessionInfo sessionInfo, OdooRpcRequest request)
        {
            if (this.IsLoginRequired && !sessionInfo.IsLoggedIn)
            {
                throw new InvalidOperationException("User is not logged in to Odoo");
            }
            else
            {
                var response = await this.RpcClient.InvokeAsync<T>("call", request);

                if (response.Error == null)
                {
                    return response.Result;
                }
                else
                {
                    throw new RpcCallException(response.Error);
                }
            }
        }
    }
}
