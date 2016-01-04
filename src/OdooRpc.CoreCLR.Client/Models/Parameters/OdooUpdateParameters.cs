using System.Collections.Generic;

namespace OdooRpc.CoreCLR.Client.Models.Parameters
{
    public class OdooUpdateParameters<T>
    {
        public string Model { get; private set; }
        public List<long> Ids { get; private set; }
        public T UpdateValues { get; set; }

        public OdooUpdateParameters(string model)
            : this(model, new List<long>())
        {
        }

        public OdooUpdateParameters(string model, IEnumerable<long> ids)
            : this(model, ids, default(T))
        {
        }

        public OdooUpdateParameters(string model, IEnumerable<long> ids, T updateValues)
        {
            this.Model = model;
            this.Ids = new List<long>(ids);
            this.UpdateValues = updateValues;
        }
    }
}