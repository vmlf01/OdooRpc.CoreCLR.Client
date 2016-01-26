using System;
using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using JsonRpc.CoreCLR.Client.Interfaces;
using System.Collections.Generic;
using System.Dynamic;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooMetadataCommand : OdooAbstractCommand
    {
        public OdooMetadataCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<T> Execute<T>(OdooSessionInfo sessionInfo, OdooMetadataParameters matadataParams)
        {
            return InvokeRpc<T>(sessionInfo, CreateMetadataRequest(sessionInfo, matadataParams));
        }

        private OdooRpcRequest CreateMetadataRequest(OdooSessionInfo sessionInfo, OdooMetadataParameters matadataParams)
        {
            List<object> requestArgs = new List<object>(
                new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    matadataParams.Model,
                    "get_metadata",
                    new object[]
                    {
                        matadataParams.Ids
                    }
                }
            );

            return new OdooRpcRequest()
            {
                service = "object",
                method = "execute_kw",
                args = requestArgs.ToArray(),
                context = new OdooUserContext()
            };
        }
    }
}
