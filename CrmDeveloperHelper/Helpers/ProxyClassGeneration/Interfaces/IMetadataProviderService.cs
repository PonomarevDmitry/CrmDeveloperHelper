using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public interface IMetadataProviderService
    {
        void StoreEntities(IEnumerable<string> entityList);

        EntityMetadata GetEntityMetadata(string entityName);
    }
}
