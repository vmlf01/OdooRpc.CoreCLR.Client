using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Interfaces
{
    public interface IOdooRpcClient
    {
        IOdooConnection OdooConnection { get; }
        
    }
}