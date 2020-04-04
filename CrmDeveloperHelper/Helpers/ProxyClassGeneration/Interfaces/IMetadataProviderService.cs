using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    public interface IMetadataProviderService
    {
        void RetrieveEntities(IEnumerable<string> entityList);

        void StoreEntityMetadata(params EntityMetadata[] entityMetadataList);

        void StoreEntityMetadata(IEnumerable<EntityMetadata> entityMetadataList);

        EntityMetadata GetEntityMetadata(string entityName);
    }
}
