using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public class EntitySaverFactory
    {
        public IEntitySaver GetEntitySaver(string entityLogicalName, IOrganizationServiceExtented service)
        {
            if (string.Equals(entityLogicalName, SavedQuery.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new SavedQueryRepository(service);
            }
            else if (string.Equals(entityLogicalName, SavedQueryVisualization.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new SavedQueryVisualizationRepository(service);
            }
            else if (string.Equals(entityLogicalName, SystemForm.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new SystemFormRepository(service);
            }
            else if (string.Equals(entityLogicalName, WebResource.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new WebResourceRepository(service);
            }
            else if (string.Equals(entityLogicalName, SiteMap.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new SitemapRepository(service);
            }
            else if (string.Equals(entityLogicalName, PluginAssembly.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new PluginAssemblyRepository(service);
            }

            return new GenericRepository(service, entityLogicalName);
        }
    }
}
