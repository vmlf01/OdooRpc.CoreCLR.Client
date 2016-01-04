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

        public Task<long> ExecuteCount(OdooSessionInfo sessionInfo, OdooSearchParameters searchParams)
        {
            return InvokeRpc<long>(sessionInfo, CreateSearchRequest(sessionInfo, "search_count", searchParams, null, null));
        }

        public Task<T> Execute<T>(OdooSessionInfo sessionInfo, OdooSearchParameters searchParams, OdooPaginationParameters pagParams)
        {
            return InvokeRpc<T>(sessionInfo, CreateSearchRequest(sessionInfo, "search", searchParams, null, pagParams));
        }

        public Task<T> Execute<T>(OdooSessionInfo sessionInfo, OdooSearchParameters searchParams, OdooFieldParameters fieldParams, OdooPaginationParameters pagParams)
        {
            return InvokeRpc<T>(sessionInfo, CreateSearchRequest(sessionInfo, "search_read", searchParams, fieldParams, pagParams));
        }

        private OdooRpcRequest CreateSearchRequest(OdooSessionInfo sessionInfo, string method, OdooSearchParameters searchParams, OdooFieldParameters fieldParams, OdooPaginationParameters pagParams)
        {
            List<object> requestArgs = new List<object>(
                new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    searchParams.Model,
                    method,
                    new object[]
                    {
                        searchParams.DomainFilter.ToFilterArray()
                    }
                }
            );

            dynamic searchOptions = new ExpandoObject();
            bool useSearchOptions = false;
            if (fieldParams != null && fieldParams.Count > 0)
            {
                searchOptions.fields = fieldParams.ToArray();
                useSearchOptions = true;
            }

            if (pagParams != null && pagParams.IsDefined())
            {
                pagParams.AddToParameters(searchOptions);
                useSearchOptions = true;
            }

            if (useSearchOptions)
            {
                requestArgs.Add(searchOptions);
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
