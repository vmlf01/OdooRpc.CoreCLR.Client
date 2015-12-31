namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooSessionInfo : OdooConnectionInfo
    {
        public bool IsLoggedIn { get; internal set; }
        public long? UserId { get; internal set; }
        public OdooUserContext UserContext { get; internal set; }

        internal OdooSessionInfo(OdooConnectionInfo connectionInfo)
        {
            this.IsSSL = connectionInfo.IsSSL;
            this.Host = connectionInfo.Host;
            this.Port = connectionInfo.Port;
            this.Database = connectionInfo.Database;
            this.Username = connectionInfo.Username;
            this.Password = connectionInfo.Password;

            this.Reset();
        }

        internal void Reset()
        {
            this.IsLoggedIn = false;
            this.UserId = null;
            this.UserContext = new OdooUserContext();
        }
    }
}