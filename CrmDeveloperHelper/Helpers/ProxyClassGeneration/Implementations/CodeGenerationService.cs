using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Implementations
{
    internal sealed class CodeGenerationService : ICodeGenerationService
    {
        private static readonly Type EntityLogicalNameAttribute = typeof(EntityLogicalNameAttribute);
        private static readonly Type AttributeLogicalNameAttribute = typeof(AttributeLogicalNameAttribute);
        private static readonly Type RelationshipSchemaNameAttribute = typeof(RelationshipSchemaNameAttribute);
        private static readonly Type DebuggerNonUserCodeAttribute = typeof(System.Diagnostics.DebuggerNonUserCodeAttribute);

        private static readonly Type DescriptionAttribute = typeof(DescriptionAttribute);

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

        private static readonly string SdkMessageNamespaceFieldName = "SdkMessageNamespace";
        private static readonly string SdkMessageRequestNameFieldName = "SdkMessageRequestName";

        private static readonly string EntityLogicalNameFieldName = "EntityLogicalName";
        private static readonly string EntitySchemaNameFieldName = "EntitySchemaName";
        private static readonly string EntityTypeCodeFieldName = "EntityTypeCode";

        private static readonly string EntityPrimaryIdAttributeFieldName = "EntityPrimaryIdAttribute";
        private static readonly string EntityPrimaryNameAttributeFieldName = "EntityPrimaryNameAttribute";
        private static readonly string EntityPrimaryImageAttributeFieldName = "EntityPrimaryImageAttribute";

        private const string CSharpLanguage = "CSharp";

        private const string MethodOnPropertyChanged = "OnPropertyChanged";
        private const string MethodOnPropertyChanging = "OnPropertyChanging";

        private const string MethodSetAttributeValue = "SetAttributeValue";

        private const string MethodGetRelatedEntities = "GetRelatedEntities";
        private const string MethodSetRelatedEntities = "SetRelatedEntities";

        private const string MethodGetRelatedEntity = "GetRelatedEntity";
        private const string MethodSetRelatedEntity = "SetRelatedEntity";

        private const string MethodCreateQuery = "CreateQuery";

        private const string VariableNameValue = "value";
        private const string VariableNamePropertyName = "propertyName";
        private const string VariableNameCollection = "collection";
        private const string VariableNameOptionSetValue = "optionSetValue";
        private const string VariableNameOptionSetValueCollection = "optionSetValueCollection";
        private const string VariableNameService = "service";

        private readonly CreateFileCSharpConfiguration _config;

        public ICodeGenerationServiceProvider iCodeGenerationServiceProvider { get; set; }

        public CodeGenerationService(CreateFileCSharpConfiguration config)
        {
            this._config = config;
        }

        public Task WriteEntitiesFileAsync(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadata> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            return Task.Run(() => this.WriteEntitiesFile(entities, optionSets, messages, outputFilePath, outputNamespace, options));
        }

        private void WriteEntitiesFile(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadata> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildEntities(entities));

            codeNamespace.Types.AddRange(this.BuildGlobalOptionSets(optionSets));

            codeNamespace.Types.AddRange(this.BuildSdkMessagesEnumerable(messages));

            codeNamespace.Types.AddRange(this.BuildServiceContext(entities));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            codeCompileUnit.AssemblyCustomAttributes.Add(Attribute(typeof(ProxyTypesAssemblyAttribute)));

            options.IndentString = _config.TabSpacer;

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    using (var provider = CodeDomProvider.CreateProvider(CSharpLanguage))
                    {
                        provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                    }

                    try
                    {
                        streamWriter.Flush();
                        memoryStream.Flush();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(outputFilePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                }
            }
        }

        public Task WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            return Task.Run(() => this.WriteEntityFile(entityMetadata, outputFilePath, outputNamespace, options));
        }

        private void WriteEntityFile(
            EntityMetadata entityMetadata
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildEntity(entityMetadata));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            options.IndentString = _config.TabSpacer;

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    using (var provider = CodeDomProvider.CreateProvider(CSharpLanguage))
                    {
                        provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                    }

                    try
                    {
                        streamWriter.Flush();
                        memoryStream.Flush();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(outputFilePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                }
            }
        }

        public Task WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            return Task.Run(() => this.WriteSdkMessage(sdkMessage, outputFilePath, outputNamespace, options));
        }

        private void WriteSdkMessage(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildSdkMessage(sdkMessage));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            options.IndentString = _config.TabSpacer;

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    using (var provider = CodeDomProvider.CreateProvider(CSharpLanguage))
                    {
                        provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                    }

                    try
                    {
                        streamWriter.Flush();
                        memoryStream.Flush();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(outputFilePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                }
            }
        }

        public Task WriteSdkMessagePairAsync(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            return Task.Run(() => this.WriteSdkMessagePair(sdkMessagePair, outputFilePath, outputNamespace, options));
        }

        private void WriteSdkMessagePair(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildSdkMessagePair(sdkMessagePair));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            options.IndentString = _config.TabSpacer;

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    using (var provider = CodeDomProvider.CreateProvider(CSharpLanguage))
                    {
                        provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                    }

                    try
                    {
                        streamWriter.Flush();
                        memoryStream.Flush();

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileBody = memoryStream.ToArray();

                        File.WriteAllBytes(outputFilePath, fileBody);
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);
                    }
                }
            }
        }

        public CodeGenerationType GetTypeForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata)
        {
            return CodeGenerationType.Enum;
        }

        private CodeTypeDeclarationCollection BuildGlobalOptionSets(IEnumerable<OptionSetMetadata> optionSetMetadata)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var OptionSetMetadata in optionSetMetadata)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(OptionSetMetadata, null)
                    && OptionSetMetadata.IsGlobal.HasValue
                    && OptionSetMetadata.IsGlobal.Value
                )
                {
                    var codeTypeDeclaration = this.BuildOptionSet(null, OptionSetMetadata);
                    if (codeTypeDeclaration != null)
                    {
                        declarationCollection.Add(codeTypeDeclaration);
                    }
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata)
        {
            var codeTypeDeclaration = this.Enum(iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entityMetadata, optionSetMetadata), Attribute(typeof(DataContractAttribute)));

            codeTypeDeclaration.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForOptionSet(entityMetadata, optionSetMetadata)));

            foreach (var option in optionSetMetadata.Options)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOption(option))
                {
                    codeTypeDeclaration.Members.Add(this.BuildOption(optionSetMetadata, option));
                }
            }

            return codeTypeDeclaration;
        }

        private CodeTypeMember BuildOption(OptionSetMetadata optionSetMetadata, OptionMetadata option)
        {
            var codeMemberField = this.Field(iCodeGenerationServiceProvider.NamingService.GetNameForOption(optionSetMetadata, option), typeof(int), option.Value.Value, Attribute(typeof(EnumMemberAttribute)));

            codeMemberField.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForOption(optionSetMetadata, option)));

            return codeMemberField;
        }

        private CodeTypeDeclarationCollection BuildEntities(IEnumerable<EntityMetadata> entityMetadata)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var entityMetadata1 in entityMetadata.OrderBy(metadata => metadata.LogicalName))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1))
                {
                    declarationCollection.AddRange(this.BuildEntity(entityMetadata1));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildEntity(EntityMetadata entityMetadata)
        {
            HashSet<string> linkedEntities = GetLinkedEntities(entityMetadata);

            if (linkedEntities.Any())
            {
                iCodeGenerationServiceProvider.MetadataProviderService.RetrieveEntities(linkedEntities);
            }

            var declarationCollection = new CodeTypeDeclarationCollection();

            var entityClassName = iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata);

            iCodeGenerationServiceProvider.NamingService.SetCurrentTypeName(entityClassName);

            var entityClass = this.Class(entityClassName, TypeRef(EntityClassBaseType)
                , Attribute(typeof(DataContractAttribute))
            );

            CodeExpression entityLogicalNameAttributeRef = FieldRef(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata), EntityLogicalNameFieldName);

            entityClass.CustomAttributes.Add(Attribute(EntityLogicalNameAttribute, AttributeArg(entityLogicalNameAttributeRef)));

            if (_config.AddDescriptionAttribute)
            {
                string description = CreateFileHandler.GetLocalizedLabel(entityMetadata.DisplayName);

                if (string.IsNullOrEmpty(description))
                {
                    description = CreateFileHandler.GetLocalizedLabel(entityMetadata.Description);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    entityClass.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            entityClass.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntity(entityMetadata)));

            this.InitializeEntityClass(entityClass, entityMetadata);

            entityClass.Members.AddRange(this.BuildAttributeTypeMembers(declarationCollection, entityMetadata));
            entityClass.Members.AddRange(this.BuildManyToOneRelationships(entityMetadata));
            entityClass.Members.AddRange(this.BuildOneToManyRelationships(entityMetadata));
            entityClass.Members.AddRange(this.BuildManyToManyRelationships(entityMetadata));

            declarationCollection.Add(entityClass);

            return declarationCollection;
        }

        public static HashSet<string> GetLinkedEntities(EntityMetadata entityMetadata)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var oneToMany in entityMetadata.OneToManyRelationships)
            {
                result.Add(oneToMany.ReferencingEntity);
            }

            foreach (var oneToMany in entityMetadata.ManyToOneRelationships)
            {
                result.Add(oneToMany.ReferencedEntity);
            }

            foreach (var manyToMany in entityMetadata.ManyToManyRelationships)
            {
                result.Add(entityMetadata.LogicalName != manyToMany.Entity1LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName);
            }

            return result;
        }

        private CodeTypeMemberCollection BuildAttributeTypeMembers(CodeTypeDeclarationCollection declarationCollection, EntityMetadata entityMetadata)
        {
            var memberCollection = new CodeTypeMemberCollection();

            CodeExpression primatyIdAttributeRef = FieldRef(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata), EntityPrimaryIdAttributeFieldName);
            CodeExpression primatyNameAttributeRef = FieldRef(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata), EntityPrimaryNameAttributeFieldName);

            {
                CodeRegionDirective startCodeRegionDirective = null;
                string regionName = Properties.CodeGenerationStrings.PrimaryAttributes;

                if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryId.GetValueOrDefault()
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata)
                    )
                    {
                        var attributeMember = this.BuildIdProperty(entityMetadata, attributeMetadata, primatyIdAttributeRef);

                        if (attributeMember != null)
                        {
                            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                        }

                        attributeMember = this.BuildAttributePropertyOnBaseType(entityMetadata, attributeMetadata, primatyIdAttributeRef);

                        if (attributeMember != null)
                        {
                            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryName.GetValueOrDefault()
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata)
                    )
                    {
                        var attributeMember = this.BuildAttributePropertyOnBaseType(entityMetadata, attributeMetadata, primatyNameAttributeRef);

                        if (attributeMember != null)
                        {
                            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                        }
                    }
                }

                CreateEndCodeRegionInNeeded(startCodeRegionDirective, memberCollection, regionName);
            }

            {
                var notPrimaryAttributes = entityMetadata.Attributes.Where(a =>
                    !string.Equals(a.LogicalName, entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase)
                    && !string.Equals(a.LogicalName, entityMetadata.PrimaryNameAttribute, StringComparison.InvariantCultureIgnoreCase)
                );

                var oobAttributes = notPrimaryAttributes.Where(a => !a.IsCustomAttribute.GetValueOrDefault());
                var customAttributes = notPrimaryAttributes.Where(a => a.IsCustomAttribute.GetValueOrDefault());

                WriteAttributeEnumeration(declarationCollection, memberCollection, Properties.CodeGenerationStrings.OOBAttributes, entityMetadata, oobAttributes);

                WriteAttributeEnumeration(declarationCollection, memberCollection, Properties.CodeGenerationStrings.CustomAttributes, entityMetadata, customAttributes);
            }

            return memberCollection;
        }

        private void WriteAttributeEnumeration(
            CodeTypeDeclarationCollection declarationCollection
            , CodeTypeMemberCollection memberCollection
            , string regionName
            , EntityMetadata entityMetadata
            , IEnumerable<AttributeMetadata> attributesEnumeration
        )
        {
            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var attributeMetadata in attributesEnumeration.OrderBy(metadata => metadata.LogicalName))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata))
                {
                    var attributeOptionSet = TypeMappingService.GetAttributeOptionSet(attributeMetadata);

                    if (attributeOptionSet != null
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(attributeOptionSet, attributeMetadata)
                    )
                    {
                        CodeTypeDeclaration codeTypeDeclarationOptionSet = this.BuildOptionSet(entityMetadata, attributeOptionSet);
                        if (codeTypeDeclarationOptionSet != null)
                        {
                            declarationCollection.Add(codeTypeDeclarationOptionSet);
                        }
                    }

                    string attributePropertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata);
                    CodeExpression attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributePropertyName, attributeMetadata.LogicalName);

                    ProxyClassAttributeEnums proxyClassAttributeEnums = GetProxyClassAttributeEnums(attributeMetadata, attributeOptionSet);
                    ProxyClassAttributeEnumsGlobalOptionSetLocation proxyClassAttributeEnumsGlobalOptionSetLocation = GetProxyClassAttributeEnumsGlobalOptionSetLocation(attributeMetadata, attributeOptionSet);

                    string enumPropertyName = string.Empty;
                    CodeMemberProperty enumAttributeMember = null;

                    if (attributeOptionSet != null
                        && !iCodeGenerationServiceProvider.CodeWriterFilterService.IgnoreOptionSet(attributeOptionSet, attributeMetadata)
                        && proxyClassAttributeEnums != ProxyClassAttributeEnums.NotNeeded
                    )
                    {
                        enumPropertyName = attributePropertyName;
                        string basePropertyName = string.Empty;

                        if (proxyClassAttributeEnums == Model.ProxyClassAttributeEnums.AddWithNameAttributeEnum)
                        {
                            enumPropertyName += "Enum";
                            basePropertyName = attributePropertyName;
                        }

                        string enumTypeName = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForOptionSet(entityMetadata, attributeOptionSet).BaseType;

                        if (proxyClassAttributeEnumsGlobalOptionSetLocation == ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassSchema)
                        {
                            var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata);

                            enumTypeName = $"{typeRef.BaseType}.Schema.OptionSets.";

                            if (attributeOptionSet.IsGlobal.GetValueOrDefault())
                            {
                                enumTypeName += attributeOptionSet.Name.ToLower();
                            }
                            else
                            {
                                enumTypeName += attributeMetadata.LogicalName;
                            }
                        }
                        else if (proxyClassAttributeEnumsGlobalOptionSetLocation == ProxyClassAttributeEnumsGlobalOptionSetLocation.InGlobalOptionSetNamespace)
                        {
                            enumTypeName = attributeOptionSet.Name.ToLower();

                            if (!string.IsNullOrEmpty(_config.NamespaceGlobalOptionSets))
                            {
                                enumTypeName = $"{_config.NamespaceGlobalOptionSets}.{enumTypeName}";
                            }
                        }

                        enumAttributeMember = this.BuildAttributePropertyOnEnum(
                            entityMetadata
                            , attributeMetadata
                            , enumPropertyName
                            , basePropertyName
                            , attributeNameRef
                            , enumTypeName
                        );
                    }

                    if (attributeOptionSet == null || (attributeOptionSet != null && proxyClassAttributeEnums != ProxyClassAttributeEnums.InsteadOfOptionSet))
                    {
                        CodeMemberProperty attributeMember = this.BuildAttributePropertyOnBaseType(
                            entityMetadata
                            , attributeMetadata
                            , attributeNameRef
                            , enumPropertyName
                        );

                        if (attributeMember != null)
                        {
                            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                        }
                    }

                    if (enumAttributeMember != null)
                    {
                        CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(enumAttributeMember));
                    }
                }
            }

            CreateEndCodeRegionInNeeded(startCodeRegionDirective, memberCollection, regionName);
        }

        private static void CreateStartCodeRegionInNeeded(ref CodeRegionDirective startCodeRegionDirective, CodeTypeMemberCollection memberCollection, string regionName)
        {
            if (startCodeRegionDirective != null)
            {
                return;
            }

            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
            {
                StartDirectives =
                {
                    startCodeRegionDirective,
                },
            });
        }

        private static void CreateEndCodeRegionInNeeded(CodeRegionDirective startCodeRegionDirective, CodeTypeMemberCollection memberCollection, string regionName)
        {
            if (startCodeRegionDirective == null)
            {
                return;
            }

            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
            {
                EndDirectives =
                {
                    new CodeRegionDirective(CodeRegionMode.End, regionName),
                },
            });
        }

        private ProxyClassAttributeEnumsGlobalOptionSetLocation GetProxyClassAttributeEnumsGlobalOptionSetLocation(AttributeMetadata attributeMetadata, OptionSetMetadata attributeOptionSet)
        {
            if (attributeOptionSet == null)
            {
                return ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassesNamespace;
            }

            if (attributeMetadata is StateAttributeMetadata
                || attributeMetadata is StatusAttributeMetadata
            )
            {
                return _config.GenerateAttributesEnumsStateStatusUseSchemaEnum
                    ? ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassSchema
                    : ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassesNamespace;
            }

            if (attributeOptionSet.IsGlobal.GetValueOrDefault())
            {
                return _config.GenerateAttributesEnumsGlobalUseSchemaEnum;
            }
            else
            {
                return _config.GenerateAttributesEnumsLocalUseSchemaEnum
                    ? ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassSchema
                    : ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassesNamespace;
            }
        }

        private ProxyClassAttributeEnums GetProxyClassAttributeEnums(AttributeMetadata attributeMetadata, OptionSetMetadata attributeOptionSet)
        {
            if (attributeOptionSet == null)
            {
                return ProxyClassAttributeEnums.NotNeeded;
            }

            if (attributeMetadata is StateAttributeMetadata
                || attributeMetadata is StatusAttributeMetadata
            )
            {
                return _config.GenerateAttributesEnumsStateStatus;
            }

            if (attributeOptionSet.IsGlobal.GetValueOrDefault())
            {
                return _config.GenerateAttributesEnumsGlobal;
            }
            else
            {
                return _config.GenerateAttributesEnumsLocal;
            }
        }

        private void InitializeEntityClass(CodeTypeDeclaration entityClass, EntityMetadata entityMetadata)
        {
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanging)));
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanged)));

            entityClass.Members.Add(this.BuildClassConstant(EntityLogicalNameFieldName, typeof(string), entityMetadata.LogicalName));

            entityClass.Members.Add(this.BuildClassConstant(EntitySchemaNameFieldName, typeof(string), entityMetadata.SchemaName));

            if (entityMetadata.ObjectTypeCode.HasValue && !entityMetadata.IsCustomEntity.GetValueOrDefault())
            {
                entityClass.Members.Add(this.BuildClassConstant(EntityTypeCodeFieldName, typeof(int), entityMetadata.ObjectTypeCode));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant(EntityPrimaryIdAttributeFieldName, typeof(string), entityMetadata.PrimaryIdAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant(EntityPrimaryNameAttributeFieldName, typeof(string), entityMetadata.PrimaryNameAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryImageAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant(EntityPrimaryImageAttributeFieldName, typeof(string), entityMetadata.PrimaryImageAttribute));
            }

            entityClass.Members.Add(this.EntityConstructorDefault(entityMetadata));

            if (_config.AddConstructorWithAnonymousTypeObject)
            {
                entityClass.Members.Add(this.EntityConstructorAnonymousObject(entityMetadata));
            }

            string regionName = Properties.CodeGenerationStrings.NotifyPropertyEvents;

            CodeRegionDirective startCodeRegionDirective = null;

            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, entityClass.Members, regionName);

            entityClass.Members.Add(this.Event(nameof(INotifyPropertyChanged.PropertyChanged), typeof(PropertyChangedEventHandler), typeof(INotifyPropertyChanged)));
            entityClass.Members.Add(this.Event(nameof(INotifyPropertyChanging.PropertyChanging), typeof(PropertyChangingEventHandler), typeof(INotifyPropertyChanging)));
            entityClass.Members.Add(this.RaiseEvent(MethodOnPropertyChanged, nameof(INotifyPropertyChanged.PropertyChanged), typeof(PropertyChangedEventArgs)));
            entityClass.Members.Add(this.RaiseEvent(MethodOnPropertyChanging, nameof(INotifyPropertyChanging.PropertyChanging), typeof(PropertyChangingEventArgs)));

            CreateEndCodeRegionInNeeded(startCodeRegionDirective, entityClass.Members, regionName);
        }

        private CodeTypeMember BuildClassConstant(string constName, Type type, object value)
        {
            var codeMemberField = this.Field(constName, type, value);
            codeMemberField.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            return codeMemberField;
        }

        private CodeTypeMember EntityConstructorDefault(EntityMetadata entityMetadata)
        {
            var codeConstructor = this.Constructor();

            codeConstructor.BaseConstructorArgs.Add(FieldRef(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata), EntityLogicalNameFieldName));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntityDefaultConstructor(entityMetadata)));

            return codeConstructor;
        }

        private CodeTypeMember EntityConstructorAnonymousObject(EntityMetadata entityMetadata)
        {
            var codeConstructor = this.Constructor();

            string primatyIdAttributeRef = string.Format("{0}.{1}", iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata).BaseType, EntityPrimaryIdAttributeFieldName);

            codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Object), "anonymousObject"));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForEntityAnonymousConstructor(entityMetadata)));

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
{0}{0}{0}{0}{0}case {1}:
{0}{0}{0}{0}{0}{0}if (value is System.Guid idValue)
{0}{0}{0}{0}{0}{0}{{
{0}{0}{0}{0}{0}{0}{0}Attributes[{1}] = base.Id = idValue;
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
{0}{0}{0}}}", _config.TabSpacer, primatyIdAttributeRef)));

            return codeConstructor;
        }

        private CodeMemberProperty BuildAttributePropertyOnBaseType(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, CodeExpression attributeNameRef, string enumPropertyName = null)
        {
            CodeTypeReference forAttributeType = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForAttributeType(entityMetadata, attributeMetadata);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttributeAsEntityProperty(entityMetadata, attributeMetadata);

            var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, propertyName, attributeNameRef, forAttributeType);

            if (codeMemberProperty.HasGet)
            {
                codeMemberProperty.GetStatements.AddRange(this.BuildAttributeGet(attributeMetadata, forAttributeType, attributeNameRef));
            }

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildAttributeSet(entityMetadata, attributeMetadata, codeMemberProperty.Name, attributeNameRef, enumPropertyName));
            }

            return codeMemberProperty;
        }

        private CodeMemberProperty BuildAttributePropertyOnEnum(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , string enumPropertyName
            , string basePropertyName
            , CodeExpression attributeNameRef
            , string enumTypeName
        )
        {
            var enumTypeRef = TypeRef(enumTypeName);

            if (attributeMetadata is MultiSelectPicklistAttributeMetadata)
            {
                CodeTypeReference forAttributeType = IEnumerable(enumTypeRef);

                var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, enumPropertyName, attributeNameRef, forAttributeType);

                if (codeMemberProperty.HasGet)
                {
                    codeMemberProperty.GetStatements.AddRange(this.BuildMultiEnumAttributeGet(attributeNameRef, enumTypeRef));
                }

                if (codeMemberProperty.HasSet)
                {
                    codeMemberProperty.SetStatements.AddRange(this.BuildMultiEnumAttributeSet(attributeNameRef, enumPropertyName, basePropertyName, enumTypeRef));
                }

                return codeMemberProperty;
            }
            else
            {
                CodeTypeReference forAttributeType = TypeRef(typeof(Nullable<>), enumTypeRef);

                var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, enumPropertyName, attributeNameRef, forAttributeType);

                if (codeMemberProperty.HasGet)
                {
                    codeMemberProperty.GetStatements.AddRange(this.BuildEnumAttributeGet(attributeNameRef, enumTypeRef));
                }

                if (codeMemberProperty.HasSet)
                {
                    codeMemberProperty.SetStatements.AddRange(this.BuildEnumAttributeSet(attributeNameRef, enumPropertyName, basePropertyName));
                }

                return codeMemberProperty;
            }
        }

        private CodeMemberProperty BuildAttributePropertyCommon(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , string propertyName
            , CodeExpression attributeNameRef
            , CodeTypeReference forAttributeType
        )
        {
            var codeMemberProperty = this.BuildClassProperty(forAttributeType, propertyName);

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable || attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attributeMetadata.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            if (_config.AddDescriptionAttribute)
            {
                string description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);

                if (string.IsNullOrEmpty(description))
                {
                    description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.Description);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (!string.IsNullOrEmpty(attributeMetadata.DeprecatedVersion) && !_config.WithoutObsoleteAttribute)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(ObsoleteFieldAttribute));
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata)));

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
                using (var provider = CodeDomProvider.CreateProvider(CSharpLanguage))
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

        private CodeStatementCollection BuildAttributeGet(AttributeMetadata attributeMetadata, CodeTypeReference targetType, CodeExpression attributeNameRef)
        {
            var statementCollection = new CodeStatementCollection();

            if (attributeMetadata.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList
                && targetType.TypeArguments.Count > 0
            )
            {
                statementCollection.AddRange(BuildEntityCollectionAttributeGet(attributeNameRef, targetType));
            }
            //else if (attributeMetadata.AttributeTypeName != null
            //    && attributeMetadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType
            //)
            //{
            //    statementCollection.AddRange(BuildAttributeGetNotNull(attributeNameRef, targetType));
            //}
            else
            {
                statementCollection.Add(Return(ThisMethodInvoke(nameof(Entity.GetAttributeValue), targetType, attributeNameRef)));
            }

            return statementCollection;
        }

        private static CodeStatementCollection BuildAttributeGetNotNull(CodeExpression attributeNameRef, CodeTypeReference propertyType)
        {
            var collectionVarRef = VarRef(VariableNameOptionSetValueCollection);

            return new CodeStatementCollection()
            {
                Var(propertyType, VariableNameOptionSetValueCollection, ThisMethodInvoke(nameof(Entity.GetAttributeValue), propertyType, attributeNameRef)),
                If(IsExpressionNull(collectionVarRef), new CodeStatement[]
                {
                    AssignValue(collectionVarRef, New(propertyType)),
                    new CodeExpressionStatement(ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, collectionVarRef)),
                }),
                Return(collectionVarRef),
            };
        }

        private CodeStatementCollection BuildAttributeSet(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , string propertyName
            , CodeExpression attributeNameRef
            , string enumPropertyName = null
        )
        {
            var valueVarRef = VarRef(VariableNameValue);
            var basePropId = BaseProp(nameof(Entity.Id));

            var statementCollection = new CodeStatementCollection();

            if (!string.IsNullOrEmpty(enumPropertyName))
            {
                statementCollection.Add(this.InvokeOnPropertyChanging(enumPropertyName));
            }

            statementCollection.Add(this.InvokeOnPropertyChanging(propertyName));

            if (attributeMetadata.AttributeType.GetValueOrDefault() == AttributeTypeCode.PartyList)
            {
                statementCollection.Add(BuildEntityCollectionAttributeSet(attributeNameRef));
            }
            else if (string.Equals(entityMetadata.PrimaryIdAttribute, attributeMetadata.LogicalName, StringComparison.InvariantCulture) && attributeMetadata.IsPrimaryId.GetValueOrDefault())
            {
                statementCollection.Add(If(
                    And
                    (
                        PropRef(valueVarRef, nameof(Nullable<Guid>.HasValue))
                        , NotEqual(PropRef(valueVarRef, nameof(Nullable<Guid>.Value)), GuidEmpty())
                    )
                    , new CodeStatement[]
                    {
                        AssignValue(basePropId, PropRef(valueVarRef, nameof(Nullable<Guid>.Value)))
                        , new CodeExpressionStatement(ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, valueVarRef))
                    }
                    , new CodeStatement[]
                    {
                        AssignValue(basePropId, GuidEmpty())
                        , new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ThisProp(nameof(Entity.Attributes)), nameof(Entity.Attributes.Remove)), attributeNameRef))
                    }
                ));
            }
            else
            {
                statementCollection.Add(ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, valueVarRef));
            }

            statementCollection.Add(this.InvokeOnPropertyChanged(propertyName));

            if (!string.IsNullOrEmpty(enumPropertyName))
            {
                statementCollection.Add(this.InvokeOnPropertyChanged(enumPropertyName));
            }

            return statementCollection;
        }

        private CodeExpression InvokeOnPropertyChanging(string propertyName)
        {
            return this.InvokeMethodOnPropertyChange(MethodOnPropertyChanging, propertyName);
        }

        private CodeExpression InvokeOnPropertyChanged(string propertyName)
        {
            return this.InvokeMethodOnPropertyChange(MethodOnPropertyChanged, propertyName);
        }

        private CodeExpression InvokeMethodOnPropertyChange(string methodName, string propertyName)
        {
            if (this._config.GenerateAttributesWithNameOf)
            {
                return ThisMethodInvoke(methodName, NameOfPropertyExpression(propertyName));
            }
            else
            {
                return ThisMethodInvoke(methodName, StringLiteral(propertyName));
            }
        }

        private static CodeStatementCollection BuildEntityCollectionAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference propertyType
        )
        {
            var variableCollectionRef = VarRef(VariableNameCollection);

            return new CodeStatementCollection()
            {
                Var(typeof(EntityCollection), VariableNameCollection, ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(EntityCollection)), attributeNameRef))
                , If(
                    And(IsNotNull(variableCollectionRef), IsNotNull(PropRef(variableCollectionRef, nameof(EntityCollection.Entities))))
                    , Return(StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Select)
                        , new[]{ TypeRef(EntityClassBaseType), propertyType.TypeArguments[0] }
                        , PropRef(variableCollectionRef, nameof(EntityCollection.Entities))
                        , new CodeSnippetExpression($"e => e.ToEntity<{propertyType.TypeArguments[0].BaseType}>()")
                    ))
                    , Return(StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Empty), propertyType.TypeArguments[0]))
                )
            };
        }

        private static CodeStatement BuildEntityCollectionAttributeSet(CodeExpression attributeNameRef)
        {
            var valueRef = VarRef(VariableNameValue);

            return If(IsValueNull()
                , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, valueRef)
                , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef
                    , New(TypeRef(typeof(EntityCollection)), New(TypeRef(typeof(List<Entity>)), valueRef))
            ));
        }

        private CodeMemberProperty BuildIdProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , CodeExpression attributeNameRef
        )
        {
            var codeMemberProperty = this.BuildClassProperty(TypeRef(typeof(Guid)), nameof(Entity.Id));

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            if (_config.AddDescriptionAttribute)
            {
                string description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.DisplayName);

                if (string.IsNullOrEmpty(description))
                {
                    description = CreateFileHandler.GetLocalizedLabel(attributeMetadata.Description);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Attributes = MemberAttributes.Public | MemberAttributes.Override;

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable || attributeMetadata.IsValidForCreate.GetValueOrDefault() || attributeMetadata.IsValidForUpdate.GetValueOrDefault();
            codeMemberProperty.HasGet = attributeMetadata.IsValidForRead.GetValueOrDefault() || codeMemberProperty.HasSet;

            codeMemberProperty.GetStatements.Add(Return(BaseProp(nameof(Entity.Id))));

            var valueVarRef = VarRef(VariableNameValue);

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.Add(AssignValue(ThisProp(iCodeGenerationServiceProvider.NamingService.GetNameForAttributeAsEntityProperty(entityMetadata, attributeMetadata)), valueVarRef));
            }
            else
            {
                codeMemberProperty.SetStatements.Add(AssignValue(BaseProp(nameof(Entity.Id)), valueVarRef));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata)));

            return codeMemberProperty;
        }

        private CodeStatementCollection BuildEnumAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference enumTypeRef
        )
        {
            var optionSetValueVarRef = VarRef(VariableNameOptionSetValue);

            return new CodeStatementCollection(new CodeStatement[2]
            {
                Var(typeof(OptionSetValue), VariableNameOptionSetValue, ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(OptionSetValue)), attributeNameRef)),
                If(And(IsNotNull(optionSetValueVarRef), EnumIsDefined(enumTypeRef, VariableNameOptionSetValue))
                    , Return(Cast(enumTypeRef, ConvertEnum(enumTypeRef, VariableNameOptionSetValue)))
                    , Return(DefaultTypeValue(TypeRef(typeof(Nullable<>), enumTypeRef)))
                ),
            });
        }

        private CodeStatementCollection BuildEnumAttributeSet(
            CodeExpression attributeNameRef
            , string enumPropertyName
            , string basePropertyName
        )
        {
            var result = new CodeStatementCollection()
            {
                this.InvokeOnPropertyChanging(enumPropertyName),
            };

            if (!string.IsNullOrEmpty(basePropertyName))
            {
                result.Add(this.InvokeOnPropertyChanging(basePropertyName));
            }

            result.Add(If(
                    IsValueNull()
                    , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, Null())
                    , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, New(TypeRef(typeof(OptionSetValue)), (CodeExpression)Cast(TypeRef(typeof(int)), VarRef(VariableNameValue))))
                )
            );

            if (!string.IsNullOrEmpty(basePropertyName))
            {
                result.Add(this.InvokeOnPropertyChanged(basePropertyName));
            }

            result.Add(this.InvokeOnPropertyChanged(enumPropertyName));

            return result;
        }

        private CodeStatementCollection BuildMultiEnumAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference enumTypeRef
        )
        {
            var optionSetValueCollectionVarRef = VarRef(VariableNameOptionSetValueCollection);

            var listType = TypeRef(typeof(List<>), enumTypeRef);

            return new CodeStatementCollection(new CodeStatement[]
            {
                Var(typeof(OptionSetValueCollection), VariableNameOptionSetValueCollection, ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(OptionSetValueCollection)), attributeNameRef)),

                If(IsNotNull(optionSetValueCollectionVarRef)

                    , Return(New(TypeRef(typeof(List<>), enumTypeRef)
                        , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Distinct), enumTypeRef
                            , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Select), new[] { TypeRef(typeof(OptionSetValue)), enumTypeRef }
                                , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Where), TypeRef(typeof(OptionSetValue))
                                    , optionSetValueCollectionVarRef
                                    , new CodeSnippetExpression($"o => o != null && System.Enum.IsDefined(typeof({enumTypeRef.BaseType}), o.Value)")
                                )
                                , new CodeSnippetExpression($"o => ({enumTypeRef.BaseType})(System.Enum.ToObject(typeof({enumTypeRef.BaseType}), o.Value))")
                            )
                        )
                    )),

                    Return(StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Empty), enumTypeRef))
                ),
            });
        }

        private CodeStatementCollection BuildMultiEnumAttributeSet(
            CodeExpression attributeNameRef
            , string enumPropertyName
            , string basePropertyName
            , CodeTypeReference enumTypeRef
        )
        {
            var result = new CodeStatementCollection()
            {
                this.InvokeOnPropertyChanging(enumPropertyName),
            };

            if (!string.IsNullOrEmpty(basePropertyName))
            {
                result.Add(this.InvokeOnPropertyChanging(basePropertyName));
            }

            result.Add(If(Or(IsValueNull(), Equal(StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Any), enumTypeRef, VarRef(VariableNameValue)), False()))
                , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef, Null())

                , ThisMethodInvoke(MethodSetAttributeValue, attributeNameRef
                    , New(TypeRef(typeof(OptionSetValueCollection))
                        , New(TypeRef(typeof(List<OptionSetValue>))
                            , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Select), new[] { enumTypeRef, TypeRef(typeof(OptionSetValue)) }
                                , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Distinct), enumTypeRef
                                    , VarRef(VariableNameValue)
                                )
                                , new CodeSnippetExpression($"o => new Microsoft.Xrm.Sdk.OptionSetValue((int)o)")
                            )
                        )
                    )
                )
            ));

            if (!string.IsNullOrEmpty(basePropertyName))
            {
                result.Add(this.InvokeOnPropertyChanged(basePropertyName));
            }

            result.Add(this.InvokeOnPropertyChanged(enumPropertyName));

            return result;
        }

        private CodeTypeMemberCollection BuildOneToManyRelationships(EntityMetadata entityMetadata)
        {
            var memberCollection = new CodeTypeMemberCollection();
            if (entityMetadata.OneToManyRelationships == null)
            {
                return memberCollection;
            }

            string regionName = string.Format(Properties.CodeGenerationStrings.OneToManyRelationshipsFormat2, "OneToMany", "1:N");

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var oneToMany in entityMetadata.OneToManyRelationships.OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntityMetadata = iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata(oneToMany.ReferencingEntity);

                if (otherEntityMetadata != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntityMetadata)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(oneToMany, otherEntityMetadata, CodeGenerationRelationshipType.OneToManyRelationship)
                )
                {
                    CodeMemberProperty codeTypeMember = this.BuildOneToManyProperty(entityMetadata, otherEntityMetadata, oneToMany);

                    if (codeTypeMember != null)
                    {
                        CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeTypeMember));
                    }

                    if (string.Equals(oneToMany.SchemaName, "calendar_calendar_rules", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(oneToMany.SchemaName, "service_calendar_rules", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var calendarRuleProperty = this.BuildCalendarRuleProperty(entityMetadata, otherEntityMetadata, oneToMany);

                        if (calendarRuleProperty != null)
                        {
                            CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(calendarRuleProperty));
                        }
                    }
                }
            }

            CreateEndCodeRegionInNeeded(startCodeRegionDirective, memberCollection, regionName);

            return memberCollection;
        }

        private CodeMemberProperty BuildCalendarRuleProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntity
            , OneToManyRelationshipMetadata oneToMany
        )
        {
            var codeMemberProperty = this.BuildClassProperty(IEnumerable(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntity)), "CalendarRules");

            var nullable = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var attributeNameRef = StringLiteral("calendarrules");

            codeMemberProperty.GetStatements.AddRange(BuildEntityCollectionAttributeGet(attributeNameRef, codeMemberProperty.Type));

            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanging(codeMemberProperty.Name));
            codeMemberProperty.SetStatements.Add(BuildEntityCollectionAttributeSet(attributeNameRef));
            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanged(codeMemberProperty.Name));

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, nullable);

            if (_config.AddDescriptionAttribute)
            {
                string description = comments.FirstOrDefault();

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(comments));

            return codeMemberProperty;
        }

        private CodeMemberProperty BuildOneToManyProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , OneToManyRelationshipMetadata oneToMany
        )
        {
            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntityMetadata);

            var entityRole = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var codeMemberProperty = this.BuildClassProperty(IEnumerable(typeForRelationship), iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, oneToMany, entityRole));

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "OneToMany", oneToMany.SchemaName);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet(MethodGetRelatedEntities, relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet(MethodSetRelatedEntities, relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, entityRole);

            if (_config.AddDescriptionAttribute)
            {
                string description = comments.FirstOrDefault();

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(comments));

            return codeMemberProperty;
        }

        private CodeTypeMemberCollection BuildManyToManyRelationships(EntityMetadata entityMetadata)
        {
            var memberCollection = new CodeTypeMemberCollection();

            if (entityMetadata.ManyToManyRelationships == null)
            {
                return memberCollection;
            }

            string regionName = Properties.CodeGenerationStrings.ManyToManyRelationships;

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var manyToMany in entityMetadata.ManyToManyRelationships.OfType<ManyToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntityMetadata = iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata(entityMetadata.LogicalName != manyToMany.Entity1LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName);
                if (otherEntityMetadata != null)
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntityMetadata)
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToMany, otherEntityMetadata, CodeGenerationRelationshipType.ManyToManyRelationship)
                    )
                    {
                        if (otherEntityMetadata.LogicalName != entityMetadata.LogicalName)
                        {
                            var nameForRelationship = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?());
                            var many = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship, new EntityRole?());

                            if (many != null)
                            {
                                CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many));
                            }
                        }
                        else
                        {
                            var nameForRelationship1 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?(EntityRole.Referencing));
                            var many1 = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship1, new EntityRole?(EntityRole.Referencing));

                            if (many1 != null)
                            {
                                CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many1));
                            }

                            var nameForRelationship2 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?(EntityRole.Referenced));
                            var many2 = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship2, new EntityRole?(EntityRole.Referenced));

                            if (many2 != null)
                            {
                                CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many2));
                            }
                        }
                    }
                }
            }

            CreateEndCodeRegionInNeeded(startCodeRegionDirective, memberCollection, regionName);

            return memberCollection;
        }

        private CodeMemberProperty BuildManyToManyProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , ManyToManyRelationshipMetadata manyToMany
            , string propertyName
            , EntityRole? entityRole
        )
        {
            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToMany, otherEntityMetadata);

            var codeMemberProperty = this.BuildClassProperty(IEnumerable(typeForRelationship), propertyName);

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "ManyToMany", manyToMany.SchemaName);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet(MethodGetRelatedEntities, relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet(MethodSetRelatedEntities, relationshipNameRef, typeForRelationship, propertyName, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToMany(entityMetadata, manyToMany, entityRole);

            if (_config.AddDescriptionAttribute)
            {
                string description = comments.FirstOrDefault();

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(comments));

            return codeMemberProperty;
        }

        private CodeTypeMemberCollection BuildManyToOneRelationships(EntityMetadata entityMetadata)
        {
            var memberCollection = new CodeTypeMemberCollection();

            if (entityMetadata.ManyToOneRelationships == null)
            {
                return memberCollection;
            }

            string regionName = string.Format(Properties.CodeGenerationStrings.OneToManyRelationshipsFormat2, "ManyToOne", "N:1");

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var manyToOne in entityMetadata.ManyToOneRelationships.OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntity = iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata(manyToOne.ReferencedEntity);

                if (otherEntity != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntity)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToOne, otherEntity, CodeGenerationRelationshipType.ManyToOneRelationship)
                )
                {
                    var one = this.BuildManyToOneProperty(entityMetadata, otherEntity, manyToOne);
                    if (one != null)
                    {
                        CreateStartCodeRegionInNeeded(ref startCodeRegionDirective, memberCollection, regionName);

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(one));
                    }
                }
            }

            CreateEndCodeRegionInNeeded(startCodeRegionDirective, memberCollection, regionName);

            return memberCollection;
        }

        private CodeMemberProperty BuildManyToOneProperty(
            EntityMetadata entityMetadata
            , EntityMetadata otherEntityMetadata
            , OneToManyRelationshipMetadata manyToOne
        )
        {
            var attributeMetadata = entityMetadata.Attributes.SingleOrDefault(attribute => attribute.LogicalName == manyToOne.ReferencingAttribute);

            if (attributeMetadata == null)
            {
                return null;
            }

            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(manyToOne, otherEntityMetadata);

            var entityRole = otherEntityMetadata.LogicalName == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referencing) : new EntityRole?();

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "ManyToOne", manyToOne.SchemaName);

            var codeMemberProperty = this.BuildClassProperty(typeForRelationship, iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToOne, entityRole));

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet(MethodGetRelatedEntity, relationshipNameRef, typeForRelationship, entityRole));

            if (_config.MakeAllPropertiesEditable
                || attributeMetadata.IsValidForCreate.GetValueOrDefault()
                || attributeMetadata.IsValidForUpdate.GetValueOrDefault()
            )
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet(MethodSetRelatedEntity, relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));
            }

            string attributePropertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata);

            var attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributePropertyName, attributeMetadata.LogicalName);

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));
            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToOne(entityMetadata, manyToOne, entityRole);

            if (_config.AddDescriptionAttribute)
            {
                string description = comments.FirstOrDefault();

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(comments));

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
            statementCollection.Add(ThisMethodInvoke(methodName, targetType, relationshipNameRef, codeExpression, VarRef(VariableNameValue)));
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

        private CodeTypeDeclarationCollection BuildServiceContext(IEnumerable<EntityMetadata> entityMetadata)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateServiceContext())
            {
                string serviceContextName = iCodeGenerationServiceProvider.NamingService.GetNameForServiceContext();

                var codeTypeDeclaration = this.Class(serviceContextName, ServiceContextBaseType);

                iCodeGenerationServiceProvider.NamingService.SetCurrentTypeName(serviceContextName);

                codeTypeDeclaration.Members.Add(this.ServiceContextConstructor());

                codeTypeDeclaration.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContext()));

                foreach (var entityMetadata1 in entityMetadata.OrderBy(metadata => metadata.LogicalName))
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1))
                    {
                        var codeMemberProperty = this.BuildEntitySetProperty(entityMetadata1);

                        codeTypeDeclaration.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeMemberProperty));
                    }
                }

                declarationCollection.Add(codeTypeDeclaration);
            }

            return declarationCollection;
        }

        private CodeTypeMember ServiceContextConstructor()
        {
            var codeConstructor = this.Constructor(Param(TypeRef(typeof(IOrganizationService)), VariableNameService));

            codeConstructor.BaseConstructorArgs.Add(VarRef(VariableNameService));

            codeConstructor.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForServiceContextConstructor()));

            return codeConstructor;
        }

        private CodeMemberProperty BuildEntitySetProperty(EntityMetadata entity)
        {
            var typeForEntity = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entity);

            var codeMemberProperty = this.BuildClassProperty(IQueryable(typeForEntity), iCodeGenerationServiceProvider.NamingService.GetNameForEntitySet(entity), (CodeStatement)Return(ThisMethodInvoke(MethodCreateQuery, typeForEntity)));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForEntitySet(entity);

            if (_config.AddDescriptionAttribute)
            {
                string description = comments.FirstOrDefault();

                if (!string.IsNullOrEmpty(description))
                {
                    codeMemberProperty.CustomAttributes.Add(Attribute(DescriptionAttribute, AttributeArg(StringLiteral(description))));
                }
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(comments));

            return codeMemberProperty;
        }

        private CodeTypeDeclarationCollection BuildSdkMessagesEnumerable(IEnumerable<CodeGenerationSdkMessage> sdkMessages)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessage in sdkMessages.OrderBy(m => m.Name))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessage(sdkMessage))
                {
                    declarationCollection.AddRange(this.BuildSdkMessage(sdkMessage));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildSdkMessage(CodeGenerationSdkMessage message)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessagePair in message.SdkMessagePairs.Values.OrderBy(p => p.Request.Name))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessagePair(sdkMessagePair))
                {
                    declarationCollection.AddRange(this.BuildSdkMessagePair(sdkMessagePair));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildSdkMessagePair(CodeGenerationSdkMessagePair sdkMessagePair)
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            declarationCollection.Add(this.BuildSdkMessageRequest(sdkMessagePair, sdkMessagePair.Request, out var namespaceRef, out var requestNameRef));
            declarationCollection.Add(this.BuildSdkMessageResponse(sdkMessagePair, sdkMessagePair.Response, namespaceRef, requestNameRef));

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildSdkMessageRequest(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageRequest sdkMessageRequest
            , out CodeExpression sdkMessageNamespaceAttributeRef
            , out CodeExpression sdkMessageRequestNameAttributeRef
        )
        {
            string requestClassName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair), RequestClassSuffix);

            var requestClass = this.Class(requestClassName, RequestClassBaseType);

            iCodeGenerationServiceProvider.NamingService.SetCurrentTypeName(requestClassName);

            requestClass.Members.Add(this.BuildClassConstant(SdkMessageNamespaceFieldName, typeof(string), messagePair.MessageNamespace));
            requestClass.Members.Add(this.BuildClassConstant(SdkMessageRequestNameFieldName, typeof(string), messagePair.Request.Name));

            List<CodeMemberProperty> genericMembers = new List<CodeMemberProperty>();
            List<CodeMemberProperty> allMembers = new List<CodeMemberProperty>();

            var statementsAssignDefaultParameters = new CodeStatementCollection();

            if (sdkMessageRequest.RequestFields != null & sdkMessageRequest.RequestFields.Count > 0)
            {
                foreach (var field in sdkMessageRequest.RequestFields.Values)
                {
                    var requestField = this.BuildRequestFieldProperty(sdkMessageRequest, field);

                    if (requestField.Type.Options == CodeTypeReferenceOptions.GenericTypeParameter)
                    {
                        genericMembers.Add(requestField);
                    }

                    if (!field.Optional.GetValueOrDefault())
                    {
                        statementsAssignDefaultParameters.Add(AssignProp(requestField.Name, new CodeDefaultValueExpression(requestField.Type)));
                    }

                    allMembers.Add(requestField);
                }
            }

            if (genericMembers.Count > 1)
            {
                for (int index = 0; index < genericMembers.Count; index++)
                {
                    genericMembers[index].Type.BaseType += (index + 1).ToString();
                }
            }

            foreach (var member in allMembers)
            {
                requestClass.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(member));
            }

            if (genericMembers.Any())
            {
                var codeParameterDeclarationCollection = new CodeParameterDeclarationExpressionCollection();
                var codeStatementCollection = new CodeStatementCollection();

                for (int index = 0; index < genericMembers.Count; index++)
                {
                    var requestField = genericMembers[index];

                    requestClass.TypeParameters.Add(new CodeTypeParameter(requestField.Type.BaseType)
                    {
                        HasConstructorConstraint = true,
                        Constraints =
                        {
                            new CodeTypeReference(EntityClassBaseType),
                        },
                    });

                    string parameterName = "target" + (genericMembers.Count == 1 ? string.Empty : (index + 1).ToString());

                    codeParameterDeclarationCollection.Add(Param(requestField.Type, parameterName));
                    codeStatementCollection.Add((CodeStatement)AssignProp(requestField.Name, VarRef(parameterName)));
                }

                requestClass.Members.Add(this.Constructor(genericMembers.Select(f => (CodeExpression)New(f.Type)).ToArray()));

                var requestClassRef = new CodeTypeReference(requestClass.Name, genericMembers.Select(f => new CodeTypeReference(EntityClassBaseType)).ToArray());

                sdkMessageNamespaceAttributeRef = FieldRef(requestClassRef, SdkMessageNamespaceFieldName);
                sdkMessageRequestNameAttributeRef = FieldRef(requestClassRef, SdkMessageRequestNameFieldName);

                var codeConstructor = this.Constructor(codeParameterDeclarationCollection);

                codeConstructor.Statements.Add(AssignProp(RequestNamePropertyName, sdkMessageRequestNameAttributeRef));

                codeConstructor.Statements.Add(new CodeSnippetStatement(string.Empty));
                codeConstructor.Statements.AddRange(statementsAssignDefaultParameters);

                codeConstructor.Statements.Add(new CodeSnippetStatement(string.Empty));
                codeConstructor.Statements.AddRange(codeStatementCollection);


                requestClass.Members.Add(codeConstructor);
            }
            else
            {
                sdkMessageNamespaceAttributeRef = FieldRef(requestClass.Name, SdkMessageNamespaceFieldName);
                sdkMessageRequestNameAttributeRef = FieldRef(requestClass.Name, SdkMessageRequestNameFieldName);

                var codeConstructor = this.Constructor();

                codeConstructor.Statements.Add(AssignProp(RequestNamePropertyName, sdkMessageRequestNameAttributeRef));

                codeConstructor.Statements.Add(new CodeSnippetStatement(string.Empty));
                codeConstructor.Statements.AddRange(statementsAssignDefaultParameters);

                requestClass.Members.Add(codeConstructor);
            }

            requestClass.CustomAttributes.Add(Attribute(typeof(DataContractAttribute), AttributeArg(nameof(DataContractAttribute.Namespace), sdkMessageRequestNameAttributeRef)));
            requestClass.CustomAttributes.Add(Attribute(typeof(RequestProxyAttribute), AttributeArg(null, sdkMessageNamespaceAttributeRef)));

            return requestClass;
        }

        private CodeTypeDeclaration BuildSdkMessageResponse(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageResponse sdkMessageResponse
            , CodeExpression sdkMessageNamespaceAttributeRef
            , CodeExpression sdkMessageRequestNameAttributeRef
        )
        {
            string responseClassName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair), ResponseClassSuffix);

            var codeTypeDeclaration = this.Class(
                responseClassName
                , ResponseClassBaseType
                , Attribute(typeof(DataContractAttribute), AttributeArg(nameof(DataContractAttribute.Namespace), sdkMessageNamespaceAttributeRef))
                , Attribute(typeof(ResponseProxyAttribute), AttributeArg(null, sdkMessageRequestNameAttributeRef))
            );

            iCodeGenerationServiceProvider.NamingService.SetCurrentTypeName(responseClassName);

            codeTypeDeclaration.Members.Add(this.Constructor());

            if (sdkMessageResponse != null && sdkMessageResponse.ResponseFields != null & sdkMessageResponse.ResponseFields.Count > 0)
            {
                foreach (var field in sdkMessageResponse.ResponseFields.Values)
                {
                    var codeMemberProperty = this.BuildResponseFieldProperty(sdkMessageResponse, field);

                    codeTypeDeclaration.Members.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeMemberProperty));
                }
            }

            return codeTypeDeclaration;
        }

        private CodeMemberProperty BuildRequestFieldProperty(CodeGenerationSdkMessageRequest request, Entities.SdkMessageRequestField field)
        {
            var typeForRequestField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRequestField(request, field);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForRequestField(request, field);

            CodeExpression properyNameRef = StringLiteral(propertyName);

            if (_config.GenerateAttributesWithNameOf)
            {
                properyNameRef = NameOfPropertyExpression(propertyName);
            }

            var codeMemberProperty = this.BuildClassProperty(typeForRequestField, propertyName);

            codeMemberProperty.HasSet = true;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(BuildRequestFieldGetStatement(properyNameRef, typeForRequestField));
            codeMemberProperty.SetStatements.Add(BuildRequestFieldSetStatement(properyNameRef));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberProperty;
        }

        private CodeStatement BuildRequestFieldGetStatement(CodeExpression properyNameRef, CodeTypeReference targetType)
        {
            return If(ContainsParameter(properyNameRef), Return(Cast(targetType, PropertyIndexer(ParametersPropertyName, properyNameRef))), Return(new CodeDefaultValueExpression(targetType)));
        }

        private CodeAssignStatement BuildRequestFieldSetStatement(CodeExpression properyNameRef)
        {
            return AssignValue(PropertyIndexer(ParametersPropertyName, properyNameRef));
        }

        private CodeMemberProperty BuildResponseFieldProperty(CodeGenerationSdkMessageResponse response, Entities.SdkMessageResponseField field)
        {
            var forResponseField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForResponseField(field);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForResponseField(response, field);

            CodeExpression properyNameRef = StringLiteral(propertyName);

            if (_config.GenerateAttributesWithNameOf)
            {
                properyNameRef = NameOfPropertyExpression(propertyName);
            }

            var codeMemberProperty = this.BuildClassProperty(forResponseField, propertyName);

            codeMemberProperty.HasSet = _config.MakeAllPropertiesEditable;
            codeMemberProperty.HasGet = true;

            codeMemberProperty.GetStatements.Add(BuildResponseFieldGetStatement(properyNameRef, forResponseField));

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.Add(BuildResponseFieldSetStatement(properyNameRef));
            }

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberProperty.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberProperty;
        }

        private CodeStatement BuildResponseFieldGetStatement(CodeExpression properyNameRef, CodeTypeReference targetType)
        {
            return If(ContainsResult(properyNameRef), Return(Cast(targetType, PropertyIndexer(ResultsPropertyName, properyNameRef))), Return(new CodeDefaultValueExpression(targetType)));
        }

        private CodeAssignStatement BuildResponseFieldSetStatement(CodeExpression properyNameRef)
        {
            return AssignValue(PropertyIndexer(ResultsPropertyName, properyNameRef));
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

        private static CodeTypeReference TypeRef(string typeName)
        {
            return new CodeTypeReference(typeName);
        }

        private static CodeTypeReference TypeRef(Type type)
        {
            return new CodeTypeReference(type);
        }

        private static CodeTypeReference TypeRef(Type type, CodeTypeReference typeParameter)
        {
            return new CodeTypeReference(type.FullName, new CodeTypeReference[1]
            {
                typeParameter
            });
        }

        private static CodeAttributeDeclaration Attribute(Type type, params CodeAttributeArgument[] args)
        {
            return new CodeAttributeDeclaration(TypeRef(type), args);
        }

        private CodeExpression BuildAttributeNameRef(EntityMetadata entityMetadata, string attributePropertyName, string attributeLogicalName)
        {
            if (this._config.UseSchemaConstInCSharpAttributes)
            {
                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata);

                var className = typeRef.BaseType + ".Schema.Attributes";

                CodeDomProvider provider = CodeDomProvider.CreateProvider(CSharpLanguage);

                attributePropertyName = attributePropertyName.ToLower();

                if (!provider.IsValidIdentifier(attributePropertyName))
                {
                    attributePropertyName = "@" + attributePropertyName;
                }

                return FieldRef(className, attributeLogicalName);
            }

            return (CodeExpression)StringLiteral(attributeLogicalName);
        }

        private CodeExpression BuildRelationshipNameRef(EntityMetadata entityMetadata, string relationshipTypeName, string schemaName)
        {
            if (this._config.UseSchemaConstInCSharpAttributes)
            {
                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata);

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

        private CodeMemberProperty BuildClassProperty(
            CodeTypeReference typeRef
            , string propertyName
            , params CodeStatement[] stmts
        )
        {
            var codeMemberProperty = new CodeMemberProperty
            {
                Type = typeRef,
                Name = propertyName,
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

            return codeMemberEvent;
        }

        private CodeMemberMethod RaiseEvent(
            string methodName
            , string eventName
            , Type eventArgsType
        )
        {
            var codeMemberMethod = new CodeMemberMethod
            {
                Name = methodName
            };

            codeMemberMethod.Parameters.Add(Param(TypeRef(typeof(string)), VariableNamePropertyName));

            var referenceExpression = new CodeEventReferenceExpression(This(), eventName);
            codeMemberMethod.Statements.Add(If(IsNotNull(referenceExpression), new CodeDelegateInvokeExpression(referenceExpression, new CodeExpression[2]
            {
                This(),
                New(TypeRef(eventArgsType), (CodeExpression) VarRef(VariableNamePropertyName))
            })));

            if (this._config.GenerateWithDebuggerNonUserCode)
            {
                codeMemberMethod.CustomAttributes.Add(Attribute(DebuggerNonUserCodeAttribute));
            }

            return codeMemberMethod;
        }

        private static CodeMethodInvokeExpression ContainsParameter(CodeExpression properyNameRef)
        {
            return new CodeMethodInvokeExpression(ThisProp(ParametersPropertyName), nameof(ParameterCollection.Contains), new CodeExpression[1]
            {
                properyNameRef
            });
        }

        private static CodeMethodInvokeExpression ContainsResult(CodeExpression properyNameRef)
        {
            return new CodeMethodInvokeExpression(ThisProp(ResultsPropertyName), nameof(ParameterCollection.Contains), new CodeExpression[1]
            {
                properyNameRef
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

        private static CodeConditionStatement If(CodeExpression condition, CodeExpression[] trueCodes)
        {
            return If(condition, trueCodes.Select(s => new CodeExpressionStatement(s)).ToArray(), null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeExpression[] trueCodes
            , CodeExpression[] falseCodes
        )
        {
            return If(condition, trueCodes.Select(s => new CodeExpressionStatement(s)).ToArray(), trueCodes.Select(s => new CodeExpressionStatement(s)).ToArray());
        }

        private static CodeConditionStatement If(CodeExpression condition, CodeStatement[] trueStatements)
        {
            return If(condition, trueStatements, null);
        }

        private static CodeConditionStatement If(
            CodeExpression condition
            , CodeStatement[] trueStatements
            , CodeStatement[] falseStatements
        )
        {
            var conditionStatement = new CodeConditionStatement(condition);

            if (trueStatements != null)
            {
                conditionStatement.TrueStatements.AddRange(trueStatements);
            }

            if (falseStatements != null)
            {
                conditionStatement.FalseStatements.AddRange(falseStatements);
            }

            return conditionStatement;
        }

        private static CodeFieldReferenceExpression FieldRef(CodeTypeReference targetType, string fieldName)
        {
            return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(targetType), fieldName);
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

        private static CodeVariableDeclarationStatement Var(
            CodeTypeReference typeRef
            , string name
            , CodeExpression init
        )
        {
            return new CodeVariableDeclarationStatement(typeRef, name, init);
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

        private CodeConstructor Constructor(CodeParameterDeclarationExpressionCollection args, CodeStatementCollection statements = null)
        {
            var codeConstructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };

            if (args != null)
            {
                codeConstructor.Parameters.AddRange(args);
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
            return AssignValue(target, new CodeVariableReferenceExpression(VariableNameValue));
        }

        private static CodeAssignStatement AssignValue(CodeExpression target, CodeExpression value)
        {
            return new CodeAssignStatement(target, value);
        }

        private static CodePropertyReferenceExpression BaseProp(string propertyName)
        {
            return new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), propertyName);
        }

        private CodeIndexerExpression PropertyIndexer(string propertyName, CodeExpression properyNameRef)
        {
            return new CodeIndexerExpression(ThisProp(propertyName), new CodeExpression[1]
            {
                properyNameRef
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

        private static CodeMethodInvokeExpression VarMethodInvoke(string varName, string methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(VarRef(varName), methodName), parameters);
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

        private static CodeMethodInvokeExpression StaticMethodInvoke(
            Type targetObject
            , string methodName
            , CodeTypeReference[] typeParameters
            , params CodeExpression[] parameters
        )
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(targetObject), methodName, typeParameters), parameters);
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

        private static CodeSnippetExpression NameOfPropertyExpression(string propertyName)
        {
            return new CodeSnippetExpression($"nameof({propertyName})");
        }

        private static CodePrimitiveExpression StringLiteral(string value)
        {
            return new CodePrimitiveExpression(value);
        }

        private static CodeMethodReturnStatement Return()
        {
            return new CodeMethodReturnStatement();
        }

        private static CodeMethodReturnStatement Return(CodeExpression expression)
        {
            return new CodeMethodReturnStatement(expression);
        }

        private static CodeDefaultValueExpression DefaultTypeValue(CodeTypeReference typeRef)
        {
            return new CodeDefaultValueExpression(typeRef);
        }

        private static CodeMethodInvokeExpression EnumIsDefined(CodeTypeReference type, string variableName)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(typeof(Enum))), nameof(System.Enum.IsDefined), new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression(VarRef(variableName), nameof(Nullable<int>.Value))
            });
        }

        private static CodeMethodInvokeExpression ConvertEnum(CodeTypeReference type, string variableName)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(typeof(Enum))), nameof(System.Enum.ToObject), new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression(VarRef(variableName), nameof(Nullable<int>.Value))
            });
        }

        private static CodeExpression IsValueNull()
        {
            return new CodeBinaryOperatorExpression(VarRef(VariableNameValue), CodeBinaryOperatorType.IdentityEquality, Null());
        }

        private static CodePrimitiveExpression Null()
        {
            return new CodePrimitiveExpression(null);
        }

        private static CodeBinaryOperatorExpression IsNotNull(CodeExpression expression)
        {
            return new CodeBinaryOperatorExpression(expression, CodeBinaryOperatorType.IdentityInequality, Null());
        }

        private static CodeBinaryOperatorExpression IsExpressionNull(CodeExpression expression)
        {
            return new CodeBinaryOperatorExpression(expression, CodeBinaryOperatorType.IdentityEquality, Null());
        }

        private static CodeExpression GuidEmpty()
        {
            return PropRef(new CodeTypeReferenceExpression(typeof(Guid)), nameof(Guid.Empty));
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

        private static CodeBinaryOperatorExpression NotEqual(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.IdentityInequality, right);
        }

        private static CodeBinaryOperatorExpression And(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanAnd, right);
        }
    }
}