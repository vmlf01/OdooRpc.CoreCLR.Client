using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using OdooRpc.CoreCLR.Client.Internals;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Internals.Commands;
using JsonRpc.CoreCLR.Client.Interfaces;

[assembly: InternalsVisibleTo("OdooRpc.CoreCLR.Client.Tests")]
namespace OdooRpc.CoreCLR.Client
{
    public class OdooRpcClient : IOdooRpcClient
    {
        public OdooSessionInfo SessionInfo { get; private set; }

        private IJsonRpcClientFactory RpcFactory { get; set; }

        public OdooRpcClient(OdooConnectionInfo connectionInfo)
            : this(new JsonRpcClientFactory(), connectionInfo)
        {
        }

        internal OdooRpcClient(IJsonRpcClientFactory rpcFactory, OdooConnectionInfo connectionInfo)
        {
            this.RpcFactory = rpcFactory;
            this.SessionInfo = new OdooSessionInfo(connectionInfo);
        }

        public Task<OdooVersionInfo> GetOdooVersion()
        {
            var versionCommand = new OdooVersionCommand(CreateRpcClient());
            return versionCommand.Execute(this.SessionInfo);
        }

        public async Task Authenticate()
        {
            this.SessionInfo.Reset();
            var loginCommand = new OdooLoginCommand(CreateRpcClient());

            await loginCommand.Execute(this.SessionInfo);

            this.SessionInfo.IsLoggedIn = loginCommand.IsLoggedIn;
            this.SessionInfo.UserId = loginCommand.UserId;
        }

        public void SetUserId(long userId)
        {
            this.SessionInfo.IsLoggedIn = true;
            this.SessionInfo.UserId = userId;
        }

        public Task<T> Get<T>(OdooGetParameters parameters)
        {
            var readCommand = new OdooReadCommand(CreateRpcClient());
            return readCommand.Execute<T>(this.SessionInfo, parameters);
        }

        public Task<T> Get<T>(string model, long id)
        {
            var getParams = new OdooGetParameters(model);
            getParams.Ids.Add(id);

            return this.Get<T>(getParams);
        }

        public Task<T> Search<T>(OdooSearchParameters parameters)
        {
            var searchCommand = new OdooSearchCommand(CreateRpcClient());
            return searchCommand.Execute<T>(this.SessionInfo, parameters);
        }

        public Task<long> SearchCount(OdooSearchCountParameters parameters)
        {
            var countCommand = new OdooSearchCountCommand(CreateRpcClient());
            return countCommand.Execute(this.SessionInfo, parameters);
        }

        private IJsonRpcClient CreateRpcClient()
        {
            return this.RpcFactory.GetRpcClient(OdooEndpoints.GetJsonRpcUri(this.SessionInfo));
        }
    }
}