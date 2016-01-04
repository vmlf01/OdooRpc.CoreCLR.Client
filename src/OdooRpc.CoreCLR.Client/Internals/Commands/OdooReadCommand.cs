using System;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using JsonRpc.CoreCLR.Client.Interfaces;
using System.Collections.Generic;
using System.Dynamic;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooReadCommand : OdooAbstractCommand
    {
        public OdooReadCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<T> Execute<T>(OdooSessionInfo sessionInfo, OdooGetParameters parameters)
        {
            return InvokeRpc<T>(sessionInfo, CreateReadRequest(sessionInfo, parameters));
        }

        private OdooRpcRequest CreateReadRequest(OdooSessionInfo sessionInfo, OdooGetParameters parameters)
        {
            List<object> requestArgs = new List<object>(
                new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    parameters.Model,
                    "read",
                    new object[]
                    {
                        parameters.Ids
                    }
                }
            );

            if (parameters.Fields.Count > 0)
            {
                dynamic getOptions = new ExpandoObject();
                getOptions.fields = parameters.Fields;
                requestArgs.Add(getOptions);
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
