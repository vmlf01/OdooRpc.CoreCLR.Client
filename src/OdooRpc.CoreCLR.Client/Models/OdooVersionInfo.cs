using Newtonsoft.Json;

namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooVersionInfo
    {
        [JsonProperty("server_version")]
        public string ServerVersion { get; internal set; }

        [JsonProperty("server_version_info")]
        public dynamic ServiceVersionInfo { get; internal set; }

        [JsonProperty("server_serie")]
        public string ServerSerie { get; internal set; }

        [JsonProperty("protocol_version")]
        public long ProtocolVersion { get; internal set; }
    }
}