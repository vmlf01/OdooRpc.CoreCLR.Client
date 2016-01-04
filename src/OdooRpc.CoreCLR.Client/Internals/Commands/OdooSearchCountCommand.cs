using System;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using JsonRpc.CoreCLR.Client.Interfaces;
using System.Dynamic;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooSearchCountCommand : OdooAbstractCommand
    {
        public OdooSearchCountCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<long> Execute(OdooSessionInfo sessionInfo, OdooSearchCountParameters parameters)
        {
            return InvokeRpc<long>(sessionInfo, CreateSearchRequest(sessionInfo, parameters));
        }

        private OdooRpcRequest CreateSearchRequest(OdooSessionInfo sessionInfo, OdooSearchCountParameters parameters)
        {
            List<object> requestArgs = new List<object>(
                new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    parameters.Model,
                    "search_count",
                    new object[]
                    {
                        parameters.DomainFilter.ToFilterArray()
                    }
                }
            );

            return new OdooRpcRequest()
            {
                service = "object",
                method = "execute_kw",
                args = requestArgs.ToArray(),
                context = sessionInfo.UserContext
            };
        }
    }
}
