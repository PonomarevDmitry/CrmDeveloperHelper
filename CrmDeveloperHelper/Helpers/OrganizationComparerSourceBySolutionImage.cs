using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class OrganizationComparerSourceBySolutionImage : OrganizationComparerSource
    {
        private SolutionImage _solutionImage;

        public OrganizationComparerSourceBySolutionImage(ConnectionData connection1, ConnectionData connection2, SolutionImage solutionImage)
            : base(connection1, connection2)
        {
            this._solutionImage = solutionImage;
        }

        protected override Task<List<Role>> GetRoleAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Role);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<Role>());
            }

            return Task.Run(async () => await GetRoles(service, imageComponents));
        }

        private async Task<List<Role>> GetRoles(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> roles)
        {
            var descriptor = new SolutionComponentDescriptor(service, false);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(roles);

            if (!solutionComponents.Any())
            {
                return new List<Role>();
            }

            return await new RoleRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), new ColumnSet(true));
        }

        protected override Task<List<Workflow>> GetWorkflowAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.Workflow);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<Workflow>());
            }

            return Task.Run(async () => await GetWorkflows(service, imageComponents));
        }

        private async Task<List<Workflow>> GetWorkflows(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> roles)
        {
            var descriptor = new SolutionComponentDescriptor(service, false);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(roles);

            if (!solutionComponents.Any())
            {
                return new List<Workflow>();
            }

            return await new WorkflowRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), new ColumnSet(true));
        }

        protected override Task<List<WebResource>> GetWebResourceAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.WebResource);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<WebResource>());
            }

            return Task.Run(async () => await GetWebResources(service, imageComponents));
        }

        private async Task<List<WebResource>> GetWebResources(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> roles)
        {
            var descriptor = new SolutionComponentDescriptor(service, false);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(roles);

            if (!solutionComponents.Any())
            {
                return new List<WebResource>();
            }

            return await new WebResourceRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), new ColumnSet(true));
        }

        protected override Task<List<ConnectionRole>> GetConnectionRoleAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.ConnectionRole);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<ConnectionRole>());
            }

            return Task.Run(async () => await GetConnectionRoles(service, imageComponents));
        }

        private async Task<List<ConnectionRole>> GetConnectionRoles(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> roles)
        {
            var descriptor = new SolutionComponentDescriptor(service, false);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(roles);

            if (!solutionComponents.Any())
            {
                return new List<ConnectionRole>();
            }

            return await new ConnectionRoleRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), new ColumnSet(true));
        }

        protected override Task<List<ContractTemplate>> GetContractTemplateAsync(IOrganizationServiceExtented service)
        {
            var imageComponents = _solutionImage.Components.Where(c => c.ComponentType == (int)ComponentType.ContractTemplate);

            if (!imageComponents.Any())
            {
                return Task.FromResult(new List<ContractTemplate>());
            }

            return Task.Run(async () => await GetContractTemplates(service, imageComponents));
        }

        private async Task<List<ContractTemplate>> GetContractTemplates(IOrganizationServiceExtented service, IEnumerable<SolutionImageComponent> roles)
        {
            var descriptor = new SolutionComponentDescriptor(service, false);

            var solutionComponents = await descriptor.GetSolutionComponentsListAsync(roles);

            if (!solutionComponents.Any())
            {
                return new List<ContractTemplate>();
            }

            return await new ContractTemplateRepository(service).GetListByIdListAsync(solutionComponents.Select(s => s.ObjectId.Value), new ColumnSet(true));
        }
    }
}
