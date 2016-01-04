using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooPaginationParameters
    {
        public long? Offset { get; set; }
        public long? Limit { get; set; }

        public OdooPaginationParameters()
        {
            this.Offset = null;
            this.Limit = null;
        }

        public OdooPaginationParameters(long offset, long limit)
        {
            this.Offset = offset;
            this.Limit = limit;
        }

        public bool IsDefined()
        {
            return this.Limit.HasValue || this.Offset.HasValue;
        }

        public void AddToParameters(dynamic parameters)
        {
            if (this.IsDefined())
            {
                if (this.Offset.HasValue)
                {
                    parameters.offset = this.Offset.Value;
                }

                if (this.Limit.HasValue)
                {
                    parameters.limit = this.Limit.Value;
                }
            }
        }
    }
}
