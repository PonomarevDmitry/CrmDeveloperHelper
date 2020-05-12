using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithMessageFilters : WindowWithConnectionList
    {
        private readonly Dictionary<Guid, Task> _cacheTaskGettingMessageFilters = new Dictionary<Guid, Task>();

        private readonly Dictionary<Guid, List<SdkMessageFilter>> _cacheMessageFilters = new Dictionary<Guid, List<SdkMessageFilter>>();

        protected WindowWithMessageFilters(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig, service)
        {

        }

        protected void StartGettingSdkMessageFilters(IOrganizationServiceExtented service)
        {
            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = CreateTaskForGettingSdkMessageFiltersAsync(service);
                }
            }
        }

        private async Task CreateTaskForGettingSdkMessageFiltersAsync(IOrganizationServiceExtented service)
        {
            var repository = new SdkMessageFilterRepository(service);

            var filters = await repository.GetAllAsync(new ColumnSet(SdkMessageFilter.Schema.Attributes.sdkmessageid, SdkMessageFilter.Schema.Attributes.primaryobjecttypecode, SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode, SdkMessageFilter.Schema.Attributes.availability));

            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                _cacheMessageFilters.Add(service.ConnectionData.ConnectionId, filters);
            }

            _cacheTaskGettingMessageFilters.Remove(service.ConnectionData.ConnectionId);
        }

        protected async Task<List<SdkMessageFilter>> GetSdkMessageFiltersAsync(IOrganizationServiceExtented service)
        {
            if (!_cacheMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
            {
                if (!_cacheTaskGettingMessageFilters.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId] = CreateTaskForGettingSdkMessageFiltersAsync(service);
                }

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.GettingMessages);

                await _cacheTaskGettingMessageFilters[service.ConnectionData.ConnectionId];

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.GettingMessagesCompleted);
            }

            return _cacheMessageFilters[service.ConnectionData.ConnectionId];
        }
    }
}