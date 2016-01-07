using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooPaginationParameters
    {
        public long? Offset { get; set; }
        public long? Limit { get; set; }
        private StringBuilder Order { get; set; }

        public OdooPaginationParameters()
        {
            this.Offset = null;
            this.Limit = null;
            this.Order = new StringBuilder();
        }

        public OdooPaginationParameters(long offset, long limit)
        {
            this.Offset = offset;
            this.Limit = limit;
            this.Order = new StringBuilder();
        }


        public bool IsDefined()
        {
            return this.Limit.HasValue || this.Offset.HasValue || this.Order.Length > 0;
        }

        public OdooPaginationParameters OrderBy(string orderField)
        {
            this.Order.Clear();
            this.Order.AppendFormat("{0} ASC", orderField);
            return this;
        }


        public OdooPaginationParameters OrderByDescending(string orderField)
        {
            this.Order.Clear();
            this.Order.AppendFormat("{0} DESC", orderField);
            return this;
        }


        public OdooPaginationParameters ThenBy(string orderField)
        {
            this.Order.AppendFormat(", {0} ASC", orderField);
            return this;
        }

        public OdooPaginationParameters ThenByDescending(string orderField)
        {
            this.Order.AppendFormat(", {0} DESC", orderField);
            return this;
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

                if (this.Order.Length > 0)
                {
                    parameters.order = this.Order.ToString();
                }
            }
        }
    }
}
