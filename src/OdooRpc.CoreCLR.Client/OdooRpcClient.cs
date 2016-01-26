using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;
using OdooRpc.CoreCLR.Client.Internals;
using OdooRpc.CoreCLR.Client.Internals.Interfaces;
using OdooRpc.CoreCLR.Client.Internals.Commands;
using JsonRpc.CoreCLR.Client.Interfaces;
using System.Collections.Generic;

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

        public Task<Dictionary<string, T>> GetModelFields<T>(OdooGetModelFieldsParameters parameters)
        {
            var getCommand = new OdooGetModelFieldsCommand(CreateRpcClient());
            return getCommand.Execute<T>(this.SessionInfo, parameters);
        }

        public Task<T> Get<T>(OdooGetParameters getParams)
        {
            return Get<T>(getParams, new OdooFieldParameters());
        }

        public Task<T> Get<T>(OdooGetParameters getParams, OdooFieldParameters fieldParams)
        {
            var readCommand = new OdooReadCommand(CreateRpcClient());
            return readCommand.Execute<T>(this.SessionInfo, getParams, fieldParams);
        }

        public Task<T> Get<T>(OdooSearchParameters getParams)
        {
            return Get<T>(getParams, new OdooFieldParameters(), new OdooPaginationParameters());
        }

        public Task<T> Get<T>(OdooSearchParameters getParams, OdooFieldParameters fieldParams)
        {
            return Get<T>(getParams, fieldParams, new OdooPaginationParameters());
        }

        public Task<T> Get<T>(OdooSearchParameters getParams, OdooPaginationParameters pagParams)
        {
            return Get<T>(getParams, new OdooFieldParameters(), pagParams);
        }

        public Task<T> Get<T>(OdooSearchParameters getParams, OdooFieldParameters fieldParams, OdooPaginationParameters pagParams)
        {
            var searchReadCommand = new OdooSearchCommand(CreateRpcClient());
            return searchReadCommand.Execute<T>(this.SessionInfo, getParams, fieldParams, pagParams);
        }

        public Task<T> GetMetadata<T>(OdooMetadataParameters matadataParams)
        {
            var metadataCommand = new OdooMetadataCommand(CreateRpcClient());
            return metadataCommand.Execute<T>(this.SessionInfo, matadataParams);
        }

        public Task<T> GetAll<T>(string model, OdooFieldParameters fieldParameters)
        {
            return Get<T>(new OdooSearchParameters(model), fieldParameters, new OdooPaginationParameters());
        }

        public Task<T> GetAll<T>(string model, OdooFieldParameters fieldParameters, OdooPaginationParameters pagParameters)
        {
            return Get<T>(new OdooSearchParameters(model), fieldParameters, pagParameters);
        }

        public Task<T> Get<T>(string model, long id)
        {
            var getParams = new OdooGetParameters(model);
            getParams.Ids.Add(id);

            return this.Get<T>(getParams, new OdooFieldParameters());
        }

        public Task<T> Search<T>(OdooSearchParameters searchParams)
        {
            var searchCommand = new OdooSearchCommand(CreateRpcClient());
            return searchCommand.Execute<T>(this.SessionInfo, searchParams, new OdooPaginationParameters());
        }

        public Task<T> Search<T>(OdooSearchParameters searchParams, OdooPaginationParameters pagParams)
        {
            var searchCommand = new OdooSearchCommand(CreateRpcClient());
            return searchCommand.Execute<T>(this.SessionInfo, searchParams, pagParams);
        }

        public Task<long> SearchCount(OdooSearchParameters searchParams)
        {
            var searchCommand = new OdooSearchCommand(CreateRpcClient());
            return searchCommand.ExecuteCount(this.SessionInfo, searchParams);
        }

        public Task<long> Create<T>(string model, T newRecord)
        {
            var createCommand = new OdooCreateCommand(CreateRpcClient());
            return createCommand.Execute(this.SessionInfo, model, newRecord);
        }

        public Task Delete(string model, long id)
        {
            return Delete(new OdooDeleteParameters(model, new long[] { id }));
        }

        public Task Delete(OdooDeleteParameters parameters)
        {
            var deleteCommand = new OdooDeleteCommand(CreateRpcClient());
            return deleteCommand.Execute(this.SessionInfo, parameters);
        }

        public Task Update<T>(string model, long id, T updateValues)
        {
            return Update<T>(new OdooUpdateParameters<T>(model, new long[] { id }, updateValues));
        }

        public Task Update<T>(OdooUpdateParameters<T> parameters)
        {
            var updateCommand = new OdooUpdateCommand(CreateRpcClient());
            return updateCommand.Execute<T>(this.SessionInfo, parameters);
        }

        private IJsonRpcClient CreateRpcClient()
        {
            return this.RpcFactory.GetRpcClient(OdooEndpoints.GetJsonRpcUri(this.SessionInfo));
        }
    }
}