using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="Implementations.MetadataProviderService"/>
    /// </summary>
    public interface IMetadataProviderService
    {
        /// <summary>
        /// <see cref="Implementations.MetadataProviderService.iCodeGenerationServiceProvider"/>
        /// </summary>
        ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        /// <summary>
        /// <see cref="Implementations.MetadataProviderService.RetrieveEntities(IEnumerable{string})"/>
        /// </summary>
        /// <param name="entityList"></param>
        void RetrieveEntities(IEnumerable<string> entityList);

        /// <summary>
        /// <see cref="Implementations.MetadataProviderService.StoreEntityMetadata(EntityMetadata[])"/>
        /// </summary>
        /// <param name="entityMetadataList"></param>
        void StoreEntityMetadata(params EntityMetadata[] entityMetadataList);

        /// <summary>
        /// <see cref="Implementations.MetadataProviderService.StoreEntityMetadata(IEnumerable{EntityMetadata})"/>
        /// </summary>
        /// <param name="entityMetadataList"></param>
        void StoreEntityMetadata(IEnumerable<EntityMetadata> entityMetadataList);

        /// <summary>
        /// <see cref="Implementations.MetadataProviderService.GetEntityMetadata(string)"/>
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        EntityMetadata GetEntityMetadata(string entityName);
    }
}
