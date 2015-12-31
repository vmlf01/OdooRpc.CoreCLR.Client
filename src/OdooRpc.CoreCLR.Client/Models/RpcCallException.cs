using System;
using JsonRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Models
{
    public class RpcCallException : Exception
    {
        public object RpcErrorCode { get; private set; }
        public object RpcErrorData { get; private set; }

        public RpcCallException(JsonRpcException rpcError)
            : base(rpcError.Message)
        {
            this.RpcErrorCode = rpcError.Code;
            this.RpcErrorData = rpcError.Data;
        }
    }
}
