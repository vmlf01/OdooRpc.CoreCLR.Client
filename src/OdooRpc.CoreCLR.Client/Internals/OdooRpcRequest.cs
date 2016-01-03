using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Internals
{
    internal class OdooRpcRequest
    {
        public string service { get; set; }
        public string method { get; set; }
        public object[] args { get; set; }
        public OdooUserContext context { get; set; }

        public OdooRpcRequest()
        {
            this.context = new OdooUserContext();
        }
    }
}
