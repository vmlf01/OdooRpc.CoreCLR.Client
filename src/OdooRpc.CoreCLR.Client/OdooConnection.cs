using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Internals;
using JsonRpc.CoreCLR.Client;
using JsonRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client
{
    public class OdooConnection : IOdooConnection
    {
        public OdooConnectionInfo ConnectionInfo { get; private set; }
        public OdooSessionInfo SessionInfo { get; private set; }

        public OdooConnection()
        { 
            this.ConnectionInfo = new OdooConnectionInfo();
            this.SessionInfo = new OdooSessionInfo();
        }
        
        public async Task Connect(OdooConnectionInfo connectionInfo)
        {
            ResetConnection(connectionInfo);

            await LoginToOdoo();
        }
        
        private void ResetConnection(OdooConnectionInfo connectionInfo)
        {
            this.ConnectionInfo = connectionInfo;
            this.SessionInfo = new OdooSessionInfo();
        }
        
        private async Task LoginToOdoo()
        {
            var rpcClient = new JsonRpcWebClient(OdooEndpoints.GetAuthenticationUri(this.ConnectionInfo));
            var request = OdooAuthenticationRequest.Create(this.ConnectionInfo);
            var response = await rpcClient.InvokeAsync<OdooAuthenticationResponse>("call", request);
            
            this.SessionInfo = ParseSessionInfo(response);                
        }
        
        private OdooSessionInfo ParseSessionInfo(JsonRpcResponse<OdooAuthenticationResponse> response)
        {
            if (response.Error != null || response.Result.uid == 0)
            {
                return new OdooSessionInfo() {
                    IsLoggedIn = false
                };
            }
            else
            {
                return new OdooSessionInfo() {
                    IsLoggedIn = true,
                    UserId = response.Result.uid,
                    SessionId = response.Result.session_id,
                    UserContext = response.Result.user_context
                };            
            }
        }
    }
}