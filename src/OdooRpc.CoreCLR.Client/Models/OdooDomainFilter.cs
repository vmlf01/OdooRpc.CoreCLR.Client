using System;
using System.Linq;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models
{
    public class OdooDomainFilter
    {
        private List<object> FilterCriteria = new List<object>();
        private int RequiredFilterCount = 0;

        public OdooDomainFilter()
        {
        }

        public OdooDomainFilter Filter(string field, string oper, object val)
        {
            AddCriteria(new object[] { field, oper, val });
            return this;
        }

        public OdooDomainFilter And()
        {
            AddCriteria("&");
            this.RequiredFilterCount = this.RequiredFilterCount + 2;
            return this;
        }

        public OdooDomainFilter Or()
        {
            AddCriteria("|");
            this.RequiredFilterCount = this.RequiredFilterCount + 2;
            return this;
        }

        private void AddCriteria(object criteria)
        {
            this.RequiredFilterCount = Math.Max(0, this.RequiredFilterCount - 1);
            this.FilterCriteria.Add(criteria);
        }

        internal object[] ToFilterArray()
        {
            if (this.RequiredFilterCount != 0)
            {
                throw new InvalidOperationException(string.Format("Domain filter is missing {0} conditions", this.RequiredFilterCount));
            }
            else
            {
                return this.FilterCriteria.ToArray();
            }
        }
    }
}