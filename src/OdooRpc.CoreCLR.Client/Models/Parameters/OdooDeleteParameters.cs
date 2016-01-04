using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooDeleteParameters
    {
        public string Model { get; private set; }
        public List<long> Ids { get; private set; }

        public OdooDeleteParameters(string model)
            : this(model, new List<long>())
        {
        }

        public OdooDeleteParameters(string model, IEnumerable<long> ids)
        {
            this.Model = model;
            this.Ids = new List<long>(ids);
        }
    }
}