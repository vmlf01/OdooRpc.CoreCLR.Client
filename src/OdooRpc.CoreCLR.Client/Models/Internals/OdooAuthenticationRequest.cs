namespace OdooRpc.CoreCLR.Client.Models.Internals
{
    internal class OdooAuthenticationRequest
    {
        public string db { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        
        public static OdooAuthenticationRequest Create(OdooConnectionInfo connectionInfo)
        {
            return new OdooAuthenticationRequest() {
                db = connectionInfo.Database,
                login = connectionInfo.Username,
                password = connectionInfo.Password
            };
        }
    }
}