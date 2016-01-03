using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooVersionCommand : OdooAbstractCommand
    {
        public OdooVersionCommand(IJsonRpcClient rpcClient)
            : base(rpcClient, isLoginRequired: false)
        {
        }

        public Task<OdooVersionInfo> Execute(OdooSessionInfo sessionInfo)
        {
            return InvokeRpc<OdooVersionInfo>(sessionInfo, CreateVersionRequest());
        }

        private OdooRpcRequest CreateVersionRequest()
        {
            return new OdooRpcRequest()
            {
                service = "common",
                method = "version",
                args = new object[0],
                context = new OdooUserContext()
            };
        }
    }
}
