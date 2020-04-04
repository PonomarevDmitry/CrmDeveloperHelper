using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="Implementations.NamingService"/>
    /// </summary>
    public interface INamingService
    {
        /// <summary>
        /// <see cref="Implementations.NamingService.iCodeGenerationServiceProvider"/>
        /// </summary>
        ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForOptionSet(EntityMetadata, OptionSetMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <returns></returns>
        string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForOption(OptionSetMetadata, OptionMetadata)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="optionMetadata"></param>
        /// <returns></returns>
        string GetNameForOption(OptionSetMetadata optionSetMetadata, OptionMetadata optionMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForEntity(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        string GetNameForEntity(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForAttributeAsEntityProperty(EntityMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        string GetNameForAttributeAsEntityProperty(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForAttribute(EntityMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForRelationship(EntityMetadata, RelationshipMetadataBase, EntityRole?)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <returns></returns>
        string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForServiceContext()"/>
        /// </summary>
        /// <returns></returns>
        string GetNameForServiceContext();

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForEntitySet(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        string GetNameForEntitySet(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForMessagePair(CodeGenerationSdkMessagePair)"/>
        /// </summary>
        /// <param name="messagePair"></param>
        /// <returns></returns>
        string GetNameForMessagePair(CodeGenerationSdkMessagePair messagePair);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForRequestField(CodeGenerationSdkMessageRequest, Entities.SdkMessageRequestField)"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestField"></param>
        /// <returns></returns>
        string GetNameForRequestField(CodeGenerationSdkMessageRequest request, Entities.SdkMessageRequestField requestField);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetNameForResponseField(CodeGenerationSdkMessageResponse, Entities.SdkMessageResponseField)"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseField"></param>
        /// <returns></returns>
        string GetNameForResponseField(CodeGenerationSdkMessageResponse response, Entities.SdkMessageResponseField responseField);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForEntity(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntity(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForEntityDefaultConstructor(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntityDefaultConstructor(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForEntityAnonymousConstructor(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntityAnonymousConstructor(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForAttribute(EntityMetadata, AttributeMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForRelationshipOneToMany(EntityMetadata, OneToManyRelationshipMetadata, EntityRole?)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipOneToMany(EntityMetadata entityMetadata, OneToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForRelationshipManyToOne(EntityMetadata, OneToManyRelationshipMetadata, EntityRole?)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipManyToOne(EntityMetadata entityMetadata, OneToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForRelationshipManyToMany(EntityMetadata, ManyToManyRelationshipMetadata, EntityRole?)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipManyToMany(EntityMetadata entityMetadata, ManyToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForOptionSet(EntityMetadata, OptionSetMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForOption(OptionSetMetadata, OptionMetadata)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="optionMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForOption(OptionSetMetadata optionSetMetadata, OptionMetadata optionMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForServiceContext()"/>
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForServiceContext();

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForServiceContextConstructor()"/>
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForServiceContextConstructor();

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForEntitySet(EntityMetadata)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntitySet(EntityMetadata entityMetadata);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForMessagePair(CodeGenerationSdkMessagePair)"/>
        /// </summary>
        /// <param name="messagePair"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForMessagePair(CodeGenerationSdkMessagePair messagePair);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForRequestField(CodeGenerationSdkMessageRequest, Entities.SdkMessageRequestField)"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestField"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRequestField(CodeGenerationSdkMessageRequest request, Entities.SdkMessageRequestField requestField);

        /// <summary>
        /// <see cref="Implementations.NamingService.GetCommentsForResponseField(CodeGenerationSdkMessageResponse, Entities.SdkMessageResponseField)"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseField"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForResponseField(CodeGenerationSdkMessageResponse response, Entities.SdkMessageResponseField responseField);

        /// <summary>
        /// <see cref="Implementations.NamingService.SetCurrentTypeName(string)"/>
        /// </summary>
        /// <param name="typeName"></param>
        void SetCurrentTypeName(string typeName);
    }
}
