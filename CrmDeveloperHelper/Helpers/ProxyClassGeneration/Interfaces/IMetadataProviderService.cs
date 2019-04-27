using Microsoft.Xrm.Sdk.Metadata;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public interface IMetadataProviderService
    {
        EntityMetadata GetEntityMetadata(string entityName);
    }
}
