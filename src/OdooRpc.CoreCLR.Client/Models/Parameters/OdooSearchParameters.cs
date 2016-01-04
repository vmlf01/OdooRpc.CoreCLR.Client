using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooSearchParameters : OdooSearchCountParameters
    {
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
            : base(model, domainFilter)
        {
            this.Pagination = paginationParams;
        }
    }
}