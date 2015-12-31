using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Interfaces
{
    public interface IOdooRpcClient
    {
        OdooConnectionInfo ConnectionInfo { get; }
        OdooSessionInfo SessionInfo { get; }

        Task Connect(OdooConnectionInfo connectionInfo);
    }
}