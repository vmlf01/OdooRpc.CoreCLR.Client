namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooSessionInfo
    {
        public bool IsLoggedIn { get; internal set; }
        public long UserId { get; internal set; }
        public string SessionId { get; internal set; }
        public OdooUserContext UserContext { get; internal set; }
    }
}