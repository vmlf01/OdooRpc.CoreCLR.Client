using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooGetParameters
    {
        public string Model { get; private set; }
        public List<long> Ids { get; private set; }

        public OdooGetParameters(string model)
            : this(model, new List<long>())
        {
        }

        public OdooGetParameters(string model, IEnumerable<long> ids)
        {
            this.Model = model;
            this.Ids = new List<long>(ids);
        }
    }
}