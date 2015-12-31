using Newtonsoft.Json;

namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooUserContext
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }
    }
}