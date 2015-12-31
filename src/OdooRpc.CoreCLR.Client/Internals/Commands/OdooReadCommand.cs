using System;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;

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

        private object CreateReadRequest(OdooSessionInfo sessionInfo, OdooGetParameters parameters)
        {
            return new
            {
                service = "object",
                method = "execute_kw",
                args = new object[] 
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    parameters.Model,
                    "read",
                    new object[]
                    {
                        parameters.Ids,
                        parameters.Fields
                    }
                },
                context = sessionInfo.UserContext
            };
        }
    }
}
