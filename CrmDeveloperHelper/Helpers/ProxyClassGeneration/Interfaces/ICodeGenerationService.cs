using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public interface ICodeGenerationService
    {
        Task WriteEntitiesFileAsync(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadataBase> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        Task WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        Task WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        void WriteEntitiesFile(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadataBase> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        void WriteEntityFile(
            EntityMetadata entityMetadata
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        void WriteSdkMessage(
            CodeGenerationSdkMessage sdkMessage
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadataBase optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForOption(
            OptionSetMetadataBase optionSetMetadata
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
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        CodeGenerationType GetTypeForResponseField(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );
    }
}
