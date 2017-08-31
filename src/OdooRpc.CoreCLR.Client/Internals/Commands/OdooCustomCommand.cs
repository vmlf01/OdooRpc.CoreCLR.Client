using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooExecWorkFlowCommand : OdooAbstractCommand
    {
        public OdooExecWorkFlowCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<long> Execute<T>(OdooSessionInfo sessionInfo, string model, string method, T id)
        {
            return InvokeRpc<long>(sessionInfo, CreateExecWorkFlowRequest(sessionInfo, model, method, id));
        }

        private OdooRpcRequest CreateExecWorkFlowRequest(OdooSessionInfo sessionInfo, string model, string method, object id)
        {
            return new OdooRpcRequest()
            {
                service = "object",
                method = "exec_workflow",
                args = new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    model,
                    method,
                    id
                },
                context = sessionInfo.UserContext
            };
        }
    }
}