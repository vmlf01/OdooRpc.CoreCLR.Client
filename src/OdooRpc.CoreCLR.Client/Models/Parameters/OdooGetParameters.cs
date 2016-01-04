using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooGetParameters
    {
        public string Model { get; private set; }
        public List<long> Ids { get; private set; }
        public List<string> Fields { get; private set; }

        public OdooGetParameters(string model)
            : this(model, new List<long>(), new List<string>())
        {
        }

        public OdooGetParameters(string model, IEnumerable<long> ids)
            : this(model, ids, new List<string>())
        {
        }

        public OdooGetParameters(string model, IEnumerable<long> ids, IEnumerable<string> fields)
        {
            this.Model = model;
            this.Ids = new List<long>(ids);
            this.Fields = new List<string>(fields);
        }
    }
}