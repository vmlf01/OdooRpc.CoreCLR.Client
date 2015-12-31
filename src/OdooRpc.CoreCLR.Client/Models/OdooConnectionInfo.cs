namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooConnectionInfo
    {
        public bool IsSSL { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public OdooConnectionInfo()
        {
            this.IsSSL = false;
            this.Port = 80;
        }
    }
}