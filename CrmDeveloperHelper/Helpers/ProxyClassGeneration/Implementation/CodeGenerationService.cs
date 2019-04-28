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
        private static readonly Type EntityLogicalNameAttribute = typeof(EntityLogicalNameAttribute);
        private static readonly Type AttributeLogicalNameAttribute = typeof(AttributeLogicalNameAttribute);
        private static readonly Type RelationshipSchemaNameAttribute = typeof(RelationshipSchemaNameAttribute);
        private static readonly Type DebuggerNonUserCodeAttribute = typeof(System.Diagnostics.DebuggerNonUserCodeAttribute);

        private static readonly Type ObsoleteFieldAttribute = typeof(ObsoleteAttribute);
        private static readonly Type ServiceContextBaseType = typeof(OrganizationServiceContext);

        private static readonly Type EntityClassBaseType = typeof(Entity);
        private static readonly Type RequestClassBaseType = typeof(OrganizationRequest);
        private static readonly Type ResponseClassBaseType = typeof(OrganizationResponse);

        private static readonly string RequestClassSuffix = "Request";
        private static readonly string ResponseClassSuffix = "Response";
        private static readonly string RequestNamePropertyName = "RequestName";
        private static readonly string ParametersPropertyName = "Parameters";
        private static readonly string ResultsPropertyName = "Results";

        private readonly CreateFileWithEntityMetadataCSharpConfiguration _config;

        public CodeGenerationService(CreateFileWithEntityMetadataCSharpConfiguration config)
        {
            this._config = config;
        }

        public Task WriteEntitiesFileAsync(
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

        public Task WriteEntityFileAsync(
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

        public Task WriteSdkMessageAsync(
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

        public void WriteEntitiesFile(
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
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildEntities(entities, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildOptionSets(optionSets, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildMessages(messages, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildServiceContext(entities, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            codeCompileUnit.AssemblyCustomAttributes.Add(Attribute(typeof(ProxyTypesAssemblyAttribute)));

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public void WriteEntityFile(
            EntityMetadata entityMetadata
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildEntity(entityMetadata, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public void WriteSdkMessage(
            CodeGenerationSdkMessage sdkMessage
            , string language
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildMessage(sdkMessage, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider(language))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public CodeGenerationType GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadataBase optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Enum;
        }

        public CodeGenerationType GetTypeForOption(
            OptionSetMetadataBase optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Field;
        }

        public CodeGenerationType GetTypeForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Class;
        }

        public CodeGenerationType GetTypeForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        public CodeGenerationType GetTypeForMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Class;
        }

        public CodeGenerationType GetTypeForRequestField(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        public CodeGenerationType GetTypeForResponseField(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        private CodeTypeDeclarationCollection BuildOptionSets(
            IEnumerable<OptionSetMetadataBase> optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var optionSetMetadataBase in optionSetMetadata)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(optionSetMetadataBase, iCodeGenerationServiceProvider)
                    && optionSetMetadataBase.IsGlobal.HasValue
                    && optionSetMetadataBase.IsGlobal.Value
                )
                {
                    var codeTypeDeclaration = this.BuildOptionSet(null, optionSetMetadataBase, iCodeGenerationServiceProvider);
                    if (codeTypeDeclaration != null)
                    {
                        declarationCollection.Add(codeTypeDeclaration);
                    }
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildOptionSet(
            EntityMetadata entity
            , OptionSetMetadataBase optionSet
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeTypeDeclaration = this.Enum(iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entity, optionSet, iCodeGenerationServiceProvider), Attribute(typeof(DataContractAttribute)));

            var optionSetMetadata = optionSet as OptionSetMetadata;
            if (optionSetMetadata == null)
            {
                return null;
            }

            foreach (var option in optionSetMetadata.Options)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOption(option, iCodeGenerationServiceProvider))
                {
                    codeTypeDeclaration.Members.Add(this.BuildOption(optionSet, option, iCodeGenerationServiceProvider));
                }
            }

            return codeTypeDeclaration;
        }

        private CodeTypeMember BuildOption(
            OptionSetMetadataBase optionSet
            , OptionMetadata option
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeMemberField = this.Field(iCodeGenerationServiceProvider.NamingService.GetNameForOption(optionSet, option, iCodeGenerationServiceProvider), typeof(int), option.Value.Value, Attribute(typeof(EnumMemberAttribute)));
            return codeMemberField;
        }

        private CodeTypeDeclarationCollection BuildEntities(
            IEnumerable<EntityMetadata> entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var entityMetadata1 in entityMetadata.OrderBy(metadata => metadata.LogicalName))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(this.BuildEntity(entityMetadata1, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            var entityClassName = iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider);

            var entityClass = this.Class(entityClassName, TypeRef(EntityClassBaseType)
                , Attribute(typeof(DataContractAttribute))
            );

            CodeExpression entityLogicalNameAttributeRef = FieldRef(entityClassName, "EntityLogicalName");

            entityClass.CustomAttributes.Add(Attribute(EntityLogicalNameAttribute, AttributeArg(entityLogicalNameAttributeRef)));

            entityClass.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntity(entityMetadata, iCodeGenerationServiceProvider)));

            this.InitializeEntityClass(entityClass, entityMetadata, iCodeGenerationServiceProvider);

            entityClass.Members.AddRange(this.BuildAttributeTypeMembers(declarationCollection, entityMetadata, iCodeGenerationServiceProvider));
            entityClass.Members.AddRange(this.BuildManyToOneRelationships(entityMetadata, iCodeGenerationServiceProvider));
            entityClass.Members.AddRange(this.BuildOneToManyRelationships(entityMetadata, iCodeGenerationServiceProvider));
            entityClass.Members.AddRange(this.BuildManyToManyRelationships(entityMetadata, iCodeGenerationServiceProvider));

            declarationCollection.Add(entityClass);

            return declarationCollection;
        }

        private CodeTypeMemberCollection BuildAttributeTypeMembers(CodeTypeDeclarationCollection declarationCollection, EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            var memberCollection = new CodeTypeMemberCollection();

            CodeExpression primatyIdAttributeRef = VarRef("EntityPrimaryIdAttribute");
            CodeExpression primatyNameAttributeRef = VarRef("EntityPrimaryNameAttribute");

            {
                CodeRegionDirective startPrimaryAttributes = null;

                if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryId.GetValueOrDefault()
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider)
                    )
                    {
                        var attributeMember = this.BuildIdProperty(entityMetadata, attributeMetadata, primatyIdAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startPrimaryAttributes == null)
                        {
                            startPrimaryAttributes = new CodeRegionDirective(CodeRegionMode.Start, "Primary Attributes");
                            attributeMember.StartDirectives.Add(startPrimaryAttributes);
                        }
                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));

                        attributeMember = this.BuildAttributeProperty(entityMetadata, attributeMetadata, primatyIdAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startPrimaryAttributes == null)
                        {
                            startPrimaryAttributes = new CodeRegionDirective(CodeRegionMode.Start, "Primary Attributes");
                            attributeMember.StartDirectives.Add(startPrimaryAttributes);
                        }
                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                    }
                }

                if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryName.GetValueOrDefault()
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider)
                    )
                    {
                        var attributeMember = this.BuildAttributeProperty(entityMetadata, attributeMetadata, primatyNameAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startPrimaryAttributes == null)
                        {
                            startPrimaryAttributes = new CodeRegionDirective(CodeRegionMode.Start, "Primary Attributes");
                            attributeMember.StartDirectives.Add(startPrimaryAttributes);
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                    }
                }

                if (startPrimaryAttributes != null && memberCollection.Count > 0)
                {
                    memberCollection.OfType<CodeTypeMember>().Last().EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Primary Attributes"));
                }
            }

            {
                CodeRegionDirective startAttributes = null;

                foreach (var attributeMetadata in entityMetadata.Attributes.OrderBy(metadata => metadata.LogicalName))
                {
                    CodeMemberProperty attributeMember = null;

                    CodeExpression attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributeMetadata.LogicalName, iCodeGenerationServiceProvider);

                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider)
                        && !string.Equals(entityMetadata.PrimaryIdAttribute, attributeMetadata.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                        && !string.Equals(entityMetadata.PrimaryNameAttribute, attributeMetadata.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        attributeMember = this.BuildAttributeProperty(entityMetadata, attributeMetadata, attributeNameRef, iCodeGenerationServiceProvider);
                    }

                    var codeTypeDeclaration = this.BuildAttributeOptionSet(entityMetadata, attributeMetadata, attributeMember, attributeNameRef, iCodeGenerationServiceProvider);
                    if (codeTypeDeclaration != null)
                    {
                        declarationCollection.Add(codeTypeDeclaration);
                    }

                    if (attributeMember != null)
                    {
                        if (startAttributes == null)
                        {
                            startAttributes = new CodeRegionDirective(CodeRegionMode.Start, "Attributes");
                            attributeMember.StartDirectives.Add(startAttributes);
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                    }
                }

                if (startAttributes != null)
                {
                    memberCollection.OfType<CodeTypeMember>().Last().EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Attributes"));
                }
            }

            return memberCollection;
        }

        private void InitializeEntityClass(
            CodeTypeDeclaration entityClass
            , EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanging)));
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanged)));

            entityClass.Members.Add(this.EntityMetadataConstant("EntityLogicalName", typeof(string), entityMetadata.LogicalName));

            entityClass.Members.Add(this.EntityMetadataConstant("EntitySchemaName", typeof(string), entityMetadata.SchemaName));

            if (entityMetadata.ObjectTypeCode.HasValue && !entityMetadata.IsCustomEntity.GetValueOrDefault())
            {
                entityClass.Members.Add(this.EntityMetadataConstant("EntityTypeCode", typeof(int), entityMetadata.ObjectTypeCode));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
            {
                entityClass.Members.Add(this.EntityMetadataConstant("EntityPrimaryIdAttribute", typeof(string), entityMetadata.PrimaryIdAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
            {
                entityClass.Members.Add(this.EntityMetadataConstant("EntityPrimaryNameAttribute", typeof(string), entityMetadata.PrimaryNameAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryImageAttribute))
            {
                entityClass.Members.Add(this.EntityMetadataConstant("EntityPrimaryImageAttribute", typeof(string), entityMetadata.PrimaryImageAttribute));
            }

            entityClass.Members.Add(this.EntityConstructorDefault(entityMetadata, iCodeGenerationServiceProvider));

            if (_config.AddConstructorWithAnonymousTypeObject)
            {
                entityClass.Members.Add(this.EntityConstructorAnonymousObject(entityMetadata, iCodeGenerationServiceProvider));
            }

            entityClass.Members.Add(this.Event("PropertyChanged", typeof(PropertyChangedEventHandler), typeof(INotifyPropertyChanged), new CodeRegionDirective(CodeRegionMode.Start, "NotifyProperty Events")));
            entityClass.Members.Add(this.Event("PropertyChanging", typeof(PropertyChangingEventHandler), typeof(INotifyPropertyChanging)));
            entityClass.Members.Add(this.RaiseEvent("OnPropertyChanged", "PropertyChanged", typeof(PropertyChangedEventArgs)));
            entityClass.Members.Add(this.RaiseEvent("OnPropertyChanging", "PropertyChanging", typeof(PropertyChangingEventArgs), new CodeRegionDirective(CodeRegionMode.End, "NotifyProperty Events")));
        }

        private CodeTypeMember EntityMetadataConstant(string constName, Type type, object value)
        {
            var codeMemberField = this.Field(constName, type, value);
            codeMemberField.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            return codeMemberField;
        }

        private CodeTypeMember EntityConstructorDefault(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeConstructor = this.Constructor();
            codeConstructor.BaseConstructorArgs.Add(VarRef("EntityLogicalName"));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntityDefaultConstructor(entityMetadata, iCodeGenerationServiceProvider)));

            return codeConstructor;
        }

        private CodeTypeMember EntityConstructorAnonymousObject(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeConstructor = this.Constructor();

            codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Object), "anonymousObject"));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntityAnonymousConstructor(entityMetadata, iCodeGenerationServiceProvider)));

            codeConstructor.ChainedConstructorArgs.Add(new CodeSnippetExpression(string.Empty));

            codeConstructor.Statements.Add(new CodeSnippetStatement(string.Format(
@"{0}{0}{0}if (anonymousObject == null)
{0}{0}{0}{{
{0}{0}{0}{0}return;
{0}{0}{0}}}

{0}{0}{0}System.Type anonymousObjectType = anonymousObject.GetType();

{0}{0}{0}if (!anonymousObjectType.Name.StartsWith(""<>"")
{0}{0}{0}{0}|| anonymousObjectType.Name.IndexOf(""AnonymousType"", System.StringComparison.InvariantCultureIgnoreCase) == -1
{0}{0}{0})
{0}{0}{0}{{
{0}{0}{0}{0}return;
{0}{0}{0}}}

{0}{0}{0}foreach (var prop in anonymousObjectType.GetProperties())
{0}{0}{0}{{
{0}{0}{0}{0}var value = prop.GetValue(anonymousObject, null);
{0}{0}{0}{0}var name = prop.Name.ToLower();

{0}{0}{0}{0}switch (name)
{0}{0}{0}{0}{{
{0}{0}{0}{0}{0}case ""id"":
{0}{0}{0}{0}{0}case EntityPrimaryIdAttribute:
{0}{0}{0}{0}{0}{0}if (value is System.Guid idValue)
{0}{0}{0}{0}{0}{0}{{
{0}{0}{0}{0}{0}{0}{0}Attributes[EntityPrimaryIdAttribute] = base.Id = idValue;
{0}{0}{0}{0}{0}{0}}}
{0}{0}{0}{0}{0}{0}break;

{0}{0}{0}{0}{0}default:
{0}{0}{0}{0}{0}{0}if (value is Microsoft.Xrm.Sdk.FormattedValueCollection formattedValueCollection)
{0}{0}{0}{0}{0}{0}{{
{0}{0}{0}{0}{0}{0}{0}FormattedValues.AddRange(formattedValueCollection);
{0}{0}{0}{0}{0}{0}}}
{0}{0}{0}{0}{0}{0}else
{0}{0}{0}{0}{0}{0}{{
{0}{0}{0}{0}{0}{0}{0}Attributes[name] = value;
{0}{0}{0}{0}{0}{0}}}
{0}{0}{0}{0}{0}{0}break;
{0}{0}{0}{0}}}
{0}{0}{0}}}", _config.TabSpacer)));

            return codeConstructor;
        }

        private CodeMemberProperty BuildAttributeProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , CodeExpression attributeNameRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var forAttributeType = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForAttributeType(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(forAttributeType, iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable || attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attributeMetadata.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;

            if (codeMemberProperty.HasGet)
            {
                codeMemberProperty.GetStatements.AddRange(this.BuildAttributeGet(attributeMetadata, forAttributeType, attributeNameRef));
            }

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildAttributeSet(entityMetadata, attributeMetadata, codeMemberProperty.Name, attributeNameRef));
            }

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            if (!string.IsNullOrEmpty(attributeMetadata.DeprecatedVersion) && !_config.WithoutObsoleteAttribute)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(ObsoleteFieldAttribute));
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeTypeMember InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(CodeMemberProperty codeMemberProperty)
        {
            if (codeMemberProperty == null)
            {
                return null;
            }

            if (!this._config.GenerateWithDebuggerNonUserCode)
            {
                return codeMemberProperty;
            }

            using (var sourceWriter = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    var options = new CodeGeneratorOptions()
                    {
                        BracingStyle = "C",
                        IndentString = _config.TabSpacer,
                    };

                    return new CodeSnippetTypeMember(GetPropertyTextWithGetSetLevelDebuggerNonUserCodeAttribute(provider, options, sourceWriter, codeMemberProperty));
                }
            }
        }

        private string GetPropertyTextWithGetSetLevelDebuggerNonUserCodeAttribute(CodeDomProvider provider, CodeGeneratorOptions options, StringWriter sourceWriter, CodeMemberProperty property)
        {
            provider.GenerateCodeFromMember(property, sourceWriter, options);

            var code = sourceWriter.ToString();

            sourceWriter.GetStringBuilder().Clear();

            var lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            lines.RemoveAt(0);

            lines.RemoveAt(lines.Count - 1);

            for (var i = lines.Count() - 1; i >= 0; i--)
            {
                var line = lines[i];

                lines[i] = _config.TabSpacer + _config.TabSpacer + line;

                if (line.TrimStart() == "get" || line.TrimStart() == "set")
                {
                    lines.Insert(i, _config.TabSpacer + _config.TabSpacer + _config.TabSpacer + $"[{DebuggerNonUserCodeAttribute.FullName}()]");
                }
            }

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private CodeStatementCollection BuildAttributeGet(
            AttributeMetadata attributeMetadata
            , CodeTypeReference targetType
            , CodeExpression attributeNameRef
        )
        {
            var statementCollection = new CodeStatementCollection();

            if (attributeMetadata.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList
                && targetType.TypeArguments.Count > 0
            )
            {
                statementCollection.AddRange(BuildEntityCollectionAttributeGet(attributeNameRef, targetType));
            }
            else
            {
                statementCollection.Add(Return(ThisMethodInvoke("GetAttributeValue", targetType, attributeNameRef)));
            }

            return statementCollection;
        }

        private CodeStatementCollection BuildAttributeSet(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , string propertyName
            , CodeExpression attributeNameRef
        )
        {
            var statementCollection = new CodeStatementCollection
            {
                this.InvokeOnPropertyChanging(propertyName)
            };

            if (attributeMetadata.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList)
            {
                statementCollection.Add(BuildEntityCollectionAttributeSet(attributeNameRef));
            }
            else
            {
                statementCollection.Add(ThisMethodInvoke("SetAttributeValue", attributeNameRef, VarRef("value")));
            }

            if (entityMetadata.PrimaryIdAttribute == attributeMetadata.LogicalName && attributeMetadata.IsPrimaryId.GetValueOrDefault())
            {
                statementCollection.Add(If(PropRef(VarRef("value"), "HasValue"), AssignValue(BaseProp("Id"), PropRef(VarRef("value"), "Value")), AssignValue(BaseProp("Id"), GuidEmpty())));
            }

            statementCollection.Add(this.InvokeOnPropertyChanged(propertyName));

            return statementCollection;
        }

        private CodeExpression InvokeOnPropertyChanging(string propertyName)
        {
            return this.InvokeMethodOnPropertyChange("OnPropertyChanging", propertyName);
        }

        private CodeExpression InvokeOnPropertyChanged(string propertyName)
        {
            return this.InvokeMethodOnPropertyChange("OnPropertyChanged", propertyName);
        }

        private CodeExpression InvokeMethodOnPropertyChange(string methodName, string propertyName)
        {
            if (this._config.GenerateAttributesWithNameOf)
            {
                return ThisMethodInvoke(methodName, new CodeSnippetExpression($"nameof({propertyName})"));
            }
            else
            {
                return ThisMethodInvoke(methodName, (CodeExpression)StringLiteral(propertyName));
            }
        }

        private static CodeStatementCollection BuildEntityCollectionAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference propertyType
        )
        {
            return new CodeStatementCollection()
            {
                Var(typeof (EntityCollection), "collection",  ThisMethodInvoke("GetAttributeValue", TypeRef(typeof (EntityCollection)), attributeNameRef)),
                If(And(NotNull(VarRef("collection")), NotNull(PropRef(VarRef("collection"), "Entities"))), Return( StaticMethodInvoke(typeof (Enumerable), "Cast", propertyType.TypeArguments[0], (CodeExpression)PropRef(VarRef("collection"), "Entities"))), Return(Null()))
            };
        }

        private static CodeStatement BuildEntityCollectionAttributeSet(CodeExpression attributeNameRef)
        {
            return If(ValueNull(), ThisMethodInvoke("SetAttributeValue", attributeNameRef, VarRef("value")), ThisMethodInvoke("SetAttributeValue", attributeNameRef, New(TypeRef(typeof(EntityCollection)), (CodeExpression)New(TypeRef(typeof(List<Entity>)), (CodeExpression)VarRef("value")))));
        }

        private CodeMemberProperty BuildIdProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , CodeExpression attributeNameRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeMemberProperty = this.PropertyGet(TypeRef(typeof(Guid)), "Id");

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Attributes = MemberAttributes.Public | MemberAttributes.Override;

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable || attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attributeMetadata.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;

            codeMemberProperty.GetStatements.Add(Return(BaseProp("Id")));

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.Add(AssignValue(ThisProp(iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)), VarRef("value")));
            }
            else
            {
                codeMemberProperty.SetStatements.Add(AssignValue(BaseProp("Id"), VarRef("value")));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeTypeDeclaration BuildAttributeOptionSet(
            EntityMetadata entity
            , AttributeMetadata attribute
            , CodeMemberProperty attributeMember
            , CodeExpression attributeNameRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var attributeOptionSet = TypeMappingService.GetAttributeOptionSet(attribute);

            if (attributeOptionSet == null
                || !iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(attributeOptionSet, iCodeGenerationServiceProvider)
            )
            {
                return null;
            }

            var codeTypeDeclaration = this.BuildOptionSet(entity, attributeOptionSet, iCodeGenerationServiceProvider);
            if (codeTypeDeclaration == null)
            {
                return null;
            }

            this.UpdateAttributeMemberStatements(attributeNameRef, attributeMember);

            return codeTypeDeclaration;
        }

        private void UpdateAttributeMemberStatements(
            CodeExpression attributeNameRef
            , CodeMemberProperty attributeMember
        )
        {
            if (attributeMember.HasGet)
            {
                attributeMember.GetStatements.Clear();
                attributeMember.GetStatements.AddRange(BuildOptionSetAttributeGet(attributeNameRef, attributeMember.Type));
            }

            if (!attributeMember.HasSet)
            {
                return;
            }

            attributeMember.SetStatements.Clear();
            attributeMember.SetStatements.AddRange(this.BuildOptionSetAttributeSet(attributeNameRef, attributeMember.Name));
        }

        private static CodeStatementCollection BuildOptionSetAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference attributeType
        )
        {
            var codeTypeReference = attributeType;
            if (codeTypeReference.TypeArguments.Count > 0)
            {
                codeTypeReference = codeTypeReference.TypeArguments[0];
            }

            return new CodeStatementCollection(new CodeStatement[2]
            {
                Var(typeof (OptionSetValue), "optionSet", ThisMethodInvoke("GetAttributeValue", TypeRef(typeof (OptionSetValue)), attributeNameRef)),
                If(NotNull(VarRef("optionSet")), Return(Cast(codeTypeReference, ConvertEnum(codeTypeReference, "optionSet"))), Return( Null()))
            });
        }

        private CodeStatementCollection BuildOptionSetAttributeSet(
            CodeExpression attributeNameRef
            , string propertyName
        )
        {
            return new CodeStatementCollection()
            {
                this.InvokeOnPropertyChanging(propertyName),
                If(ValueNull(), ThisMethodInvoke("SetAttributeValue", attributeNameRef, Null()), ThisMethodInvoke("SetAttributeValue", attributeNameRef, New(TypeRef(typeof(OptionSetValue)), (CodeExpression)Cast(TypeRef(typeof(int)), VarRef("value"))))),
                this.InvokeOnPropertyChanged(propertyName),
            };
        }

        private CodeTypeMemberCollection BuildOneToManyRelationships(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var memberCollection = new CodeTypeMemberCollection();
            if (entity.OneToManyRelationships == null)
            {
                return memberCollection;
            }

            const string regionName = "OneToMany Relationships";

            CodeRegionDirective startAttributes = null;

            foreach (var oneToMany in entity.OneToManyRelationships.OfType<OneToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                var entityMetadata = GetEntityMetadata(oneToMany.ReferencingEntity, iCodeGenerationServiceProvider);
                if (entityMetadata != null)
                {
                    CodeMemberProperty codeTypeMember = null;

                    if (string.Equals(oneToMany.SchemaName, "calendar_calendar_rules", StringComparison.Ordinal)
                        || string.Equals(oneToMany.SchemaName, "service_calendar_rules", StringComparison.Ordinal)
                    )
                    {
                        codeTypeMember = this.BuildCalendarRuleProperty(entity, entityMetadata, oneToMany, iCodeGenerationServiceProvider);
                    }
                    else if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata, iCodeGenerationServiceProvider) && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(oneToMany, entityMetadata, iCodeGenerationServiceProvider))
                    {
                        codeTypeMember = this.BuildOneToManyProperty(entity, entityMetadata, oneToMany, iCodeGenerationServiceProvider);
                    }

                    if (codeTypeMember != null)
                    {
                        if (startAttributes == null)
                        {
                            startAttributes = new CodeRegionDirective(CodeRegionMode.Start, regionName);
                            codeTypeMember.StartDirectives.Add(startAttributes);
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeTypeMember));
                    }
                }
            }

            if (startAttributes != null)
            {
                memberCollection.OfType<CodeTypeMember>().Last().EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, regionName));
            }

            return memberCollection;
        }

        private CodeMemberProperty BuildCalendarRuleProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntity
            , OneToManyRelationshipMetadata oneToMany
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeMemberProperty = this.PropertyGet(IEnumerable(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntity, iCodeGenerationServiceProvider)), "CalendarRules");

            var nullable = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var attributeNameRef = this.BuildAttributeNameRef(entityMetadata, "calendarrules", iCodeGenerationServiceProvider);

            codeMemberProperty.GetStatements.AddRange(BuildEntityCollectionAttributeGet(attributeNameRef, codeMemberProperty.Type));

            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanging(codeMemberProperty.Name));
            codeMemberProperty.SetStatements.Add(BuildEntityCollectionAttributeSet(attributeNameRef));
            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanged(codeMemberProperty.Name));

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, nullable, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeMemberProperty BuildOneToManyProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , OneToManyRelationshipMetadata oneToMany
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntityMetadata, iCodeGenerationServiceProvider);

            var entityRole = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var codeMemberProperty = this.PropertyGet(IEnumerable(typeForRelationship), iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, oneToMany, entityRole, iCodeGenerationServiceProvider));

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "OneToMany", oneToMany.SchemaName, iCodeGenerationServiceProvider);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntities", relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntities", relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeTypeMemberCollection BuildManyToManyRelationships(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var memberCollection = new CodeTypeMemberCollection();

            if (entity.ManyToManyRelationships == null)
            {
                return memberCollection;
            }

            const string regionName = "ManyToMany Relationships";

            CodeRegionDirective startAttributes = null;

            foreach (var manyToMany in entity.ManyToManyRelationships.OfType<ManyToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                var entityMetadata = GetEntityMetadata(entity.LogicalName != manyToMany.Entity1LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName, iCodeGenerationServiceProvider);
                if (entityMetadata != null)
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata, iCodeGenerationServiceProvider)
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToMany, entityMetadata, iCodeGenerationServiceProvider)
                    )
                    {
                        if (entityMetadata.LogicalName != entity.LogicalName)
                        {
                            var nameForRelationship = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(), iCodeGenerationServiceProvider);
                            var many = this.BuildManyToManyProperty(entity, entityMetadata, manyToMany, nameForRelationship, new EntityRole?(), iCodeGenerationServiceProvider);

                            if (many != null)
                            {
                                if (startAttributes == null)
                                {
                                    startAttributes = new CodeRegionDirective(CodeRegionMode.Start, regionName);
                                    many.StartDirectives.Add(startAttributes);
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many));
                            }
                        }
                        else
                        {
                            var nameForRelationship1 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);
                            var many1 = this.BuildManyToManyProperty(entity, entityMetadata, manyToMany, nameForRelationship1, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);

                            if (many1 != null)
                            {
                                if (startAttributes == null)
                                {
                                    startAttributes = new CodeRegionDirective(CodeRegionMode.Start, regionName);
                                    many1.StartDirectives.Add(startAttributes);
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many1));
                            }

                            var nameForRelationship2 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entity, manyToMany, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);
                            var many2 = this.BuildManyToManyProperty(entity, entityMetadata, manyToMany, nameForRelationship2, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);

                            if (many2 != null)
                            {
                                if (startAttributes == null)
                                {
                                    startAttributes = new CodeRegionDirective(CodeRegionMode.Start, regionName);
                                    many2.StartDirectives.Add(startAttributes);
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many2));
                            }
                        }
                    }
                }
            }

            if (startAttributes != null)
            {
                memberCollection.OfType<CodeTypeMember>().Last().EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, regionName));
            }

            return memberCollection;
        }

        private CodeMemberProperty BuildManyToManyProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , ManyToManyRelationshipMetadata manyToMany
            , string propertyName
            , EntityRole? entityRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToMany, otherEntityMetadata, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(IEnumerable(typeForRelationship), propertyName);

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "ManyToMany", manyToMany.SchemaName, iCodeGenerationServiceProvider);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntities", relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntities", relationshipNameRef, typeForRelationship, propertyName, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToMany(entityMetadata, manyToMany, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeTypeMemberCollection BuildManyToOneRelationships(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var memberCollection = new CodeTypeMemberCollection();

            if (entityMetadata.ManyToOneRelationships == null)
            {
                return memberCollection;
            }

            const string regionName = "ManyToOne Relationships";

            CodeRegionDirective startAttributes = null;

            foreach (var manyToOne in entityMetadata.ManyToOneRelationships.OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntity = GetEntityMetadata(manyToOne.ReferencedEntity, iCodeGenerationServiceProvider);

                if (otherEntity != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntity, iCodeGenerationServiceProvider)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToOne, otherEntity, iCodeGenerationServiceProvider)
                )
                {
                    var one = this.BuildManyToOneProperty(entityMetadata, otherEntity, manyToOne, iCodeGenerationServiceProvider);
                    if (one != null)
                    {
                        if (startAttributes == null)
                        {
                            startAttributes = new CodeRegionDirective(CodeRegionMode.Start, regionName);
                            one.StartDirectives.Add(startAttributes);
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(one));
                    }
                }
            }

            if (startAttributes != null)
            {
                memberCollection.OfType<CodeTypeMember>().Last().EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, regionName));
            }

            return memberCollection;
        }

        private CodeMemberProperty BuildManyToOneProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , OneToManyRelationshipMetadata manyToOne
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var attributeMetadata = entityMetadata.Attributes.SingleOrDefault(attribute => attribute.LogicalName == manyToOne.ReferencingAttribute);

            if (attributeMetadata == null)
            {
                return null;
            }

            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToOne, otherEntityMetadata, iCodeGenerationServiceProvider);

            var entityRole = otherEntityMetadata.LogicalName == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referencing) : new EntityRole?();

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "ManyToOne", manyToOne.SchemaName, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(typeForRelationship, iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider));
            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntity", relationshipNameRef, typeForRelationship, entityRole));

            if (_config.MakeAllPropertiesEditable
                || attributeMetadata.IsValidForCreate.GetValueOrDefault()
                || attributeMetadata.IsValidForUpdate.GetValueOrDefault()
            )
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntity", relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));
            }

            var attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributeMetadata.LogicalName, iCodeGenerationServiceProvider);

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));
            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToOne(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private static CodeStatement BuildRelationshipGet(
            string methodName
            , CodeExpression relationshipNameRef
            , CodeTypeReference targetType
            , EntityRole? entityRole
        )
        {
            var codeExpression = entityRole.HasValue ? FieldRef(typeof(EntityRole), entityRole.ToString()) : (CodeExpression)Null();
            return Return(ThisMethodInvoke(methodName, targetType, relationshipNameRef, codeExpression));
        }

        private CodeStatementCollection BuildRelationshipSet(
            string methodName
            , CodeExpression relationshipNameRef
            , CodeTypeReference targetType
            , string propertyName
            , EntityRole? entityRole
        )
        {
            var statementCollection = new CodeStatementCollection();

            var codeExpression = entityRole.HasValue ? FieldRef(typeof(EntityRole), entityRole.ToString()) : (CodeExpression)Null();

            statementCollection.Add(this.InvokeOnPropertyChanging(propertyName));
            statementCollection.Add(ThisMethodInvoke(methodName, targetType, relationshipNameRef, codeExpression, VarRef("value")));
            statementCollection.Add(this.InvokeOnPropertyChanged(propertyName));

            return statementCollection;
        }

        private static CodeAttributeDeclaration BuildRelationshipSchemaNameAttribute(
            CodeExpression relationshipNameRef
            , EntityRole? entityRole
        )
        {
            if (entityRole.HasValue)
            {
                return Attribute(RelationshipSchemaNameAttribute, AttributeArg(relationshipNameRef), AttributeArg(FieldRef(typeof(EntityRole), entityRole.ToString())));
            }

            return Attribute(RelationshipSchemaNameAttribute, AttributeArg(relationshipNameRef));
        }

        private static EntityMetadata GetEntityMetadata(
            string entityLogicalName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata(entityLogicalName);
        }

        private CodeTypeDeclarationCollection BuildServiceContext(
            IEnumerable<EntityMetadata> entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateServiceContext(iCodeGenerationServiceProvider))
            {
                var codeTypeDeclaration = this.Class(iCodeGenerationServiceProvider.NamingService.GetNameForServiceContext(iCodeGenerationServiceProvider), ServiceContextBaseType);

                codeTypeDeclaration.Members.Add(this.ServiceContextConstructor(iCodeGenerationServiceProvider));

                codeTypeDeclaration.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContext(iCodeGenerationServiceProvider)));

                foreach (var entityMetadata1 in entityMetadata.OrderBy(metadata => metadata.LogicalName))
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1, iCodeGenerationServiceProvider)
                        && !string.Equals(entityMetadata1.LogicalName, "calendarrule", StringComparison.Ordinal)
                    )
                    {
                        var codeMemberProperty = this.BuildEntitySetProperty(entityMetadata1, iCodeGenerationServiceProvider);

                        codeTypeDeclaration.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeMemberProperty));
                    }
                }

                declarationCollection.Add(codeTypeDeclaration);
            }

            return declarationCollection;
        }

        private CodeTypeMember ServiceContextConstructor(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            var codeConstructor = this.Constructor(Param(TypeRef(typeof(IOrganizationService)), "service"));

            codeConstructor.BaseConstructorArgs.Add(VarRef("service"));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContextConstructor(iCodeGenerationServiceProvider)));

            return codeConstructor;
        }

        private CodeMemberProperty BuildEntitySetProperty(
            EntityMetadata entity
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForEntity = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entity, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(IQueryable(typeForEntity), iCodeGenerationServiceProvider.NamingService.GetNameForEntitySet(entity, iCodeGenerationServiceProvider), (CodeStatement)Return(ThisMethodInvoke("CreateQuery", typeForEntity)));

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntitySet(entity, iCodeGenerationServiceProvider)));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberProperty;
        }

        private CodeTypeDeclarationCollection BuildMessages(
            IEnumerable<CodeGenerationSdkMessage> sdkMessages
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessage in sdkMessages)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessage(sdkMessage, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(this.BuildMessage(sdkMessage, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildMessage(
            CodeGenerationSdkMessage message
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessagePair in message.SdkMessagePairs.Values)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessagePair(sdkMessagePair, iCodeGenerationServiceProvider))
                {
                    declarationCollection.Add(this.BuildMessageRequest(sdkMessagePair, sdkMessagePair.Request, iCodeGenerationServiceProvider));
                    declarationCollection.Add(this.BuildMessageResponse(sdkMessagePair, sdkMessagePair.Response, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildMessageRequest(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageRequest sdkMessageRequest
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var requestClass = this.Class(string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), RequestClassSuffix), RequestClassBaseType, Attribute(typeof(DataContractAttribute), AttributeArg("Namespace", messagePair.MessageNamespace)), Attribute(typeof(RequestProxyAttribute), AttributeArg(null, messagePair.Request.Name)));

            var flag = false;

            var statementCollection = new CodeStatementCollection();

            if (sdkMessageRequest.RequestFields != null & sdkMessageRequest.RequestFields.Count > 0)
            {
                foreach (var field in sdkMessageRequest.RequestFields.Values)
                {
                    var requestField = this.BuildRequestFieldProperty(sdkMessageRequest, field, iCodeGenerationServiceProvider);

                    if (requestField.Type.Options == CodeTypeReferenceOptions.GenericTypeParameter)
                    {
                        flag = true;
                        this.ConvertRequestToGeneric(messagePair, requestClass, requestField);
                    }

                    if (!field.Optional.GetValueOrDefault())
                    {
                        statementCollection.Add(AssignProp(requestField.Name, new CodeDefaultValueExpression(requestField.Type)));
                    }

                    requestClass.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(requestField));
                }
            }

            if (!flag)
            {
                var codeConstructor = this.Constructor();
                codeConstructor.Statements.Add(AssignProp(RequestNamePropertyName, new CodePrimitiveExpression(messagePair.Request.Name)));
                codeConstructor.Statements.AddRange(statementCollection);
                requestClass.Members.Add(codeConstructor);
            }

            return requestClass;
        }

        private void ConvertRequestToGeneric(
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
                    new CodeTypeReference(EntityClassBaseType),
                },
            });

            requestClass.Members.Add(this.Constructor((CodeExpression)New(requestField.Type)));

            var codeConstructor = this.Constructor(Param(requestField.Type, "target"), (CodeStatement)AssignProp(requestField.Name, VarRef("target")));
            codeConstructor.Statements.Add(AssignProp(RequestNamePropertyName, new CodePrimitiveExpression(messagePair.Request.Name)));

            requestClass.Members.Add(codeConstructor);
        }

        private CodeTypeDeclaration BuildMessageResponse(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageResponse sdkMessageResponse
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeTypeDeclaration = this.Class(string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), ResponseClassSuffix), ResponseClassBaseType, Attribute(typeof(DataContractAttribute), AttributeArg("Namespace", messagePair.MessageNamespace)), Attribute(typeof(ResponseProxyAttribute), AttributeArg(null, messagePair.Request.Name)));

            codeTypeDeclaration.Members.Add(this.Constructor());

            if (sdkMessageResponse != null && sdkMessageResponse.ResponseFields != null & sdkMessageResponse.ResponseFields.Count > 0)
            {
                foreach (var field in sdkMessageResponse.ResponseFields.Values)
                {
                    var codeMemberProperty = this.BuildResponseFieldProperty(sdkMessageResponse, field, iCodeGenerationServiceProvider);

                    codeTypeDeclaration.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeMemberProperty));
                }
            }

            return codeTypeDeclaration;
        }

        private CodeMemberProperty BuildRequestFieldProperty(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForRequestField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRequestField(request, field, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(typeForRequestField, iCodeGenerationServiceProvider.NamingService.GetNameForRequestField(request, field, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = true;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(BuildRequestFieldGetStatement(field, typeForRequestField));
            codeMemberProperty.SetStatements.Add(BuildRequestFieldSetStatement(field));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberProperty;
        }

        private static CodeStatement BuildRequestFieldGetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
            , CodeTypeReference targetType
        )
        {
            return If(ContainsParameter(field.Name), Return(Cast(targetType, PropertyIndexer(ParametersPropertyName, field.Name))), Return(new CodeDefaultValueExpression(targetType)));
        }

        private static CodeAssignStatement BuildRequestFieldSetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField field
        )
        {
            return AssignValue(PropertyIndexer(ParametersPropertyName, field.Name));
        }

        private CodeMemberProperty BuildResponseFieldProperty(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var forResponseField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForResponseField(field, iCodeGenerationServiceProvider);

            var codeMemberProperty = this.PropertyGet(forResponseField, iCodeGenerationServiceProvider.NamingService.GetNameForResponseField(response, field, iCodeGenerationServiceProvider));

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(BuildResponseFieldGetStatement(field, forResponseField));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberProperty;
        }

        private static CodeStatement BuildResponseFieldGetStatement(
            Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField field
            , CodeTypeReference targetType
        )
        {
            return If(ContainsResult(field.Name), Return(Cast(targetType, PropertyIndexer(ResultsPropertyName, field.Name))), Return(new CodeDefaultValueExpression(targetType)));
        }

        private static CodeNamespace Namespace(string name)
        {
            return new CodeNamespace(name);
        }

        private CodeTypeDeclaration Class(
            string name
            , Type baseType
            , params CodeAttributeDeclaration[] attrs
        )
        {
            return this.Class(name, TypeRef(baseType), attrs);
        }

        private CodeTypeDeclaration Class(
            string name
            , CodeTypeReference baseType
            , params CodeAttributeDeclaration[] attrs
        )
        {
            var codeTypeDeclaration = new CodeTypeDeclaration(name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public,
            };

            codeTypeDeclaration.BaseTypes.Add(baseType);

            if (attrs != null)
            {
                codeTypeDeclaration.CustomAttributes.AddRange(attrs);
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeTypeDeclaration.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            //codeTypeDeclaration.CustomAttributes.Add(Attribute(typeof(GeneratedCodeAttribute), AttributeArg((object)CrmSvcUtil.ApplicationName), AttributeArg((object)CrmSvcUtil.ApplicationVersion)));

            return codeTypeDeclaration;
        }

        private CodeTypeDeclaration Enum(string name, params CodeAttributeDeclaration[] attrs)
        {
            var codeTypeDeclaration = new CodeTypeDeclaration(name)
            {
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public,
            };

            if (attrs != null)
            {
                codeTypeDeclaration.CustomAttributes.AddRange(attrs);
            }

            //codeTypeDeclaration.CustomAttributes.Add(Attribute(typeof(GeneratedCodeAttribute), AttributeArg((object)CrmSvcUtil.ApplicationName), AttributeArg((object)CrmSvcUtil.ApplicationVersion)));

            return codeTypeDeclaration;
        }

        private static CodeTypeReference TypeRef(Type type)
        {
            return new CodeTypeReference(type);
        }

        private static CodeAttributeDeclaration Attribute(Type type, params CodeAttributeArgument[] args)
        {
            return new CodeAttributeDeclaration(TypeRef(type), args);
        }

        private CodeExpression BuildAttributeNameRef(
            EntityMetadata entityMetadata
            , string attributeLogicalName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._config.UseSchemaConstInCSharpAttributes)
            {
                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata, iCodeGenerationServiceProvider);

                var className = typeRef.BaseType + ".Schema.Attributes";

                return FieldRef(className, attributeLogicalName);
            }

            return (CodeExpression)StringLiteral(attributeLogicalName);
        }

        private CodeExpression BuildRelationshipNameRef(
            EntityMetadata entityMetadata
            , string relationshipTypeName
            , string schemaName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._config.UseSchemaConstInCSharpAttributes)
            {
                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata, iCodeGenerationServiceProvider);

                var className = $"{typeRef.BaseType}.Schema.{relationshipTypeName}.{schemaName.ToLower()}";

                return FieldRef(className, "Name");
            }

            return (CodeExpression)StringLiteral(schemaName);
        }

        private static CodeAttributeArgument AttributeArg(object value)
        {
            var codeExpression = value as CodeExpression;
            if (codeExpression != null)
            {
                return AttributeArg(null, codeExpression);
            }

            return AttributeArg(null, value);
        }

        private static CodeAttributeArgument AttributeArg(string name, object value)
        {
            return AttributeArg(name, new CodePrimitiveExpression(value));
        }

        private static CodeAttributeArgument AttributeArg(string name, CodeExpression value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return new CodeAttributeArgument(name, value);
            }

            return new CodeAttributeArgument(value);
        }

        private CodeMemberProperty PropertyGet(
            CodeTypeReference type
            , string name
            , params CodeStatement[] stmts
        )
        {
            var codeMemberProperty = new CodeMemberProperty
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

        private CodeMemberEvent Event(
            string name
            , Type type
            , Type implementationType
            , CodeRegionDirective codeRegionDirective = null
        )
        {
            var codeMemberEvent = new CodeMemberEvent
            {
                Name = name,
                Type = TypeRef(type),
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
            };

            if (implementationType != null)
            {
                codeMemberEvent.ImplementationTypes.Add(TypeRef(implementationType));
            }

            if (codeRegionDirective != null)
            {
                if (codeRegionDirective.RegionMode == CodeRegionMode.Start)
                {
                    codeMemberEvent.StartDirectives.Add(codeRegionDirective);
                }
                else if (codeRegionDirective.RegionMode == CodeRegionMode.End)
                {
                    codeMemberEvent.EndDirectives.Add(codeRegionDirective);
                }
            }

            return codeMemberEvent;
        }

        private CodeMemberMethod RaiseEvent(
            string methodName
            , string eventName
            , Type eventArgsType
            , CodeRegionDirective codeRegionDirective = null
        )
        {
            var codeMemberMethod = new CodeMemberMethod
            {
                Name = methodName
            };

            codeMemberMethod.Parameters.Add(Param(TypeRef(typeof(string)), "propertyName"));

            var referenceExpression = new CodeEventReferenceExpression(This(), eventName);
            codeMemberMethod.Statements.Add(If(NotNull(referenceExpression), new CodeDelegateInvokeExpression(referenceExpression, new CodeExpression[2]
            {
                This(),
                New(TypeRef(eventArgsType), (CodeExpression) VarRef("propertyName"))
            })));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberMethod.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            if (codeRegionDirective != null)
            {
                if (codeRegionDirective.RegionMode == CodeRegionMode.Start)
                {
                    codeMemberMethod.StartDirectives.Add(codeRegionDirective);
                }
                else if (codeRegionDirective.RegionMode == CodeRegionMode.End)
                {
                    codeMemberMethod.EndDirectives.Add(codeRegionDirective);
                }
            }

            return codeMemberMethod;
        }

        private static CodeMethodInvokeExpression ContainsParameter(
            string parameterName
        )
        {
            return new CodeMethodInvokeExpression(ThisProp(ParametersPropertyName), "Contains", new CodeExpression[1]
            {
                StringLiteral(parameterName)
            });
        }

        private static CodeMethodInvokeExpression ContainsResult(string resultName)
        {
            return new CodeMethodInvokeExpression(ThisProp(ResultsPropertyName), "Contains", new CodeExpression[1]
            {
                StringLiteral(resultName)
            });
        }

        private static CodeConditionStatement If(CodeExpression condition, CodeExpression trueCode)
        {
            return If(condition, new CodeExpressionStatement(trueCode), null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeExpression trueCode
            , CodeExpression falseCode
        )
        {
            return If(condition, new CodeExpressionStatement(trueCode), new CodeExpressionStatement(falseCode));
        }

        private static CodeConditionStatement If(CodeExpression condition, CodeStatement trueStatement)
        {
            return If(condition, trueStatement, null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeStatement trueStatement
            , CodeStatement falseStatement
        )
        {
            var conditionStatement = new CodeConditionStatement(condition, new CodeStatement[1]
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

        private static CodeFieldReferenceExpression FieldRef(string targetTypeName, string fieldName)
        {
            return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(targetTypeName), fieldName);
        }

        private CodeMemberField Field(
            string name
            , Type type
            , object value
            , params CodeAttributeDeclaration[] attrs
        )
        {
            var codeMemberField = new CodeMemberField(type, name)
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
            var codeTypeParameter = new CodeTypeParameter(name);
            if (constraints != null)
            {
                codeTypeParameter.Constraints.AddRange(constraints.Select(TypeRef).ToArray());
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

        private CodeConstructor Constructor(params CodeExpression[] thisArgs)
        {
            var codeConstructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };

            if (thisArgs != null)
            {
                codeConstructor.ChainedConstructorArgs.AddRange(thisArgs);
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeConstructor.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeConstructor;
        }

        private CodeConstructor Constructor(CodeParameterDeclarationExpression arg, params CodeStatement[] statements)
        {
            var codeConstructor = new CodeConstructor
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

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeConstructor.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
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
                Left = ThisProp(propName),
                Right = value,
            };
        }

        private static CodeAssignStatement AssignValue(CodeExpression target)
        {
            return AssignValue(target, new CodeVariableReferenceExpression("value"));
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
            return new CodeIndexerExpression(ThisProp(propertyName), new CodeExpression[1]
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
            return new CodePropertyReferenceExpression(This(), propertyName);
        }

        private static CodeThisReferenceExpression This()
        {
            return new CodeThisReferenceExpression();
        }

        private static CodeMethodInvokeExpression ThisMethodInvoke(string methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(This(), methodName), parameters);
        }

        private static CodeMethodInvokeExpression ThisMethodInvoke(
            string methodName
            , CodeTypeReference typeParameter
            , params CodeExpression[] parameters
        )
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(This(), methodName, new CodeTypeReference[1]
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
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(typeof(Enum))), "ToObject", new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression( VarRef(variableName), "Value")
            });
        }

        private static CodeExpression ValueNull()
        {
            return new CodeBinaryOperatorExpression(VarRef("value"), CodeBinaryOperatorType.IdentityEquality, Null());
        }

        private static CodePrimitiveExpression Null()
        {
            return new CodePrimitiveExpression(null);
        }

        private static CodeBinaryOperatorExpression NotNull(CodeExpression expression)
        {
            return new CodeBinaryOperatorExpression(expression, CodeBinaryOperatorType.IdentityInequality, Null());
        }

        private static CodeBinaryOperatorExpression Null(CodeExpression expression)
        {
            return new CodeBinaryOperatorExpression(expression, CodeBinaryOperatorType.IdentityEquality, Null());
        }

        private static CodeExpression GuidEmpty()
        {
            return PropRef(new CodeTypeReferenceExpression(typeof(Guid)), "Empty");
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
            return new CodeThrowExceptionStatement(New(TypeRef(typeof(ArgumentNullException)), (CodeExpression)StringLiteral(paramName)));
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