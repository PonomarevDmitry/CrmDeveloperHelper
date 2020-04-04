using Microsoft.Xrm.Sdk.Metadata;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="Implementations.CodeGenerationService"/>
    /// </summary>
    public interface ICodeGenerationService
    {
        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.iCodeGenerationServiceProvider"/>
        /// </summary>
        ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.WriteEntitiesFileAsync(IEnumerable{EntityMetadata}, IEnumerable{OptionSetMetadata}, IEnumerable{CodeGenerationSdkMessage}, string, string, CodeGeneratorOptions)"/>
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="optionSets"></param>
        /// <param name="messages"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="outputNamespace"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task WriteEntitiesFileAsync(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadata> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        );

        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.WriteEntityFileAsync(EntityMetadata, string, string, CodeGeneratorOptions)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="outputNamespace"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        );

        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.WriteSdkMessageAsync(CodeGenerationSdkMessage, string, string, CodeGeneratorOptions)"/>
        /// </summary>
        /// <param name="sdkMessage"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="outputNamespace"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        );

        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.WriteSdkMessagePairAsync(CodeGenerationSdkMessagePair, string, string, CodeGeneratorOptions)"/>
        /// </summary>
        /// <param name="sdkMessagePair"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="outputNamespace"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task WriteSdkMessagePairAsync(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        );

        /// <summary>
        /// <see cref="Implementations.CodeGenerationService.GetTypeForOptionSet(EntityMetadata, OptionSetMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <returns></returns>
        CodeGenerationType GetTypeForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata);
    }
}
