using System.Linq;
using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooFieldParameters : List<string>
    {
        public OdooFieldParameters()
            : base() 
        {
        }

        public OdooFieldParameters(IEnumerable<string> collection)
            : base(collection)
        {

        }
    }
}