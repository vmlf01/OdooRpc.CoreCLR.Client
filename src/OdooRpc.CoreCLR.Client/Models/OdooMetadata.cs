using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooMetadata
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }

        [JsonProperty("xmlid")]
        public string ExternalId { get; internal set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; internal set; }

        [JsonProperty("write_date")]
        public DateTime WriteDate { get; internal set; }

        [JsonProperty("noupdate")]
        public bool NoUpdate { get; internal set; }

        [JsonProperty("create_uid")]
        public dynamic CreateUid { get; internal set; }

        [JsonProperty("write_uid")]
        public dynamic WriteUid { get; internal set; }
        
        
    }
}