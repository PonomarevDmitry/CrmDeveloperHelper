using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces
{
    /// <summary>
    /// <see cref="NamingService"/>
    /// </summary>
    public interface INamingService
    {
        /// <summary>
        /// <see cref="NamingService.GetNameForOptionSet(EntityMetadata, OptionSetMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForOption(OptionSetMetadata, OptionMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="optionMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForEntity(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetNameForAttributeAsEntityProperty(EntityMetadata, AttributeMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForAttributeAsEntityProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForAttribute(EntityMetadata, AttributeMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForRelationship(EntityMetadata, RelationshipMetadataBase, EntityRole?, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForRelationship(
            EntityMetadata entityMetadata
            , RelationshipMetadataBase relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForServiceContext(ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetNameForEntitySet(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForEntitySet(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetNameForMessagePair(CodeGenerationSdkMessagePair, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="messagePair"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForMessagePair(CodeGenerationSdkMessagePair messagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetNameForRequestField(CodeGenerationSdkMessageRequest, Entities.SdkMessageRequestField, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestField"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForRequestField(
            CodeGenerationSdkMessageRequest request
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetNameForResponseField(CodeGenerationSdkMessageResponse, Entities.SdkMessageResponseField, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseField"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        string GetNameForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForEntity(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForEntityDefaultConstructor(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntityDefaultConstructor(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForEntityAnonymousConstructor(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntityAnonymousConstructor(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForAttribute(EntityMetadata, AttributeMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="attributeMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForRelationshipOneToMany(EntityMetadata, OneToManyRelationshipMetadata, EntityRole?, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipOneToMany(
            EntityMetadata entityMetadata
            , OneToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForRelationshipManyToOne(EntityMetadata, OneToManyRelationshipMetadata, EntityRole?, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipManyToOne(
            EntityMetadata entityMetadata
            , OneToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForRelationshipManyToMany(EntityMetadata, ManyToManyRelationshipMetadata, EntityRole?, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="relationshipMetadata"></param>
        /// <param name="reflexiveRole"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRelationshipManyToMany(
            EntityMetadata entityMetadata
            , ManyToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForOptionSet(EntityMetadata, OptionSetMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="optionSetMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForOption(OptionSetMetadata, OptionMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="optionSetMetadata"></param>
        /// <param name="optionMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForServiceContext(ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForServiceContextConstructor(ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForServiceContextConstructor(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForEntitySet(EntityMetadata, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="entityMetadata"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForEntitySet(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForMessagePair(CodeGenerationSdkMessagePair, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="messagePair"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForMessagePair(CodeGenerationSdkMessagePair messagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        /// <summary>
        /// <see cref="NamingService.GetCommentsForRequestField(CodeGenerationSdkMessageRequest, Entities.SdkMessageRequestField, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestField"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForRequestField(
            CodeGenerationSdkMessageRequest request
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.GetCommentsForResponseField(CodeGenerationSdkMessageResponse, Entities.SdkMessageResponseField, ICodeGenerationServiceProvider)"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseField"></param>
        /// <param name="iCodeGenerationServiceProvider"></param>
        /// <returns></returns>
        IEnumerable<string> GetCommentsForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        /// <summary>
        /// <see cref="NamingService.SetCurrentTypeName(string)"/>
        /// </summary>
        /// <param name="typeName"></param>
        void SetCurrentTypeName(string typeName);
    }
}
