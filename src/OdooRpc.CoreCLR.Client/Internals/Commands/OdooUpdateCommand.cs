using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models.Parameters;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooUpdateCommand : OdooAbstractCommand
    {
        public OdooUpdateCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task Execute<T>(OdooSessionInfo sessionInfo, OdooUpdateParameters<T> parameters)
        {
            return InvokeRpc<object>(sessionInfo, CreateUpdateRequest(sessionInfo, parameters));
        }

        private OdooRpcRequest CreateUpdateRequest<T>(OdooSessionInfo sessionInfo, OdooUpdateParameters<T> parameters)
        {
            return new OdooRpcRequest()
            {
                service = "object",
                method = "execute_kw",
                args = new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    parameters.Model,
                    "write",
                    new object[]
                    {
                        parameters.Ids.ToArray(),
                        parameters.UpdateValues
                    }
                },
                context = sessionInfo.UserContext
            };
        }
    }
}