using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public interface ITypeMappingService
    {
        CodeTypeReference GetTypeForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForAttributeType(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForRelationship(
            RelationshipMetadataBase relationshipMetadata
            , EntityMetadata otherEntityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForRequestField(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForResponseField(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );
    }
}
