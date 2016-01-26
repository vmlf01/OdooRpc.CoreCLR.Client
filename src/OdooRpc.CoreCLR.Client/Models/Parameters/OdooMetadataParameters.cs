using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooMetadataParameters : OdooGetParameters
    {
        public OdooMetadataParameters(string model)
            : base(model, new List<long>())
        {
        }

        public OdooMetadataParameters(string model, IEnumerable<long> ids)
            : base(model, ids)
        {
        }
    }
}
