using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithConnectionList : WindowWithOutputAndCommonConfig
    {
        private readonly object sysObjectConnections = new object();

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly OrganizationServiceExtentedLocker _serviceLocker;

        protected WindowWithConnectionList(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig)
        {
            _connectionCache[service.ConnectionData.ConnectionId] = service;

            this._serviceLocker = new OrganizationServiceExtentedLocker(service);

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);
        }

        protected WindowWithConnectionList(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connectionData
        ) : base(iWriteToOutput, commonConfig)
        {
            this._serviceLocker = new OrganizationServiceExtentedLocker();

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this._connectionCache.Clear();

            this._serviceLocker.Dispose();
        }

        protected async Task<IOrganizationServiceExtented> GetOrganizationService(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

            try
            {
                var service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service != null)
                {
                    _connectionCache[connectionData.ConnectionId] = service;
                    this._serviceLocker.Lock(service);
                }

                return service;
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
        }

        protected abstract void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args);

        protected async Task PublishEntityAsync(ConnectionData connectionData, IEnumerable<string> entityNames)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var service = await GetOrganizationService(connectionData);

            var entityNamesOrdered = string.Join(",", entityNames.OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(entityNames);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionPublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
        }

        protected virtual async Task AddEntityMetadataToSolution(
            ConnectionData connectionData
            , IEnumerable<Guid> idsEntityMetadataEnum
            , bool withSelect
            , string solutionUniqueName
            , SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior
        )
        {
            if (idsEntityMetadataEnum == null || !idsEntityMetadataEnum.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetOrganizationService(connectionData);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(
                    _iWriteToOutput
                    , service
                    , null
                    , _commonConfig
                    , solutionUniqueName
                    , ComponentType.Entity
                    , idsEntityMetadataEnum
                    , rootComponentBehavior
                    , withSelect
                );
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }
    }
}
