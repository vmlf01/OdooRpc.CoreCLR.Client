using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using JsonRpc.CoreCLR.Client.Interfaces;

namespace OdooRpc.CoreCLR.Client.Internals.Commands
{
    internal class OdooCreateCommand : OdooAbstractCommand
    {
        public OdooCreateCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<long> Execute<T>(OdooSessionInfo sessionInfo, string model, T newRecord)
        {
            return InvokeRpc<long>(sessionInfo, CreateCreateRequest(sessionInfo, model, newRecord));
        }

        private OdooRpcRequest CreateCreateRequest(OdooSessionInfo sessionInfo, string model, object newRecord)
        {
            return new OdooRpcRequest()
            {
                service = "object",
                method = "execute_kw",
                args = new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    model,
                    "create",
                    new object[]
                    {
                        newRecord
                    }
                },
                context = sessionInfo.UserContext
            };
        }
    }

    // Needed for e.g.:
    // var changeQuantity = OdooRpcClient.OdooCreateWithMethodCommand<dynamic>("stock.change.product.qty", "change_product_qty", 1 );
    internal class OdooCreateDynamicCommand : OdooAbstractCommand
    {
        public OdooCreateDynamicCommand(IJsonRpcClient rpcClient)
            : base(rpcClient)
        {
        }

        public Task<dynamic> Execute<T>(OdooSessionInfo sessionInfo, string model, string method, T id)
        {
            return InvokeRpc<dynamic>(sessionInfo, CreateCreateDynamicRequest(sessionInfo, model, method, id));
        }

        private OdooRpcRequest CreateCreateDynamicRequest(OdooSessionInfo sessionInfo, string model, string method, object id)
        {
            return new OdooRpcRequest()
            {
                service = "object",
                method = "execute_kw",
                args = new object[]
                {
                    sessionInfo.Database,
                    sessionInfo.UserId,
                    sessionInfo.Password,
                    model,
                    method,
                    new object[]
                    {
                        id
                    }
                },
                context = sessionInfo.UserContext
            };
        }

    }

}