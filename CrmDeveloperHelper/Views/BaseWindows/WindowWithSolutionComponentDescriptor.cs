using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithSolutionComponentDescriptor : WindowWithConnectionList
    {
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        protected WindowWithSolutionComponentDescriptor(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput, commonConfig, service)
        {
        }

        protected WindowWithSolutionComponentDescriptor(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connectionData
        ) : base(iWriteToOutput, commonConfig, connectionData)
        {
        }

        protected async Task<SolutionComponentDescriptor> GetSolutionComponentDescriptor(ConnectionData connectionData)
        {
            var service = await GetOrganizationService(connectionData);

            if (service == null)
            {
                return null;
            }

            return GetSolutionComponentDescriptor(service);
        }

        protected SolutionComponentDescriptor GetSolutionComponentDescriptor(IOrganizationServiceExtented service)
        {
            if (service == null)
            {
                return null;
            }

            if (!_descriptorCache.ContainsKey(service.ConnectionData.ConnectionId))
            {
                _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service);
            }

            _descriptorCache[service.ConnectionData.ConnectionId].SetSettings(_commonConfig);

            return _descriptorCache[service.ConnectionData.ConnectionId];
        }

        protected override async Task AddEntityMetadataToSolution(
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
            var descriptor = GetSolutionComponentDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(
                    _iWriteToOutput
                    , service
                    , descriptor
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
