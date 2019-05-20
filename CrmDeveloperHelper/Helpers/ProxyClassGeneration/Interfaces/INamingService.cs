using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    public interface INamingService
    {
        string GetNameForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        string GetNameForAttributeAsEntityProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForRelationship(
            EntityMetadata entityMetadata
            , RelationshipMetadataBase relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        string GetNameForEntitySet(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        string GetNameForMessagePair(CodeGenerationSdkMessagePair messagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        string GetNameForRequestField(
            CodeGenerationSdkMessageRequest request
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        string GetNameForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForEntityDefaultConstructor(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForEntityAnonymousConstructor(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForRelationshipOneToMany(
            EntityMetadata entityMetadata
            , OneToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForRelationshipManyToOne(
            EntityMetadata entityMetadata
            , OneToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForRelationshipManyToMany(
            EntityMetadata entityMetadata
            , ManyToManyRelationshipMetadata relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForServiceContextConstructor(ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForEntitySet(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForMessagePair(CodeGenerationSdkMessagePair messagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider);

        IEnumerable<string> GetCommentsForRequestField(
            CodeGenerationSdkMessageRequest request
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );

        IEnumerable<string> GetCommentsForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        );
    }
}
