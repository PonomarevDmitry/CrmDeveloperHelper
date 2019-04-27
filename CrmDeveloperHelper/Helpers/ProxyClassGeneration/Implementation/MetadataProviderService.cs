using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public class MetadataProviderService : IMetadataProviderService
    {
        private readonly EntityMetadataRepository _repository;

        public MetadataProviderService(EntityMetadataRepository repository)
        {
            this._repository = repository;
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            return _repository.GetEntityMetadata(entityName);
        }
    }
}
