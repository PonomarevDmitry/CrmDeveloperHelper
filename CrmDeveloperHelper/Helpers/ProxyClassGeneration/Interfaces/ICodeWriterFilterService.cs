using Microsoft.Xrm.Sdk.Metadata;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    public interface ICodeWriterFilterService
    {
        bool GenerateOptionSet(OptionSetMetadata optionSetMetadata, AttributeMetadata attributeMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool IgnoreOptionSet(OptionSetMetadata optionSetMetadata, AttributeMetadata attributeMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateOption(OptionMetadata optionMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateAttribute(AttributeMetadata attributeMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateRelationship(
            RelationshipMetadataBase relationshipMetadata
            , EntityMetadata otherEntityMetadata
            , CodeGenerationRelationshipType relationshipType
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        bool GenerateSdkMessage(CodeGenerationSdkMessage sdkMessage, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateSdkMessagePair(CodeGenerationSdkMessagePair sdkMessagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        bool GenerateServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);
    }
}
