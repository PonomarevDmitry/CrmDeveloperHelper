using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    internal sealed class CodeGenerationService : ICodeGenerationService
    {
        private static Type AttributeLogicalNameAttribute = typeof(Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute);
        private static Type EntityLogicalNameAttribute = typeof(Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute);
        private static Type RelationshipSchemaNameAttribute = typeof(Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute);

        private static Type ObsoleteFieldAttribute = typeof(ObsoleteAttribute);
        private static Type ServiceContextBaseType = typeof(OrganizationServiceContext);

        private static Type EntityClassBaseType = typeof(Entity);
        private static Type RequestClassBaseType = typeof(OrganizationRequest);
        private static Type ResponseClassBaseType = typeof(OrganizationResponse);

        private static readonly string RequestClassSuffix = "Request";
        private static readonly string ResponseClassSuffix = "Response";
        private static string RequestNamePropertyName = "RequestName";
        private static string ParametersPropertyName = "Parameters";
        private static string ResultsPropertyName = "Results";

        Task ICodeGenerationService.WriteEntitiesFileAsync(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadataBase> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => (this as ICodeGenerationService).WriteEntitiesFile(entities, optionSets, messages, language, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        Task ICodeGenerationService.WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => (this as ICodeGenerationService).WriteEntityFile(entityMetadata, language, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        Task ICodeGenerationService.WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => (this as ICodeGenerationService).WriteSdkMessage(sdkMessage, language, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        void ICodeGenerationService.WriteEntitiesFile(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadataBase> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeNamespace codeNamespace = CodeGenerationService.Namespace(outputNamespace);

            codeNamespace.Types.AddRange(CodeGenerationService.BuildEntities(entities, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(CodeGenerationService.BuildOptionSets(optionSets, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(CodeGenerationService.BuildMessages(messages, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(CodeGenerationService.BuildServiceContext(entities, iCodeGenerationServiceProvider));

            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            codeCompileUnit.AssemblyCustomAttributes.Add(CodeGenerationService.Attribute(typeof(ProxyTypesAssemblyAttribute)));

            //CodeGeneratorOptions options = new CodeGeneratorOptions
            //{
            //    BlankLinesBetweenMembers = true,
            //    BracingStyle = "C",
            //    IndentString = "\t",
            //    VerbatimOrder = true,
            //};

            using (StreamWriter streamWriter = new StreamWriter(outputFilePath))
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        void ICodeGenerationService.WriteEntityFile(
            EntityMetadata entityMetadata
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeNamespace codeNamespace = CodeGenerationService.Namespace(outputNamespace);

            codeNamespace.Types.AddRange(CodeGenerationService.BuildEntity(entityMetadata, iCodeGenerationServiceProvider));

            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            using (StreamWriter streamWriter = new StreamWriter(outputFilePath))
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        void ICodeGenerationService.WriteSdkMessage(
            CodeGenerationSdkMessage sdkMessage
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeNamespace codeNamespace = CodeGenerationService.Namespace(outputNamespace);

            codeNamespace.Types.AddRange(CodeGenerationService.BuildMessage(sdkMessage, iCodeGenerationServiceProvider));

            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            using (StreamWriter streamWriter = new StreamWriter(outputFilePath))
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        CodeGenerationType ICodeGenerationService.GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadataBase optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Enum;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForOption(
            OptionSetMetadataBase optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Field;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Class;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Class;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForRequestField(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        CodeGenerationType ICodeGenerationService.GetTypeForResponseField(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        private static CodeTypeDeclarationCollection BuildOptionSets(
            IEnumerable<OptionSetMetadataBase> optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            foreach (OptionSetMetadataBase optionSetMetadataBase in optionSetMetadata)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(optionSetMetadataBase, iCodeGenerationServiceProvider) && optionSetMetadataBase.IsGlobal.HasValue && optionSetMetadataBase.IsGlobal.Value)
                {
                    CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.BuildOptionSet(null, optionSetMetadataBase, iCodeGenerationServiceProvider);
                    if (codeTypeDeclaration != null)
                    {
                        declarationCollection.Add(codeTypeDeclaration);
                    }
                }
            }

            return declarationCollection;
        }

        private static CodeTypeDeclaration BuildOptionSet(
            EntityMetadata entity
            , OptionSetMetadataBase optionSet
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.Enum(iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entity, optionSet, iCodeGenerationServiceProvider), CodeGenerationService.Attribute(typeof(DataContractAttribute)));

            OptionSetMetadata optionSetMetadata = optionSet as OptionSetMetadata;
            if (optionSetMetadata == null)
            {
                return null;
            }

            foreach (OptionMetadata option in optionSetMetadata.Options)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOption(option, iCodeGenerationServiceProvider))
                {
                    codeTypeDeclaration.Members.Add(CodeGenerationService.BuildOption(optionSet, option, iCodeGenerationServiceProvider));
                }
            }

            return codeTypeDeclaration;
        }

        private static CodeTypeMember BuildOption(
            OptionSetMetadataBase optionSet
            , OptionMetadata option
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeMemberField codeMemberField = CodeGenerationService.Field(iCodeGenerationServiceProvider.NamingService.GetNameForOption(optionSet, option, iCodeGenerationServiceProvider), typeof(int), option.Value.Value, CodeGenerationService.Attribute(typeof(EnumMemberAttribute)));
            return codeMemberField;
        }

        private static CodeTypeDeclarationCollection BuildEntities(
            IEnumerable<EntityMetadata> entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            foreach (EntityMetadata entityMetadata1 in entityMetadata.OrderBy(metadata => metadata.LogicalName))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(CodeGenerationService.BuildEntity(entityMetadata1, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private static CodeTypeDeclarationCollection BuildEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            CodeTypeDeclaration entityClass = CodeGenerationService.Class(iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider), CodeGenerationService.TypeRef(CodeGenerationService.EntityClassBaseType), CodeGenerationService.Attribute(typeof(DataContractAttribute)), CodeGenerationService.Attribute(CodeGenerationService.EntityLogicalNameAttribute, CodeGenerationService.AttributeArg(entityMetadata.LogicalName)));
            CodeGenerationService.InitializeEntityClass(entityClass, entityMetadata, iCodeGenerationServiceProvider);

            entityClass.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntity(entityMetadata, iCodeGenerationServiceProvider)));

            CodeTypeMember attributeMember = null;

            foreach (AttributeMetadata attributeMetadata in ((IEnumerable<AttributeMetadata>)entityMetadata.Attributes).OrderBy<AttributeMetadata, string>(metadata => metadata.LogicalName))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider))
                {
                    attributeMember = CodeGenerationService.BuildAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);
                    entityClass.Members.Add(attributeMember);
                    if (entityMetadata.PrimaryIdAttribute == attributeMetadata.LogicalName && attributeMetadata.IsPrimaryId.GetValueOrDefault())
                    {
                        entityClass.Members.Add(CodeGenerationService.BuildIdProperty(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider));
                    }
                }

                CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.BuildAttributeOptionSet(entityMetadata, attributeMetadata, attributeMember, iCodeGenerationServiceProvider);
                if (codeTypeDeclaration != null)
                {
                    declarationCollection.Add(codeTypeDeclaration);
                }
            }

            entityClass.Members.AddRange(CodeGenerationService.BuildOneToManyRelationships(entityMetadata, iCodeGenerationServiceProvider));
            entityClass.Members.AddRange(CodeGenerationService.BuildManyToManyRelationships(entityMetadata, iCodeGenerationServiceProvider));
            entityClass.Members.AddRange(CodeGenerationService.BuildManyToOneRelationships(entityMetadata, iCodeGenerationServiceProvider));

            declarationCollection.Add(entityClass);

            return declarationCollection;
        }

        private static void InitializeEntityClass(
            CodeTypeDeclaration entityClass
            , EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            entityClass.BaseTypes.Add(CodeGenerationService.TypeRef(typeof(INotifyPropertyChanging)));
            entityClass.BaseTypes.Add(CodeGenerationService.TypeRef(typeof(INotifyPropertyChanged)));

            entityClass.Members.Add(CodeGenerationService.EntityConstructor(entityMetadata, iCodeGenerationServiceProvider));

            entityClass.Members.Add(CodeGenerationService.EntityLogicalNameConstant(entityMetadata));
            entityClass.Members.Add(CodeGenerationService.EntityTypeCodeConstant(entityMetadata));

            entityClass.Members.Add(CodeGenerationService.Event("PropertyChanged", typeof(PropertyChangedEventHandler), typeof(INotifyPropertyChanged)));
            entityClass.Members.Add(CodeGenerationService.Event("PropertyChanging", typeof(PropertyChangingEventHandler), typeof(INotifyPropertyChanging)));
            entityClass.Members.Add(CodeGenerationService.RaiseEvent("OnPropertyChanged", "PropertyChanged", typeof(PropertyChangedEventArgs)));
            entityClass.Members.Add(CodeGenerationService.RaiseEvent("OnPropertyChanging", "PropertyChanging", typeof(PropertyChangingEventArgs)));
        }

        private static CodeTypeMember EntityLogicalNameConstant(EntityMetadata entity)
        {
            CodeMemberField codeMemberField = CodeGenerationService.Field("EntityLogicalName", typeof(string), entity.LogicalName);
            codeMemberField.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            return codeMemberField;
        }

        private static CodeTypeMember EntityTypeCodeConstant(EntityMetadata entity)
        {
            CodeMemberField codeMemberField = CodeGenerationService.Field("EntityTypeCode", typeof(int), entity.ObjectTypeCode.GetValueOrDefault());
            codeMemberField.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            return codeMemberField;
        }

        private static CodeTypeMember EntityConstructor(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            CodeConstructor codeConstructor = CodeGenerationService.Constructor();
            codeConstructor.BaseConstructorArgs.Add(CodeGenerationService.VarRef("EntityLogicalName"));

            codeConstructor.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntityDefaultConstructor(entityMetadata, iCodeGenerationServiceProvider)));

            return codeConstructor;
        }

        private static CodeTypeMember BuildAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference forAttributeType = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForAttributeType(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(forAttributeType, iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attributeMetadata.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;

            if (codeMemberProperty.HasGet)
            {
                codeMemberProperty.GetStatements.AddRange(CodeGenerationService.BuildAttributeGet(attributeMetadata, forAttributeType));
            }

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.AddRange(CodeGenerationService.BuildAttributeSet(entityMetadata, attributeMetadata, codeMemberProperty.Name));
            }

            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.Attribute(CodeGenerationService.AttributeLogicalNameAttribute, CodeGenerationService.AttributeArg(attributeMetadata.LogicalName)));
            if (attributeMetadata.DeprecatedVersion != null)
            {
                codeMemberProperty.CustomAttributes.Add(CodeGenerationService.Attribute(CodeGenerationService.ObsoleteFieldAttribute));
            }

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeStatementCollection BuildAttributeGet(
            AttributeMetadata attribute
            , CodeTypeReference targetType
        )
        {
            CodeStatementCollection statementCollection = new CodeStatementCollection();

            if (attribute.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList && targetType.TypeArguments.Count > 0)
            {
                statementCollection.AddRange(CodeGenerationService.BuildEntityCollectionAttributeGet(attribute.LogicalName, targetType));
            }
            else
            {
                statementCollection.Add(CodeGenerationService.Return(CodeGenerationService.ThisMethodInvoke("GetAttributeValue", targetType, (CodeExpression)CodeGenerationService.StringLiteral(attribute.LogicalName))));
            }

            return statementCollection;
        }

        private static CodeStatementCollection BuildAttributeSet(
            EntityMetadata entity
            , AttributeMetadata attribute
            , string propertyName
        )
        {
            CodeStatementCollection statementCollection = new CodeStatementCollection
            {
                CodeGenerationService.ThisMethodInvoke("OnPropertyChanging", (CodeExpression)CodeGenerationService.StringLiteral(propertyName))
            };

            if (attribute.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList)
            {
                statementCollection.Add(CodeGenerationService.BuildEntityCollectionAttributeSet(attribute.LogicalName));
            }
            else
            {
                statementCollection.Add(CodeGenerationService.ThisMethodInvoke("SetAttributeValue", CodeGenerationService.StringLiteral(attribute.LogicalName), CodeGenerationService.VarRef("value")));
            }

            if (entity.PrimaryIdAttribute == attribute.LogicalName && attribute.IsPrimaryId.GetValueOrDefault())
            {
                statementCollection.Add(CodeGenerationService.If(CodeGenerationService.PropRef(CodeGenerationService.VarRef("value"), "HasValue"), CodeGenerationService.AssignValue(CodeGenerationService.BaseProp("Id"), CodeGenerationService.PropRef(CodeGenerationService.VarRef("value"), "Value")), CodeGenerationService.AssignValue(CodeGenerationService.BaseProp("Id"), CodeGenerationService.GuidEmpty())));
            }

            statementCollection.Add(CodeGenerationService.ThisMethodInvoke("OnPropertyChanged", (CodeExpression)CodeGenerationService.StringLiteral(propertyName)));

            return statementCollection;
        }

        private static CodeStatementCollection BuildEntityCollectionAttributeGet(
            string attributeLogicalName
            , CodeTypeReference propertyType
        )
        {
            return new CodeStatementCollection()
            {
                CodeGenerationService.Var(typeof (EntityCollection), "collection",  CodeGenerationService.ThisMethodInvoke("GetAttributeValue", CodeGenerationService.TypeRef(typeof (EntityCollection)), (CodeExpression) CodeGenerationService.StringLiteral(attributeLogicalName))),
                CodeGenerationService.If( CodeGenerationService.And( CodeGenerationService.NotNull( CodeGenerationService.VarRef("collection")),  CodeGenerationService.NotNull( CodeGenerationService.PropRef( CodeGenerationService.VarRef("collection"), "Entities"))),  CodeGenerationService.Return( CodeGenerationService.StaticMethodInvoke(typeof (Enumerable), "Cast", propertyType.TypeArguments[0], (CodeExpression) CodeGenerationService.PropRef( CodeGenerationService.VarRef("collection"), "Entities"))),  CodeGenerationService.Return( CodeGenerationService.Null()))
            };
        }

        private static CodeStatement BuildEntityCollectionAttributeSet(
            string attributeLogicalName
        )
        {
            return CodeGenerationService.If(CodeGenerationService.ValueNull(), CodeGenerationService.ThisMethodInvoke("SetAttributeValue", CodeGenerationService.StringLiteral(attributeLogicalName), CodeGenerationService.VarRef("value")), CodeGenerationService.ThisMethodInvoke("SetAttributeValue", CodeGenerationService.StringLiteral(attributeLogicalName), CodeGenerationService.New(CodeGenerationService.TypeRef(typeof(EntityCollection)), (CodeExpression)CodeGenerationService.New(CodeGenerationService.TypeRef(typeof(List<Entity>)), (CodeExpression)CodeGenerationService.VarRef("value")))));
        }

        private static CodeTypeMember BuildIdProperty(
            EntityMetadata entity
            , AttributeMetadata attribute
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(CodeGenerationService.TypeRef(typeof(Guid)), "Id");

            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.Attribute(CodeGenerationService.AttributeLogicalNameAttribute, CodeGenerationService.AttributeArg(attribute.LogicalName)));
            codeMemberProperty.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            codeMemberProperty.HasSet = attribute.IsValidForCreate.GetValueOrDefault() || attribute.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attribute.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;
            codeMemberProperty.GetStatements.Add(CodeGenerationService.Return(CodeGenerationService.BaseProp("Id")));

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.Add(CodeGenerationService.AssignValue(CodeGenerationService.ThisProp(iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entity, attribute, iCodeGenerationServiceProvider)), CodeGenerationService.VarRef("value")));
            }
            else
            {
                codeMemberProperty.SetStatements.Add(CodeGenerationService.AssignValue(CodeGenerationService.BaseProp("Id"), CodeGenerationService.VarRef("value")));
            }

            return codeMemberProperty;
        }

        private static CodeTypeDeclaration BuildAttributeOptionSet(
            EntityMetadata entity
            , AttributeMetadata attribute
            , CodeTypeMember attributeMember
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            OptionSetMetadataBase attributeOptionSet = TypeMappingService.GetAttributeOptionSet(attribute);
            if (attributeOptionSet == null || !iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(attributeOptionSet, iCodeGenerationServiceProvider))
            {
                if (attributeOptionSet != null)
                {
                }

                return null;
            }

            CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.BuildOptionSet(entity, attributeOptionSet, iCodeGenerationServiceProvider);
            if (codeTypeDeclaration == null)
            {
                return null;
            }

            CodeGenerationService.UpdateAttributeMemberStatements(attribute, attributeMember);
            return codeTypeDeclaration;
        }

        private static void UpdateAttributeMemberStatements(
            AttributeMetadata attribute
            , CodeTypeMember attributeMember
        )
        {
            CodeMemberProperty codeMemberProperty = attributeMember as CodeMemberProperty;
            if (codeMemberProperty.HasGet)
            {
                codeMemberProperty.GetStatements.Clear();
                codeMemberProperty.GetStatements.AddRange(CodeGenerationService.BuildOptionSetAttributeGet(attribute, codeMemberProperty.Type));
            }

            if (!codeMemberProperty.HasSet)
            {
                return;
            }

            codeMemberProperty.SetStatements.Clear();
            codeMemberProperty.SetStatements.AddRange(CodeGenerationService.BuildOptionSetAttributeSet(attribute, codeMemberProperty.Name));
        }

        private static CodeStatementCollection BuildOptionSetAttributeGet(
            AttributeMetadata attribute
            , CodeTypeReference attributeType
        )
        {
            CodeTypeReference codeTypeReference = attributeType;
            if (codeTypeReference.TypeArguments.Count > 0)
            {
                codeTypeReference = codeTypeReference.TypeArguments[0];
            }

            return new CodeStatementCollection(new CodeStatement[2]
            {
                CodeGenerationService.Var(typeof (OptionSetValue), "optionSet",  CodeGenerationService.ThisMethodInvoke("GetAttributeValue", CodeGenerationService.TypeRef(typeof (OptionSetValue)), (CodeExpression) CodeGenerationService.StringLiteral(attribute.LogicalName))),
                CodeGenerationService.If( CodeGenerationService.NotNull( CodeGenerationService.VarRef("optionSet")),  CodeGenerationService.Return( CodeGenerationService.Cast(codeTypeReference,  CodeGenerationService.ConvertEnum(codeTypeReference, "optionSet"))),  CodeGenerationService.Return( CodeGenerationService.Null()))
            });
        }

        private static CodeStatementCollection BuildOptionSetAttributeSet(
            AttributeMetadata attribute
            , string propertyName
        )
        {
            return new CodeStatementCollection()
            {
                CodeGenerationService.ThisMethodInvoke("OnPropertyChanging", (CodeExpression) CodeGenerationService.StringLiteral(propertyName)),
                CodeGenerationService.If(CodeGenerationService.ValueNull(),  CodeGenerationService.ThisMethodInvoke("SetAttributeValue",  CodeGenerationService.StringLiteral(attribute.LogicalName),  CodeGenerationService.Null()),  CodeGenerationService.ThisMethodInvoke("SetAttributeValue",  CodeGenerationService.StringLiteral(attribute.LogicalName),  CodeGenerationService.New(CodeGenerationService.TypeRef(typeof (OptionSetValue)), (CodeExpression) CodeGenerationService.Cast(CodeGenerationService.TypeRef(typeof (int)),  CodeGenerationService.VarRef("value"))))),
                CodeGenerationService.ThisMethodInvoke("OnPropertyChanged", (CodeExpression) CodeGenerationService.StringLiteral(propertyName))
            };
        }

        private static CodeTypeMemberCollection BuildOneToManyRelationships(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();
            if (entity.OneToManyRelationships == null)
            {
                return memberCollection;
            }

            foreach (OneToManyRelationshipMetadata oneToMany in entity.OneToManyRelationships.OfType<OneToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                EntityMetadata entityMetadata = CodeGenerationService.GetEntityMetadata(oneToMany.ReferencingEntity, iCodeGenerationServiceProvider);
                if (entityMetadata != null)
                {
                    if (string.Equals(oneToMany.SchemaName, "calendar_calendar_rules", StringComparison.Ordinal) || string.Equals(oneToMany.SchemaName, "service_calendar_rules", StringComparison.Ordinal))
                    {
                        memberCollection.Add(CodeGenerationService.BuildCalendarRuleAttribute(entity, entityMetadata, oneToMany, iCodeGenerationServiceProvider));
                    }
                    else if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata, iCodeGenerationServiceProvider) && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(oneToMany, entityMetadata, iCodeGenerationServiceProvider))
                    {
                        memberCollection.Add(CodeGenerationService.BuildOneToMany(entity, entityMetadata, oneToMany, iCodeGenerationServiceProvider));
                    }
                }
            }

            return memberCollection;
        }

        private static CodeTypeMember BuildCalendarRuleAttribute(
            EntityMetadata entity
            , EntityMetadata otherEntity
            , OneToManyRelationshipMetadata oneToMany
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(CodeGenerationService.IEnumerable(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntity, iCodeGenerationServiceProvider)), "CalendarRules");

            EntityRole? nullable = oneToMany.ReferencingEntity == entity.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            codeMemberProperty.GetStatements.AddRange(CodeGenerationService.BuildEntityCollectionAttributeGet("calendarrules", codeMemberProperty.Type));

            codeMemberProperty.SetStatements.Add(CodeGenerationService.ThisMethodInvoke("OnPropertyChanging", (CodeExpression)CodeGenerationService.StringLiteral(codeMemberProperty.Name)));
            codeMemberProperty.SetStatements.Add(CodeGenerationService.BuildEntityCollectionAttributeSet("calendarrules"));
            codeMemberProperty.SetStatements.Add(CodeGenerationService.ThisMethodInvoke("OnPropertyChanged", (CodeExpression)CodeGenerationService.StringLiteral(codeMemberProperty.Name)));

            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.Attribute(CodeGenerationService.AttributeLogicalNameAttribute, CodeGenerationService.AttributeArg("calendarrules")));

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entity, oneToMany, nullable, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeTypeMember BuildOneToMany(
            EntityMetadata entity
            , EntityMetadata otherEntity
            , OneToManyRelationshipMetadata oneToMany
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntity, iCodeGenerationServiceProvider);

            EntityRole? entityRole = oneToMany.ReferencingEntity == entity.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(CodeGenerationService.IEnumerable(typeForRelationship), iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, oneToMany, entityRole, iCodeGenerationServiceProvider));

            codeMemberProperty.GetStatements.Add(CodeGenerationService.BuildRelationshipGet("GetRelatedEntities", oneToMany, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(CodeGenerationService.BuildRelationshipSet("SetRelatedEntities", oneToMany, typeForRelationship, codeMemberProperty.Name, entityRole));
            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.BuildRelationshipSchemaNameAttribute(oneToMany.SchemaName, entityRole));

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entity, oneToMany, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeTypeMemberCollection BuildManyToManyRelationships(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();

            if (entity.ManyToManyRelationships == null)
            {
                return memberCollection;
            }

            foreach (ManyToManyRelationshipMetadata manyToMany in entity.ManyToManyRelationships.OfType<ManyToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                EntityMetadata entityMetadata = CodeGenerationService.GetEntityMetadata(entity.LogicalName != manyToMany.Entity1LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName, iCodeGenerationServiceProvider);
                if (entityMetadata != null)
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata, iCodeGenerationServiceProvider)
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToMany, entityMetadata, iCodeGenerationServiceProvider)
                    )
                    {
                        if (entityMetadata.LogicalName != entity.LogicalName)
                        {
                            string nameForRelationship = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(), iCodeGenerationServiceProvider);
                            CodeTypeMember many = CodeGenerationService.BuildManyToMany(entity, entityMetadata, manyToMany, nameForRelationship, new EntityRole?(), iCodeGenerationServiceProvider);
                            memberCollection.Add(many);
                        }
                        else
                        {
                            string nameForRelationship1 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);
                            CodeTypeMember many1 = CodeGenerationService.BuildManyToMany(entity, entityMetadata, manyToMany, nameForRelationship1, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);
                            memberCollection.Add(many1);

                            string nameForRelationship2 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);
                            CodeTypeMember many2 = CodeGenerationService.BuildManyToMany(entity, entityMetadata, manyToMany, nameForRelationship2, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);
                            memberCollection.Add(many2);
                        }
                    }
                }
            }

            return memberCollection;
        }

        private static CodeTypeMember BuildManyToMany(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , ManyToManyRelationshipMetadata manyToMany
            , string propertyName
            , EntityRole? entityRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToMany, otherEntityMetadata, iCodeGenerationServiceProvider);

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(CodeGenerationService.IEnumerable(typeForRelationship), propertyName);

            codeMemberProperty.GetStatements.Add(CodeGenerationService.BuildRelationshipGet("GetRelatedEntities", manyToMany, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(CodeGenerationService.BuildRelationshipSet("SetRelatedEntities", manyToMany, typeForRelationship, propertyName, entityRole));

            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.BuildRelationshipSchemaNameAttribute(manyToMany.SchemaName, entityRole));

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToMany(entityMetadata, manyToMany, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeTypeMemberCollection BuildManyToOneRelationships(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeMemberCollection memberCollection = new CodeTypeMemberCollection();

            if (entityMetadata.ManyToOneRelationships == null)
            {
                return memberCollection;
            }

            foreach (OneToManyRelationshipMetadata manyToOne in ((IEnumerable<OneToManyRelationshipMetadata>)entityMetadata.ManyToOneRelationships).OrderBy(metadata => metadata.SchemaName))
            {
                EntityMetadata otherEntity = CodeGenerationService.GetEntityMetadata(manyToOne.ReferencedEntity, iCodeGenerationServiceProvider);

                if (otherEntity != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntity, iCodeGenerationServiceProvider)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToOne, otherEntity, iCodeGenerationServiceProvider)
                )
                {
                    CodeTypeMember one = CodeGenerationService.BuildManyToOne(entityMetadata, otherEntity, manyToOne, iCodeGenerationServiceProvider);
                    if (one != null)
                    {
                        memberCollection.Add(one);
                    }
                }
            }

            return memberCollection;
        }

        private static CodeTypeMember BuildManyToOne(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , OneToManyRelationshipMetadata manyToOne
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToOne, otherEntityMetadata, iCodeGenerationServiceProvider);

            EntityRole? entityRole = otherEntityMetadata.LogicalName == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referencing) : new EntityRole?();

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(typeForRelationship, iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider));
            codeMemberProperty.GetStatements.Add(CodeGenerationService.BuildRelationshipGet("GetRelatedEntity", manyToOne, typeForRelationship, entityRole));

            AttributeMetadata attributeMetadata = ((IEnumerable<AttributeMetadata>)entityMetadata.Attributes).SingleOrDefault<AttributeMetadata>(attribute => attribute.LogicalName == manyToOne.ReferencingAttribute);

            if (attributeMetadata == null)
            {
                return null;
            }

            if (attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault())
            {
                codeMemberProperty.SetStatements.AddRange(CodeGenerationService.BuildRelationshipSet("SetRelatedEntity", manyToOne, typeForRelationship, codeMemberProperty.Name, entityRole));
            }

            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.Attribute(CodeGenerationService.AttributeLogicalNameAttribute, CodeGenerationService.AttributeArg(manyToOne.ReferencingAttribute)));
            codeMemberProperty.CustomAttributes.Add(CodeGenerationService.BuildRelationshipSchemaNameAttribute(manyToOne.SchemaName, entityRole));

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToOne(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeStatement BuildRelationshipGet(
            string methodName
            , RelationshipMetadataBase relationship
            , CodeTypeReference targetType
            , EntityRole? entityRole
        )
        {
            CodeExpression codeExpression = entityRole.HasValue ? CodeGenerationService.FieldRef(typeof(EntityRole), entityRole.ToString()) : (CodeExpression)CodeGenerationService.Null();
            return CodeGenerationService.Return(CodeGenerationService.ThisMethodInvoke(methodName, targetType, CodeGenerationService.StringLiteral(relationship.SchemaName), codeExpression));
        }

        private static CodeStatementCollection BuildRelationshipSet(
            string methodName
            , RelationshipMetadataBase relationship
            , CodeTypeReference targetType
            , string propertyName
            , EntityRole? entityRole
        )
        {
            CodeStatementCollection statementCollection = new CodeStatementCollection();

            CodeExpression codeExpression = entityRole.HasValue ? CodeGenerationService.FieldRef(typeof(EntityRole), entityRole.ToString()) : (CodeExpression)CodeGenerationService.Null();

            statementCollection.Add(CodeGenerationService.ThisMethodInvoke("OnPropertyChanging", (CodeExpression)CodeGenerationService.StringLiteral(propertyName)));
            statementCollection.Add(CodeGenerationService.ThisMethodInvoke(methodName, targetType, CodeGenerationService.StringLiteral(relationship.SchemaName), codeExpression, CodeGenerationService.VarRef("value")));
            statementCollection.Add(CodeGenerationService.ThisMethodInvoke("OnPropertyChanged", (CodeExpression)CodeGenerationService.StringLiteral(propertyName)));

            return statementCollection;
        }

        private static CodeAttributeDeclaration BuildRelationshipSchemaNameAttribute(
            string relationshipSchemaName
            , EntityRole? entityRole
        )
        {
            if (entityRole.HasValue)
            {
                return CodeGenerationService.Attribute(CodeGenerationService.RelationshipSchemaNameAttribute, CodeGenerationService.AttributeArg(relationshipSchemaName), CodeGenerationService.AttributeArg(CodeGenerationService.FieldRef(typeof(EntityRole), entityRole.ToString())));
            }

            return CodeGenerationService.Attribute(CodeGenerationService.RelationshipSchemaNameAttribute, CodeGenerationService.AttributeArg(relationshipSchemaName));
        }

        private static EntityMetadata GetEntityMetadata(
            string entityLogicalName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata(entityLogicalName);
        }

        private static CodeTypeDeclarationCollection BuildServiceContext(
            IEnumerable<EntityMetadata> entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateServiceContext(iCodeGenerationServiceProvider))
            {
                CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.Class(iCodeGenerationServiceProvider.NamingService.GetNameForServiceContext(iCodeGenerationServiceProvider), CodeGenerationService.ServiceContextBaseType);

                codeTypeDeclaration.Members.Add(CodeGenerationService.ServiceContextConstructor(iCodeGenerationServiceProvider));

                codeTypeDeclaration.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContext(iCodeGenerationServiceProvider)));

                foreach (EntityMetadata entityMetadata1 in entityMetadata.OrderBy<EntityMetadata, string>(metadata => metadata.LogicalName))
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1, iCodeGenerationServiceProvider) && !string.Equals(entityMetadata1.LogicalName, "calendarrule", StringComparison.Ordinal))
                    {
                        codeTypeDeclaration.Members.Add(CodeGenerationService.BuildEntitySet(entityMetadata1, iCodeGenerationServiceProvider));
                    }
                }

                declarationCollection.Add(codeTypeDeclaration);
            }

            return declarationCollection;
        }

        private static CodeTypeMember ServiceContextConstructor(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            CodeConstructor codeConstructor = CodeGenerationService.Constructor(CodeGenerationService.Param(CodeGenerationService.TypeRef(typeof(IOrganizationService)), "service"));

            codeConstructor.BaseConstructorArgs.Add(CodeGenerationService.VarRef("service"));

            codeConstructor.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContextConstructor(iCodeGenerationServiceProvider)));

            return codeConstructor;
        }

        private static CodeTypeMember BuildEntitySet(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference typeForEntity = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entity, iCodeGenerationServiceProvider);

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(CodeGenerationService.IQueryable(typeForEntity), iCodeGenerationServiceProvider.NamingService.GetNameForEntitySet(entity, iCodeGenerationServiceProvider), (CodeStatement)CodeGenerationService.Return(CodeGenerationService.ThisMethodInvoke("CreateQuery", typeForEntity)));

            codeMemberProperty.Comments.AddRange(CodeGenerationService.CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntitySet(entity, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeTypeDeclarationCollection BuildMessages(
            IEnumerable<CodeGenerationSdkMessage> sdkMessages
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            foreach (CodeGenerationSdkMessage sdkMessage in sdkMessages)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessage(sdkMessage, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(CodeGenerationService.BuildMessage(sdkMessage, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private static CodeTypeDeclarationCollection BuildMessage(
            CodeGenerationSdkMessage message
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclarationCollection declarationCollection = new CodeTypeDeclarationCollection();

            foreach (CodeGenerationSdkMessagePair sdkMessagePair in message.SdkMessagePairs.Values)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessagePair(sdkMessagePair, iCodeGenerationServiceProvider))
                {
                    declarationCollection.Add(CodeGenerationService.BuildMessageRequest(sdkMessagePair, sdkMessagePair.Request, iCodeGenerationServiceProvider));
                    declarationCollection.Add(CodeGenerationService.BuildMessageResponse(sdkMessagePair, sdkMessagePair.Response, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private static CodeTypeDeclaration BuildMessageRequest(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageRequest sdkMessageRequest
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclaration requestClass = CodeGenerationService.Class(string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), RequestClassSuffix), CodeGenerationService.RequestClassBaseType, CodeGenerationService.Attribute(typeof(DataContractAttribute), CodeGenerationService.AttributeArg("Namespace", messagePair.MessageNamespace)), CodeGenerationService.Attribute(typeof(RequestProxyAttribute), CodeGenerationService.AttributeArg(null, messagePair.Request.Name)));

            bool flag = false;

            CodeStatementCollection statementCollection = new CodeStatementCollection();

            if (sdkMessageRequest.RequestFields != null & sdkMessageRequest.RequestFields.Count > 0)
            {
                foreach (Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field in sdkMessageRequest.RequestFields.Values)
                {
                    CodeMemberProperty requestField = CodeGenerationService.BuildRequestField(sdkMessageRequest, field, iCodeGenerationServiceProvider);
                    if (requestField.Type.Options == CodeTypeReferenceOptions.GenericTypeParameter)
                    {
                        flag = true;
                        CodeGenerationService.ConvertRequestToGeneric(messagePair, requestClass, requestField);
                    }
                    requestClass.Members.Add(requestField);
                    if (!field.Optional.GetValueOrDefault())
                    {
                        statementCollection.Add(CodeGenerationService.AssignProp(requestField.Name, new CodeDefaultValueExpression(requestField.Type)));
                    }
                }
            }

            if (!flag)
            {
                CodeConstructor codeConstructor = CodeGenerationService.Constructor();
                codeConstructor.Statements.Add(CodeGenerationService.AssignProp(CodeGenerationService.RequestNamePropertyName, new CodePrimitiveExpression(messagePair.Request.Name)));
                codeConstructor.Statements.AddRange(statementCollection);
                requestClass.Members.Add(codeConstructor);
            }

            return requestClass;
        }

        private static void ConvertRequestToGeneric(
            CodeGenerationSdkMessagePair messagePair
            , CodeTypeDeclaration requestClass
            , CodeMemberProperty requestField
        )
        {
            requestClass.TypeParameters.Add(new CodeTypeParameter(requestField.Type.BaseType)
            {
                HasConstructorConstraint = true,
                Constraints =
                {
                    new CodeTypeReference(CodeGenerationService.EntityClassBaseType),
                },
            });

            requestClass.Members.Add(CodeGenerationService.Constructor((CodeExpression)CodeGenerationService.New(requestField.Type)));

            CodeConstructor codeConstructor = CodeGenerationService.Constructor(CodeGenerationService.Param(requestField.Type, "target"), (CodeStatement)CodeGenerationService.AssignProp(requestField.Name, CodeGenerationService.VarRef("target")));
            codeConstructor.Statements.Add(CodeGenerationService.AssignProp(CodeGenerationService.RequestNamePropertyName, new CodePrimitiveExpression(messagePair.Request.Name)));

            requestClass.Members.Add(codeConstructor);
        }

        private static CodeTypeDeclaration BuildMessageResponse(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageResponse sdkMessageResponse
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeDeclaration codeTypeDeclaration = CodeGenerationService.Class(string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), ResponseClassSuffix), CodeGenerationService.ResponseClassBaseType, CodeGenerationService.Attribute(typeof(DataContractAttribute), CodeGenerationService.AttributeArg("Namespace", messagePair.MessageNamespace)), CodeGenerationService.Attribute(typeof(ResponseProxyAttribute), CodeGenerationService.AttributeArg(null, messagePair.Request.Name)));
            codeTypeDeclaration.Members.Add(CodeGenerationService.Constructor());
            if (sdkMessageResponse != null && sdkMessageResponse.ResponseFields != null & sdkMessageResponse.ResponseFields.Count > 0)
            {
                foreach (Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField field in sdkMessageResponse.ResponseFields.Values)
                {
                    codeTypeDeclaration.Members.Add(CodeGenerationService.BuildResponseField(sdkMessageResponse, field, iCodeGenerationServiceProvider));
                }
            }

            return codeTypeDeclaration;
        }

        private static CodeMemberProperty BuildRequestField(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference typeForRequestField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRequestField(request, field, iCodeGenerationServiceProvider);

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(typeForRequestField, iCodeGenerationServiceProvider.NamingService.GetNameForRequestField(request, field, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = true;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(CodeGenerationService.BuildRequestFieldGetStatement(field, typeForRequestField));
            codeMemberProperty.SetStatements.Add(CodeGenerationService.BuildRequestFieldSetStatement(field));

            return codeMemberProperty;
        }

        private static CodeStatement BuildRequestFieldGetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
            , CodeTypeReference targetType
        )
        {
            return CodeGenerationService.If(CodeGenerationService.ContainsParameter(field.Name), CodeGenerationService.Return(CodeGenerationService.Cast(targetType, CodeGenerationService.PropertyIndexer(CodeGenerationService.ParametersPropertyName, field.Name))), CodeGenerationService.Return(new CodeDefaultValueExpression(targetType)));
        }

        private static CodeAssignStatement BuildRequestFieldSetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
        )
        {
            return CodeGenerationService.AssignValue(CodeGenerationService.PropertyIndexer(CodeGenerationService.ParametersPropertyName, field.Name));
        }

        private static CodeMemberProperty BuildResponseField(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference forResponseField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForResponseField(field, iCodeGenerationServiceProvider);

            CodeMemberProperty codeMemberProperty = CodeGenerationService.PropertyGet(forResponseField, iCodeGenerationServiceProvider.NamingService.GetNameForResponseField(response, field, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = false;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(CodeGenerationService.BuildResponseFieldGetStatement(field, forResponseField));

            return codeMemberProperty;
        }

        private static CodeStatement BuildResponseFieldGetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField field
            , CodeTypeReference targetType
        )
        {
            return CodeGenerationService.If(CodeGenerationService.ContainsResult(field.Name), CodeGenerationService.Return(CodeGenerationService.Cast(targetType, CodeGenerationService.PropertyIndexer(CodeGenerationService.ResultsPropertyName, field.Name))), CodeGenerationService.Return(new CodeDefaultValueExpression(targetType)));
        }

        private static CodeNamespace Namespace(string name)
        {
            return new CodeNamespace(name);
        }

        private static CodeTypeDeclaration Class(
            string name
            , Type baseType
            , params CodeAttributeDeclaration[] attrs
        )
        {
            return CodeGenerationService.Class(name, CodeGenerationService.TypeRef(baseType), attrs);
        }

        private static CodeTypeDeclaration Class(
            string name
            , CodeTypeReference baseType
            , params CodeAttributeDeclaration[] attrs
        )
        {
            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public,
            };

            codeTypeDeclaration.BaseTypes.Add(baseType);

            if (attrs != null)
            {
                codeTypeDeclaration.CustomAttributes.AddRange(attrs);
            }

            codeTypeDeclaration.IsPartial = true;

            //codeTypeDeclaration.CustomAttributes.Add(CodeGenerationService.Attribute(typeof(GeneratedCodeAttribute), CodeGenerationService.AttributeArg((object)CrmSvcUtil.ApplicationName), CodeGenerationService.AttributeArg((object)CrmSvcUtil.ApplicationVersion)));

            return codeTypeDeclaration;
        }

        private static CodeTypeDeclaration Enum(string name, params CodeAttributeDeclaration[] attrs)
        {
            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name)
            {
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public,
            };

            if (attrs != null)
            {
                codeTypeDeclaration.CustomAttributes.AddRange(attrs);
            }

            //codeTypeDeclaration.CustomAttributes.Add(CodeGenerationService.Attribute(typeof(GeneratedCodeAttribute), CodeGenerationService.AttributeArg((object)CrmSvcUtil.ApplicationName), CodeGenerationService.AttributeArg((object)CrmSvcUtil.ApplicationVersion)));

            return codeTypeDeclaration;
        }

        private static CodeTypeReference TypeRef(Type type)
        {
            return new CodeTypeReference(type);
        }

        private static CodeAttributeDeclaration Attribute(Type type)
        {
            return new CodeAttributeDeclaration(CodeGenerationService.TypeRef(type));
        }

        private static CodeAttributeDeclaration Attribute(Type type, params CodeAttributeArgument[] args)
        {
            return new CodeAttributeDeclaration(CodeGenerationService.TypeRef(type), args);
        }

        private static CodeAttributeArgument AttributeArg(object value
        )
        {
            CodeExpression codeExpression = value as CodeExpression;
            if (codeExpression != null)
            {
                return CodeGenerationService.AttributeArg(null, codeExpression);
            }

            return CodeGenerationService.AttributeArg(null, value);
        }

        private static CodeAttributeArgument AttributeArg(string name, object value)
        {
            return CodeGenerationService.AttributeArg(name, new CodePrimitiveExpression(value));
        }

        private static CodeAttributeArgument AttributeArg(string name, CodeExpression value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return new CodeAttributeArgument(name, value);
            }

            return new CodeAttributeArgument(value);
        }

        private static CodeMemberProperty PropertyGet(
            CodeTypeReference type
            , string name
            , params CodeStatement[] stmts
        )
        {
            CodeMemberProperty codeMemberProperty = new CodeMemberProperty
            {
                Type = type,
                Name = name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                HasGet = true,
                HasSet = false,
            };

            codeMemberProperty.GetStatements.AddRange(stmts);

            return codeMemberProperty;
        }

        private static CodeMemberEvent Event(
            string name
            , Type type
            , Type implementationType
        )
        {
            CodeMemberEvent codeMemberEvent = new CodeMemberEvent
            {
                Name = name,
                Type = CodeGenerationService.TypeRef(type),
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
            };

            if (implementationType != null)
            {
                codeMemberEvent.ImplementationTypes.Add(CodeGenerationService.TypeRef(implementationType));
            }

            return codeMemberEvent;
        }

        private static CodeMemberMethod RaiseEvent(
            string methodName
            , string eventName
            , Type eventArgsType
        )
        {
            CodeMemberMethod codeMemberMethod1 = new CodeMemberMethod
            {
                Name = methodName
            };

            CodeMemberMethod codeMemberMethod2 = codeMemberMethod1;
            codeMemberMethod2.Parameters.Add(CodeGenerationService.Param(CodeGenerationService.TypeRef(typeof(string)), "propertyName"));

            CodeEventReferenceExpression referenceExpression = new CodeEventReferenceExpression(CodeGenerationService.This(), eventName);
            codeMemberMethod2.Statements.Add(CodeGenerationService.If(CodeGenerationService.NotNull(referenceExpression), new CodeDelegateInvokeExpression(referenceExpression, new CodeExpression[2]
            {
                CodeGenerationService.This(),
                CodeGenerationService.New(CodeGenerationService.TypeRef(eventArgsType), (CodeExpression) CodeGenerationService.VarRef("propertyName"))
            })));

            return codeMemberMethod2;
        }

        private static CodeMethodInvokeExpression ContainsParameter(
            string parameterName
        )
        {
            return new CodeMethodInvokeExpression(CodeGenerationService.ThisProp(CodeGenerationService.ParametersPropertyName), "Contains", new CodeExpression[1]
            {
                CodeGenerationService.StringLiteral(parameterName)
            });
        }

        private static CodeMethodInvokeExpression ContainsResult(string resultName)
        {
            return new CodeMethodInvokeExpression(CodeGenerationService.ThisProp(CodeGenerationService.ResultsPropertyName), "Contains", new CodeExpression[1]
            {
                CodeGenerationService.StringLiteral(resultName)
            });
        }

        private static CodeConditionStatement If(CodeExpression condition, CodeExpression trueCode)
        {
            return CodeGenerationService.If(condition, new CodeExpressionStatement(trueCode), null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeExpression trueCode
            , CodeExpression falseCode
        )
        {
            return CodeGenerationService.If(condition, new CodeExpressionStatement(trueCode), new CodeExpressionStatement(falseCode));
        }

        private static CodeConditionStatement If(CodeExpression condition, CodeStatement trueStatement)
        {
            return CodeGenerationService.If(condition, trueStatement, null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeStatement trueStatement
            , CodeStatement falseStatement
        )
        {
            CodeConditionStatement conditionStatement = new CodeConditionStatement(condition, new CodeStatement[1]
            {
                trueStatement
            });

            if (falseStatement != null)
            {
                conditionStatement.FalseStatements.Add(falseStatement);
            }

            return conditionStatement;
        }

        private static CodeFieldReferenceExpression FieldRef(Type targetType, string fieldName)
        {
            return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(targetType), fieldName);
        }

        private static CodeMemberField Field(
            string name
            , Type type
            , object value
            , params CodeAttributeDeclaration[] attrs
        )
        {
            CodeMemberField codeMemberField = new CodeMemberField(type, name)
            {
                InitExpression = new CodePrimitiveExpression(value)
            };

            if (attrs != null)
            {
                codeMemberField.CustomAttributes.AddRange(attrs);
            }

            return codeMemberField;
        }

        private static CodeParameterDeclarationExpression Param(CodeTypeReference type, string name)
        {
            return new CodeParameterDeclarationExpression(type, name);
        }

        private static CodeTypeParameter TypeParam(string name, params Type[] constraints)
        {
            CodeTypeParameter codeTypeParameter = new CodeTypeParameter(name);
            if (constraints != null)
            {
                codeTypeParameter.Constraints.AddRange(((IEnumerable<Type>)constraints).Select<Type, CodeTypeReference>(new Func<Type, CodeTypeReference>(CodeGenerationService.TypeRef)).ToArray<CodeTypeReference>());
            }

            return codeTypeParameter;
        }

        private static CodeVariableReferenceExpression VarRef(string name)
        {
            return new CodeVariableReferenceExpression(name);
        }

        private static CodeVariableDeclarationStatement Var(
            Type type
            , string name
            , CodeExpression init
        )
        {
            return new CodeVariableDeclarationStatement(type, name, init);
        }

        private static CodeConstructor Constructor(params CodeExpression[] thisArgs)
        {
            CodeConstructor codeConstructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };

            if (thisArgs != null)
            {
                codeConstructor.ChainedConstructorArgs.AddRange(thisArgs);
            }

            return codeConstructor;
        }

        private static CodeConstructor Constructor(CodeParameterDeclarationExpression arg, params CodeStatement[] statements)
        {
            CodeConstructor codeConstructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };
            if (arg != null)
            {
                codeConstructor.Parameters.Add(arg);
            }

            if (statements != null)
            {
                codeConstructor.Statements.AddRange(statements);
            }

            return codeConstructor;
        }

        private static CodeObjectCreateExpression New(CodeTypeReference createType, params CodeExpression[] args)
        {
            return new CodeObjectCreateExpression(createType, args);
        }

        private static CodeAssignStatement AssignProp(string propName, CodeExpression value)
        {
            return new CodeAssignStatement()
            {
                Left = CodeGenerationService.ThisProp(propName),
                Right = value,
            };
        }

        private static CodeAssignStatement AssignValue(CodeExpression target)
        {
            return CodeGenerationService.AssignValue(target, new CodeVariableReferenceExpression("value"));
        }

        private static CodeAssignStatement AssignValue(CodeExpression target, CodeExpression value)
        {
            return new CodeAssignStatement(target, value);
        }

        private static CodePropertyReferenceExpression BaseProp(string propertyName)
        {
            return new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), propertyName);
        }

        private static CodeIndexerExpression PropertyIndexer(string propertyName, string index)
        {
            return new CodeIndexerExpression(CodeGenerationService.ThisProp(propertyName), new CodeExpression[1]
            {
                new CodePrimitiveExpression( index)
            });
        }

        private static CodePropertyReferenceExpression PropRef(CodeExpression expression, string propertyName)
        {
            return new CodePropertyReferenceExpression(expression, propertyName);
        }

        private static CodePropertyReferenceExpression ThisProp(string propertyName)
        {
            return new CodePropertyReferenceExpression(CodeGenerationService.This(), propertyName);
        }

        private static CodeThisReferenceExpression This()
        {
            return new CodeThisReferenceExpression();
        }

        private static CodeMethodInvokeExpression ThisMethodInvoke(string methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(CodeGenerationService.This(), methodName), parameters);
        }

        private static CodeMethodInvokeExpression ThisMethodInvoke(
            string methodName
            , CodeTypeReference typeParameter
            , params CodeExpression[] parameters
        )
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(CodeGenerationService.This(), methodName, new CodeTypeReference[1]
            {
                typeParameter
            }), parameters);
        }

        private static CodeMethodInvokeExpression StaticMethodInvoke(
            Type targetObject
            , string methodName
            , params CodeExpression[] parameters
        )
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(targetObject), methodName), parameters);
        }

        private static CodeMethodInvokeExpression StaticMethodInvoke(
            Type targetObject
            , string methodName
            , CodeTypeReference typeParameter
            , params CodeExpression[] parameters
        )
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(targetObject), methodName, new CodeTypeReference[1] { typeParameter }), parameters);
        }

        private static CodeCastExpression Cast(CodeTypeReference targetType, CodeExpression expression)
        {
            return new CodeCastExpression(targetType, expression);
        }

        private static CodeCommentStatementCollection CommentSummary(IEnumerable<string> comments)
        {
            if (!comments.Any())
            {
                return new CodeCommentStatementCollection();
            }

            var result = new CodeCommentStatementCollection()
            {
                new CodeCommentStatement("<summary>", true),
            };

            foreach (var comment in comments)
            {
                result.Add(new CodeCommentStatement(comment, true));
            }

            result.Add(new CodeCommentStatement("</summary>", true));

            return result;
        }

        //private static CodeCommentStatementCollection CommentSummary(string comment)
        //{
        //    if (string.IsNullOrEmpty(comment))
        //    {
        //        return new CodeCommentStatementCollection();
        //    }

        //    return new CodeCommentStatementCollection()
        //    {
        //        new CodeCommentStatement("<summary>", true),
        //        new CodeCommentStatement(comment, true),
        //        new CodeCommentStatement("</summary>", true)
        //    };
        //}

        private static CodePrimitiveExpression StringLiteral(string value)
        {
            return new CodePrimitiveExpression(value);
        }

        private static CodeMethodReturnStatement Return()
        {
            return new CodeMethodReturnStatement();
        }

        private static CodeMethodReturnStatement Return(
            CodeExpression expression
        )
        {
            return new CodeMethodReturnStatement(expression);
        }

        private static CodeMethodInvokeExpression ConvertEnum(CodeTypeReference type, string variableName)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(CodeGenerationService.TypeRef(typeof(Enum))), "ToObject", new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression( CodeGenerationService.VarRef(variableName), "Value")
            });
        }

        private static CodeExpression ValueNull()
        {
            return new CodeBinaryOperatorExpression(CodeGenerationService.VarRef("value"), CodeBinaryOperatorType.IdentityEquality, CodeGenerationService.Null());
        }

        private static CodePrimitiveExpression Null()
        {
            return new CodePrimitiveExpression(null);
        }

        private static CodeBinaryOperatorExpression NotNull(CodeExpression expression)
        {
            return new CodeBinaryOperatorExpression(expression, CodeBinaryOperatorType.IdentityInequality, CodeGenerationService.Null());
        }

        private static CodeExpression GuidEmpty()
        {
            return CodeGenerationService.PropRef(new CodeTypeReferenceExpression(typeof(Guid)), "Empty");
        }

        private static CodeExpression False()
        {
            return new CodePrimitiveExpression(false);
        }

        private static CodeTypeReference IEnumerable(CodeTypeReference typeParameter)
        {
            return new CodeTypeReference(typeof(IEnumerable<>).FullName, new CodeTypeReference[1]
            {
                typeParameter
            });
        }

        private static CodeTypeReference IQueryable(CodeTypeReference typeParameter)
        {
            return new CodeTypeReference(typeof(IQueryable<>).FullName, new CodeTypeReference[1]
            {
                typeParameter
            });
        }

        private static CodeThrowExceptionStatement ThrowArgumentNull(string paramName)
        {
            return new CodeThrowExceptionStatement(CodeGenerationService.New(CodeGenerationService.TypeRef(typeof(ArgumentNullException)), (CodeExpression)CodeGenerationService.StringLiteral(paramName)));
        }

        private static CodeBinaryOperatorExpression Or(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanOr, right);
        }

        private static CodeBinaryOperatorExpression Equal(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.IdentityEquality, right);
        }

        private static CodeBinaryOperatorExpression And(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanAnd, right);
        }
    }
}