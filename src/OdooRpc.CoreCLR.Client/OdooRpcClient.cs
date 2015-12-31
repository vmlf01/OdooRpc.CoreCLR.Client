using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Internals;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;

[assembly: InternalsVisibleTo("OdooRpc.CoreCLR.Client.Tests")]
namespace OdooRpc.CoreCLR.Client
{
    internal class OdooRpcClient : IOdooRpcClient
    {
        public OdooConnectionInfo ConnectionInfo { get { return this.OdooConnection.ConnectionInfo; } }
        public OdooSessionInfo SessionInfo { get { return this.OdooConnection.SessionInfo; } }

        private IOdooConnection OdooConnection { get; set; }

        public OdooRpcClient() 
            : this(new OdooConnection())
        {
        }

        public OdooRpcClient(IOdooConnection connection)
        {
            this.OdooConnection = connection;
        }

        public async Task Connect(OdooConnectionInfo connectionInfo)
        {
            await this.OdooConnection.Connect(connectionInfo);
        }

    }
}