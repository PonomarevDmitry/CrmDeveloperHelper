using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    /// <summary>
    /// <see cref="CodeGenerationService"/>
    /// </summary>
    public interface ICodeGenerationService
    {
        Task WriteEntitiesFileAsync(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadata> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        Task WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        Task WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        Task WriteSdkMessagePairAsync(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForRequestField(
            CodeGenerationSdkMessageRequest request
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );
    }
}
