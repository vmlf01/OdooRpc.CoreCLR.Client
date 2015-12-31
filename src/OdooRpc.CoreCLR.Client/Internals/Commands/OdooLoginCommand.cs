using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooLoginCommand : OdooAbstractCommand
    {
        public bool IsLoggedIn { get; private set; }
        public long? UserId { get; private set; }

        public OdooLoginCommand(IJsonRpcClient rpcClient)
            : base(rpcClient, isLoginRequired: false)
        {
        }

        public async Task Execute(OdooSessionInfo sessionInfo)
        {
            this.IsLoggedIn = false;
            this.UserId = null;

            var request = CreateLoginRequest(sessionInfo);
            var result = await InvokeRpc<object>(sessionInfo, request);

            long uid;
            if (long.TryParse(result.ToString(), out uid))
            {
                this.IsLoggedIn = true;
                this.UserId = uid;
            }
        }

        private object CreateLoginRequest(OdooSessionInfo sessionInfo)
        {
            return new
            {
                service = "common",
                method = "authenticate",
                args = new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.Username,
                    sessionInfo.Password,
                    sessionInfo.UserContext
                }
            };
        }
    }
}