using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Internals.Models
{
    internal class OdooAuthenticationResponse
    {
        public long uid { get; set; }
        public string username { get; set; }
        public string session_id { get; set; }
        public string db { get; set; }
        public long? company_id { get; set; }
        public long? partner_id { get; set; }
        public OdooUserContext user_context { get; set; }
    }
}