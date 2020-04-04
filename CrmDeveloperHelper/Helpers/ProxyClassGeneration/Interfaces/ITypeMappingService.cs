using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="Implementations.TypeMappingService"/>
    /// </summary>
    public interface ITypeMappingService
    {
        /// <summary>
        /// <see cref="Implementations.TypeMappingService.iCodeGenerationServiceProvider"/>
        /// </summary>
        ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForEntity(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForEntity(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForOptionSet(EntityMetadata, OptionSetMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata);

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForAttributeType(EntityMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForAttributeType(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForRelationship(RelationshipMetadataBase, EntityMetadata)"/>
        /// </summary>
        /// <param name="relationshipMetadata"></param>
        /// <param name="otherEntityMetadata"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata);

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForRequestField(CodeGenerationSdkMessageRequest, Entities.SdkMessageRequestField)"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestField"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForRequestField(CodeGenerationSdkMessageRequest request, Entities.SdkMessageRequestField requestField);

        /// <summary>
        /// <see cref="Implementations.TypeMappingService.GetTypeForResponseField(Entities.SdkMessageResponseField)"/>
        /// </summary>
        /// <param name="responseField"></param>
        /// <returns></returns>
        CodeTypeReference GetTypeForResponseField(Entities.SdkMessageResponseField responseField);
    }
}
