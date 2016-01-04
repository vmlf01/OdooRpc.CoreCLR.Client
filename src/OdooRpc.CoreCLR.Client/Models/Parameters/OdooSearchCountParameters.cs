using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooSearchCountParameters
    {
        public string Model { get; private set; }
        public OdooDomainFilter DomainFilter { get; private set; }

        public OdooSearchCountParameters(string model)
            : this(model, new OdooDomainFilter())
        {
        }

        public OdooSearchCountParameters(string model, OdooDomainFilter domainFilter)
        {
            this.Model = model;
            this.DomainFilter = domainFilter;
        }
    }
}