using Microsoft.Xrm.Sdk.Metadata;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="Implementations.CodeWriterFilterService"/>
    /// </summary>
    public interface ICodeWriterFilterService
    {
        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService"/>
        /// </summary>
        ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateOptionSet(OptionSetMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        bool GenerateOptionSet(OptionSetMetadata optionSetMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.IgnoreOptionSet(OptionSetMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        bool IgnoreOptionSet(OptionSetMetadata optionSetMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateOption(OptionMetadata)"/>
        /// </summary>
        /// <param name="optionMetadata"></param>
        /// <returns></returns>
        bool GenerateOption(OptionMetadata optionMetadata);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateEntity(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        bool GenerateEntity(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateAttribute(AttributeMetadata)"/>
        /// </summary>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        bool GenerateAttribute(AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateRelationship(RelationshipMetadataBase, EntityMetadata, CodeGenerationRelationshipType)"/>
        /// </summary>
        /// <param name="relationshipMetadata"></param>
        /// <param name="otherEntityMetadata"></param>
        /// <param name="relationshipType"></param>
        /// <returns></returns>
        bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, CodeGenerationRelationshipType relationshipType);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateSdkMessage(CodeGenerationSdkMessage)"/>
        /// </summary>
        /// <param name="sdkMessage"></param>
        /// <returns></returns>
        bool GenerateSdkMessage(CodeGenerationSdkMessage sdkMessage);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateSdkMessagePair(CodeGenerationSdkMessagePair)"/>
        /// </summary>
        /// <param name="sdkMessagePair"></param>
        /// <returns></returns>
        bool GenerateSdkMessagePair(CodeGenerationSdkMessagePair sdkMessagePair);

        /// <summary>
        /// <see cref="Implementations.CodeWriterFilterService.GenerateServiceContext"/>
        /// </summary>
        /// <returns></returns>
        bool GenerateServiceContext();
    }
}
