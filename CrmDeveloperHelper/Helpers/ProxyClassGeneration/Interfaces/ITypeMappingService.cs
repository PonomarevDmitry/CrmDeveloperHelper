using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    public interface ITypeMappingService
    {
        CodeTypeReference GetTypeForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
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
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeTypeReference GetTypeForResponseField(
            Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );
    }
}
