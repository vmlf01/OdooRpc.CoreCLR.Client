using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooSearchParameters
    {
        public string Model { get; private set; }
        public OdooDomainFilter DomainFilter { get; private set; }
        public OdooPaginationParameters Pagination { get; private set; }

        public OdooSearchParameters(string model)
            : this(model, new OdooDomainFilter())
        {
        }

        public OdooSearchParameters(string model, OdooDomainFilter domainFilter)
            : this(model, domainFilter, new OdooPaginationParameters())
        {
        }

        public OdooSearchParameters(string model, OdooDomainFilter domainFilter, OdooPaginationParameters paginationParams)
        {
            this.Model = model;
            this.DomainFilter = domainFilter;
            this.Pagination = paginationParams;
        }
    }
}