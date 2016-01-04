using System;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using JsonRpc.CoreCLR.Client.Interfaces;
using System.Dynamic;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooSearchCommand : OdooAbstractCommand
    {
        public OdooSearchCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<T> Execute<T>(OdooSessionInfo sessionInfo, OdooSearchParameters parameters)
        {
            return InvokeRpc<T>(sessionInfo, CreateSearchRequest(sessionInfo, parameters));
        }

        private OdooRpcRequest CreateSearchRequest(OdooSessionInfo sessionInfo, OdooSearchParameters parameters)
        {
            List<object> requestArgs = new List<object>(
                new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    parameters.Model,
                    "search",
                    new object[]
                    {
                        parameters.DomainFilter.ToFilterArray()
                    }
                }
            );

            if (parameters.Pagination.IsDefined())
            {
                dynamic searchParams = new ExpandoObject();
                parameters.Pagination.AddToParameters(searchParams);
                requestArgs.Add(searchParams);
            }

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
