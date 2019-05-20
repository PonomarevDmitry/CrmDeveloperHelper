using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
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
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
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

        private readonly CreateFileCSharpConfiguration _config;

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
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => this.WriteEntitiesFile(entities, optionSets, messages, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        private void WriteEntitiesFile(
            IEnumerable<EntityMetadata> entities
            , IEnumerable<OptionSetMetadata> optionSets
            , IEnumerable<CodeGenerationSdkMessage> messages
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildEntities(entities, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildGlobalOptionSets(optionSets, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildSdkMessagesEnumerable(messages, iCodeGenerationServiceProvider));

            codeNamespace.Types.AddRange(this.BuildServiceContext(entities, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            codeCompileUnit.AssemblyCustomAttributes.Add(Attribute(typeof(ProxyTypesAssemblyAttribute)));

            options.IndentString = _config.TabSpacer;

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public Task WriteEntityFileAsync(
            EntityMetadata entityMetadata
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => this.WriteEntityFile(entityMetadata, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        private void WriteEntityFile(
            EntityMetadata entityMetadata
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

            options.IndentString = _config.TabSpacer;

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public Task WriteSdkMessageAsync(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => this.WriteSdkMessage(sdkMessage, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        private void WriteSdkMessage(
            CodeGenerationSdkMessage sdkMessage
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildSdkMessage(sdkMessage, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            options.IndentString = _config.TabSpacer;

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public Task WriteSdkMessagePairAsync(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return Task.Run(() => this.WriteSdkMessagePair(sdkMessagePair, outputFilePath, outputNamespace, options, iCodeGenerationServiceProvider));
        }

        private void WriteSdkMessagePair(
            CodeGenerationSdkMessagePair sdkMessagePair
            , string outputFilePath
            , string outputNamespace
            , CodeGeneratorOptions options
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeNamespace = Namespace(outputNamespace);

            codeNamespace.Types.AddRange(this.BuildSdkMessagePair(sdkMessagePair, iCodeGenerationServiceProvider));

            var codeCompileUnit = new CodeCompileUnit();

            codeCompileUnit.Namespaces.Add(codeNamespace);

            options.IndentString = _config.TabSpacer;

            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, options);
                }
            }
        }

        public CodeGenerationType GetTypeForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Enum;
        }

        public CodeGenerationType GetTypeForOption(
            OptionSetMetadata optionSetMetadata
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
            , Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        public CodeGenerationType GetTypeForResponseField(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return CodeGenerationType.Property;
        }

        private CodeTypeDeclarationCollection BuildGlobalOptionSets(
            IEnumerable<OptionSetMetadata> optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var OptionSetMetadata in optionSetMetadata)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(OptionSetMetadata, null, iCodeGenerationServiceProvider)
                    && OptionSetMetadata.IsGlobal.HasValue
                    && OptionSetMetadata.IsGlobal.Value
                )
                {
                    var codeTypeDeclaration = this.BuildOptionSet(null, OptionSetMetadata, iCodeGenerationServiceProvider);
                    if (codeTypeDeclaration != null)
                    {
                        declarationCollection.Add(codeTypeDeclaration);
                    }
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadata optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeTypeDeclaration = this.Enum(iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entityMetadata, optionSetMetadata, iCodeGenerationServiceProvider), Attribute(typeof(DataContractAttribute)));

            codeTypeDeclaration.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForOptionSet(entityMetadata, optionSetMetadata, iCodeGenerationServiceProvider)));

            foreach (var option in optionSetMetadata.Options)
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOption(option, iCodeGenerationServiceProvider))
                {
                    codeTypeDeclaration.Members.Add(this.BuildOption(optionSetMetadata, option, iCodeGenerationServiceProvider));
                }
            }

            return codeTypeDeclaration;
        }

        private CodeTypeMember BuildOption(
            OptionSetMetadata optionSetMetadata
            , OptionMetadata option
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeMemberField = this.Field(iCodeGenerationServiceProvider.NamingService.GetNameForOption(optionSetMetadata, option, iCodeGenerationServiceProvider), typeof(int), option.Value.Value, Attribute(typeof(EnumMemberAttribute)));

            codeMemberField.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForOption(optionSetMetadata, option, iCodeGenerationServiceProvider)));

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
                CodeRegionDirective startCodeRegionDirective = null;
                string regionName = "Primary Attributes";

                if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata.PrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase));

                    if (attributeMetadata != null
                        && attributeMetadata.IsPrimaryId.GetValueOrDefault()
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider)
                    )
                    {
                        var attributeMember = this.BuildIdProperty(entityMetadata, attributeMetadata, primatyIdAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startCodeRegionDirective == null)
                        {
                            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                            {
                                StartDirectives =
                                {
                                    startCodeRegionDirective,
                                },
                            });
                        }
                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));

                        attributeMember = this.BuildAttributePropertyOnBaseType(entityMetadata, attributeMetadata, primatyIdAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startCodeRegionDirective == null)
                        {
                            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                            {
                                StartDirectives =
                                {
                                    startCodeRegionDirective,
                                },
                            });
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
                        var attributeMember = this.BuildAttributePropertyOnBaseType(entityMetadata, attributeMetadata, primatyNameAttributeRef, iCodeGenerationServiceProvider);

                        if (attributeMember != null && startCodeRegionDirective == null)
                        {
                            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                            {
                                StartDirectives =
                                {
                                    startCodeRegionDirective,
                                },
                            });
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                    }
                }

                if (startCodeRegionDirective != null)
                {
                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                    {
                        EndDirectives =
                        {
                            new CodeRegionDirective(CodeRegionMode.End, regionName),
                        },
                    });
                }
            }

            {
                string regionName = "Attributes";

                CodeRegionDirective startCodeRegionDirective = null;

                foreach (var attributeMetadata in entityMetadata.Attributes.OrderBy(metadata => metadata.LogicalName))
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateAttribute(attributeMetadata, iCodeGenerationServiceProvider)
                        && !string.Equals(entityMetadata.PrimaryIdAttribute, attributeMetadata.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                        && !string.Equals(entityMetadata.PrimaryNameAttribute, attributeMetadata.LogicalName, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var attributeOptionSet = TypeMappingService.GetAttributeOptionSet(attributeMetadata);

                        if (attributeOptionSet != null
                            && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(attributeOptionSet, attributeMetadata, iCodeGenerationServiceProvider)
                        )
                        {
                            CodeTypeDeclaration codeTypeDeclarationOptionSet = this.BuildOptionSet(entityMetadata, attributeOptionSet, iCodeGenerationServiceProvider);
                            if (codeTypeDeclarationOptionSet != null)
                            {
                                declarationCollection.Add(codeTypeDeclarationOptionSet);
                            }
                        }

                        string attributePropertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);
                        CodeExpression attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributePropertyName, attributeMetadata.LogicalName, iCodeGenerationServiceProvider);

                        ProxyClassAttributeEnums proxyClassAttributeEnums = GetProxyClassAttributeEnums(attributeMetadata, attributeOptionSet);
                        ProxyClassAttributeEnumsGlobalOptionSetLocation proxyClassAttributeEnumsGlobalOptionSetLocation = GetProxyClassAttributeEnumsGlobalOptionSetLocation(attributeMetadata, attributeOptionSet);

                        if (attributeOptionSet == null
                            || (attributeOptionSet != null && proxyClassAttributeEnums != Model.ProxyClassAttributeEnums.InsteadOfOptionSet)
                        )
                        {
                            CodeMemberProperty attributeMember = this.BuildAttributePropertyOnBaseType(
                                entityMetadata
                                , attributeMetadata
                                , attributeNameRef
                                , iCodeGenerationServiceProvider
                            );

                            if (attributeMember != null)
                            {
                                if (startCodeRegionDirective == null)
                                {
                                    startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                    {
                                        StartDirectives =
                                        {
                                            startCodeRegionDirective,
                                        },
                                    });
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                            }
                        }

                        if (attributeOptionSet != null
                            && !iCodeGenerationServiceProvider.CodeWriterFilterService.IgnoreOptionSet(attributeOptionSet, attributeMetadata, iCodeGenerationServiceProvider)
                            && proxyClassAttributeEnums != Model.ProxyClassAttributeEnums.NotNeeded
                        )
                        {
                            string baseAttributeName = string.Empty;

                            if (proxyClassAttributeEnums == Model.ProxyClassAttributeEnums.AddWithNameAttributeEnum)
                            {
                                baseAttributeName = attributePropertyName;
                                attributePropertyName += "Enum";
                            }

                            string enumTypeName = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForOptionSet(entityMetadata, attributeOptionSet, iCodeGenerationServiceProvider).BaseType;

                            if (proxyClassAttributeEnumsGlobalOptionSetLocation == Model.ProxyClassAttributeEnumsGlobalOptionSetLocation.InClassSchema)
                            {
                                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata, iCodeGenerationServiceProvider);

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
                            else if (proxyClassAttributeEnumsGlobalOptionSetLocation == Model.ProxyClassAttributeEnumsGlobalOptionSetLocation.InGlobalOptionSetNamespace)
                            {
                                enumTypeName = attributeOptionSet.Name.ToLower();

                                if (!string.IsNullOrEmpty(_config.NamespaceGlobalOptionSets))
                                {
                                    enumTypeName = $"{_config.NamespaceGlobalOptionSets}.{enumTypeName}";
                                }
                            }

                            CodeMemberProperty attributeMember = this.BuildAttributePropertyOnEnum(
                                entityMetadata
                                , attributeMetadata
                                , attributePropertyName
                                , baseAttributeName
                                , attributeNameRef
                                , enumTypeName
                                , iCodeGenerationServiceProvider
                            );

                            if (attributeMember != null)
                            {
                                if (startCodeRegionDirective == null)
                                {
                                    startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                    {
                                        StartDirectives =
                                        {
                                            startCodeRegionDirective,
                                        },
                                    });
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(attributeMember));
                            }
                        }
                    }
                }

                if (startCodeRegionDirective != null)
                {
                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                    {
                        EndDirectives =
                        {
                            new CodeRegionDirective(CodeRegionMode.End, regionName),
                        },
                    });
                }
            }

            return memberCollection;
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

        private void InitializeEntityClass(
            CodeTypeDeclaration entityClass
            , EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanging)));
            entityClass.BaseTypes.Add(TypeRef(typeof(INotifyPropertyChanged)));

            entityClass.Members.Add(this.BuildClassConstant("EntityLogicalName", typeof(string), entityMetadata.LogicalName));

            entityClass.Members.Add(this.BuildClassConstant("EntitySchemaName", typeof(string), entityMetadata.SchemaName));

            if (entityMetadata.ObjectTypeCode.HasValue && !entityMetadata.IsCustomEntity.GetValueOrDefault())
            {
                entityClass.Members.Add(this.BuildClassConstant("EntityTypeCode", typeof(int), entityMetadata.ObjectTypeCode));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryIdAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant("EntityPrimaryIdAttribute", typeof(string), entityMetadata.PrimaryIdAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryNameAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant("EntityPrimaryNameAttribute", typeof(string), entityMetadata.PrimaryNameAttribute));
            }

            if (!string.IsNullOrEmpty(entityMetadata.PrimaryImageAttribute))
            {
                entityClass.Members.Add(this.BuildClassConstant("EntityPrimaryImageAttribute", typeof(string), entityMetadata.PrimaryImageAttribute));
            }

            entityClass.Members.Add(this.EntityConstructorDefault(entityMetadata, iCodeGenerationServiceProvider));

            if (_config.AddConstructorWithAnonymousTypeObject)
            {
                entityClass.Members.Add(this.EntityConstructorAnonymousObject(entityMetadata, iCodeGenerationServiceProvider));
            }

            string regionName = "NotifyProperty Events";

            entityClass.Members.Add(new CodeSnippetTypeMember(string.Empty)
            {
                StartDirectives =
                {
                    new CodeRegionDirective(CodeRegionMode.Start, regionName),
                },
            });
            
            entityClass.Members.Add(this.Event("PropertyChanged", typeof(PropertyChangedEventHandler), typeof(INotifyPropertyChanged)));
            entityClass.Members.Add(this.Event("PropertyChanging", typeof(PropertyChangingEventHandler), typeof(INotifyPropertyChanging)));
            entityClass.Members.Add(this.RaiseEvent("OnPropertyChanged", "PropertyChanged", typeof(PropertyChangedEventArgs)));
            entityClass.Members.Add(this.RaiseEvent("OnPropertyChanging", "PropertyChanging", typeof(PropertyChangingEventArgs)));

            entityClass.Members.Add(new CodeSnippetTypeMember(string.Empty)
            {
                EndDirectives =
                {
                    new CodeRegionDirective(CodeRegionMode.End, regionName),
                },
            });
        }

        private CodeTypeMember BuildClassConstant(string constName, Type type, object value)
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

        private CodeMemberProperty BuildAttributePropertyOnBaseType(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , CodeExpression attributeNameRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            CodeTypeReference forAttributeType = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForAttributeType(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttributeAsEntityProperty(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);

            var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, propertyName, attributeNameRef, forAttributeType, iCodeGenerationServiceProvider);

            if (codeMemberProperty.HasGet)
            {
                codeMemberProperty.GetStatements.AddRange(this.BuildAttributeGet(attributeMetadata, forAttributeType, attributeNameRef));
            }

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildAttributeSet(entityMetadata, attributeMetadata, codeMemberProperty.Name, attributeNameRef));
            }

            return codeMemberProperty;
        }

        private CodeMemberProperty BuildAttributePropertyOnEnum(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , string propertyName
            , string basePropertyName
            , CodeExpression attributeNameRef
            , string enumTypeName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var enumTypeRef = TypeRef(enumTypeName);

            if (attributeMetadata is MultiSelectPicklistAttributeMetadata)
            {
                CodeTypeReference forAttributeType = IEnumerable(enumTypeRef);

                var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, propertyName, attributeNameRef, forAttributeType, iCodeGenerationServiceProvider);

                if (codeMemberProperty.HasGet)
                {
                    codeMemberProperty.GetStatements.AddRange(this.BuildMultiEnumAttributeGet(attributeNameRef, enumTypeRef));
                }

                if (codeMemberProperty.HasSet)
                {
                    codeMemberProperty.SetStatements.AddRange(this.BuildMultiEnumAttributeSet(attributeNameRef, propertyName, basePropertyName, enumTypeRef));
                }

                return codeMemberProperty;
            }
            else
            {
                CodeTypeReference forAttributeType = TypeRef(typeof(Nullable<>), enumTypeRef);

                var codeMemberProperty = BuildAttributePropertyCommon(entityMetadata, attributeMetadata, propertyName, attributeNameRef, forAttributeType, iCodeGenerationServiceProvider);

                if (codeMemberProperty.HasGet)
                {
                    codeMemberProperty.GetStatements.AddRange(this.BuildEnumAttributeGet(attributeNameRef, enumTypeRef));
                }

                if (codeMemberProperty.HasSet)
                {
                    codeMemberProperty.SetStatements.AddRange(this.BuildEnumAttributeSet(attributeNameRef, propertyName, basePropertyName));
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
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
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
            if (attributeMetadata.AttributeTypeName != null
                && attributeMetadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType
            )
            {
                statementCollection.AddRange(BuildAttributeGetNotNull(attributeNameRef, targetType));
            }
            else
            {
                statementCollection.Add(Return(ThisMethodInvoke(nameof(Entity.GetAttributeValue), targetType, attributeNameRef)));
            }

            return statementCollection;
        }

        private static CodeStatementCollection BuildAttributeGetNotNull(
            CodeExpression attributeNameRef
            , CodeTypeReference propertyType
        )
        {
            return new CodeStatementCollection()
            {
                Var(propertyType, "collection",  ThisMethodInvoke(nameof(Entity.GetAttributeValue), propertyType, attributeNameRef)),
                If(ExpressionNull(VarRef("collection")), new CodeStatement[]
                {
                    AssignValue(VarRef("collection"), New(propertyType)),
                    new CodeExpressionStatement( ThisMethodInvoke("SetAttributeValue", attributeNameRef, VarRef("collection"))),
                }),
                Return(VarRef("collection")),
            };
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
                statementCollection.Add(If(PropRef(VarRef("value"), nameof(Nullable<int>.HasValue)), AssignValue(BaseProp("Id"), PropRef(VarRef("value"), "Value")), AssignValue(BaseProp("Id"), GuidEmpty())));
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
                Var(typeof (EntityCollection), "collection", ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(EntityCollection)), attributeNameRef)),
                If(And(NotNull(VarRef("collection")), NotNull(PropRef(VarRef("collection"), "Entities"))), Return(StaticMethodInvoke(typeof(Enumerable), "Cast", propertyType.TypeArguments[0], (CodeExpression)PropRef(VarRef("collection"), "Entities"))), Return(Null()))
            };
        }

        private static CodeStatement BuildEntityCollectionAttributeSet(CodeExpression attributeNameRef)
        {
            return If(ValueNull()
                , ThisMethodInvoke("SetAttributeValue", attributeNameRef, VarRef("value"))
                , ThisMethodInvoke("SetAttributeValue", attributeNameRef
                    , New(TypeRef(typeof(EntityCollection)), (CodeExpression)New(TypeRef(typeof(List<Entity>)), (CodeExpression)VarRef("value")))
            ));
        }

        private CodeMemberProperty BuildIdProperty(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , CodeExpression attributeNameRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeMemberProperty = this.BuildClassProperty(TypeRef(typeof(Guid)), "Id");

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

            codeMemberProperty.GetStatements.Add(Return(BaseProp("Id")));

            if (codeMemberProperty.HasSet)
            {
                codeMemberProperty.SetStatements.Add(AssignValue(ThisProp(iCodeGenerationServiceProvider.NamingService.GetNameForAttributeAsEntityProperty(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)), VarRef("value")));
            }
            else
            {
                codeMemberProperty.SetStatements.Add(AssignValue(BaseProp("Id"), VarRef("value")));
            }

            codeMemberProperty.Comments.AddRange(CommentSummary(iCodeGenerationServiceProvider.NamingService.GetCommentsForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider)));

            return codeMemberProperty;
        }

        private CodeStatementCollection BuildEnumAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference enumTypeRef
        )
        {
            return new CodeStatementCollection(new CodeStatement[2]
            {
                Var(typeof(OptionSetValue), "optionSetValue", ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(OptionSetValue)), attributeNameRef)),
                If(And(NotNull(VarRef("optionSetValue")), EnumIsDefined(enumTypeRef, "optionSetValue"))
                    , Return(Cast(enumTypeRef, ConvertEnum(enumTypeRef, "optionSetValue")))
                    , Return(Null())
                ),
            });
        }

        private CodeStatementCollection BuildEnumAttributeSet(
            CodeExpression attributeNameRef
            , string propertyName
            , string baseAttributeName
        )
        {
            var result = new CodeStatementCollection()
            {
                this.InvokeOnPropertyChanging(propertyName),
            };

            if (!string.IsNullOrEmpty(baseAttributeName))
            {
                result.Add(this.InvokeOnPropertyChanging(baseAttributeName));
            }

            result.Add(If(ValueNull(), ThisMethodInvoke("SetAttributeValue", attributeNameRef, Null()), ThisMethodInvoke("SetAttributeValue", attributeNameRef, New(TypeRef(typeof(OptionSetValue)), (CodeExpression)Cast(TypeRef(typeof(int)), VarRef("value"))))));

            if (!string.IsNullOrEmpty(baseAttributeName))
            {
                result.Add(this.InvokeOnPropertyChanged(baseAttributeName));
            }

            result.Add(this.InvokeOnPropertyChanged(propertyName));

            return result;
        }

        private CodeStatementCollection BuildMultiEnumAttributeGet(
            CodeExpression attributeNameRef
            , CodeTypeReference enumTypeRef
        )
        {
            var listType = TypeRef(typeof(List<>), enumTypeRef);

            return new CodeStatementCollection(new CodeStatement[]
            {
                Var(typeof(OptionSetValueCollection), "optionSetValueCollection", ThisMethodInvoke(nameof(Entity.GetAttributeValue), TypeRef(typeof(OptionSetValueCollection)), attributeNameRef)),

                If(NotNull(VarRef("optionSetValueCollection"))

                    , Return(New(TypeRef(typeof(List<>), enumTypeRef)
                        , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Distinct), enumTypeRef
                            , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Select), new[] { TypeRef(typeof(OptionSetValue)), enumTypeRef }
                                , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Where), TypeRef(typeof(OptionSetValue))
                                    , VarRef("optionSetValueCollection")
                                    , new CodeSnippetExpression($"o => o != null && System.Enum.IsDefined(typeof({enumTypeRef.BaseType}), o.Value)")
                                )
                                , new CodeSnippetExpression($"o => ({enumTypeRef.BaseType})(System.Enum.ToObject(typeof({enumTypeRef.BaseType}), o.Value))")
                            )
                        )
                    )),

                    Return(StaticMethodInvoke(typeof(Enumerable), "Empty", enumTypeRef))
                ),
            });
        }

        private CodeStatementCollection BuildMultiEnumAttributeSet(
            CodeExpression attributeNameRef
            , string propertyName
            , string baseAttributeName
            , CodeTypeReference enumTypeRef
        )
        {
            var result = new CodeStatementCollection()
            {
                this.InvokeOnPropertyChanging(propertyName),
            };

            if (!string.IsNullOrEmpty(baseAttributeName))
            {
                result.Add(this.InvokeOnPropertyChanging(baseAttributeName));
            }

            result.Add(If(Or(ValueNull(), Equal(StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Any), enumTypeRef, VarRef("value")), False()))
                , ThisMethodInvoke("SetAttributeValue", attributeNameRef, Null())

                , ThisMethodInvoke("SetAttributeValue", attributeNameRef
                    , New(TypeRef(typeof(OptionSetValueCollection))
                        , New(TypeRef(typeof(List<OptionSetValue>))
                            , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Select), new[] { enumTypeRef, TypeRef(typeof(OptionSetValue)) }
                                , StaticMethodInvoke(typeof(Enumerable), nameof(Enumerable.Distinct), enumTypeRef
                                    , VarRef("value")
                                )
                                , new CodeSnippetExpression($"o => new Microsoft.Xrm.Sdk.OptionSetValue((int)o)")
                            )
                        )
                    )
                )
            ));

            if (!string.IsNullOrEmpty(baseAttributeName))
            {
                result.Add(this.InvokeOnPropertyChanged(baseAttributeName));
            }

            result.Add(this.InvokeOnPropertyChanged(propertyName));

            return result;
        }

        private CodeTypeMemberCollection BuildOneToManyRelationships(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var memberCollection = new CodeTypeMemberCollection();
            if (entityMetadata.OneToManyRelationships == null)
            {
                return memberCollection;
            }

            const string regionName = "OneToMany Relationships";

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var oneToMany in entityMetadata.OneToManyRelationships.OfType<OneToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntityMetadata = GetEntityMetadata(oneToMany.ReferencingEntity, iCodeGenerationServiceProvider);
                if (otherEntityMetadata != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntityMetadata, iCodeGenerationServiceProvider)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(oneToMany, otherEntityMetadata, CodeGenerationRelationshipType.OneToManyRelationship, iCodeGenerationServiceProvider)
                )
                {
                    CodeMemberProperty codeTypeMember = this.BuildOneToManyProperty(entityMetadata, otherEntityMetadata, oneToMany, iCodeGenerationServiceProvider);

                    if (codeTypeMember != null)
                    {
                        if (startCodeRegionDirective == null)
                        {
                            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                            {
                                StartDirectives =
                                {
                                    startCodeRegionDirective,
                                },
                            });
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(codeTypeMember));
                    }

                    if (string.Equals(oneToMany.SchemaName, "calendar_calendar_rules", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(oneToMany.SchemaName, "service_calendar_rules", StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        var calendarRuleProperty = this.BuildCalendarRuleProperty(entityMetadata, otherEntityMetadata, oneToMany, iCodeGenerationServiceProvider);

                        if (calendarRuleProperty != null)
                        {
                            if (startCodeRegionDirective == null)
                            {
                                startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                {
                                    StartDirectives =
                                    {
                                        startCodeRegionDirective,
                                    },
                                });
                            }

                            memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(calendarRuleProperty));
                        }
                    }
                }
            }

            if (startCodeRegionDirective != null)
            {
                memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                {
                    EndDirectives =
                    {
                        new CodeRegionDirective(CodeRegionMode.End, regionName),
                    },
                });
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
            var codeMemberProperty = this.BuildClassProperty(IEnumerable(iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntity, iCodeGenerationServiceProvider)), "CalendarRules");

            var nullable = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var attributeNameRef = StringLiteral("calendarrules");

            codeMemberProperty.GetStatements.AddRange(BuildEntityCollectionAttributeGet(attributeNameRef, codeMemberProperty.Type));

            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanging(codeMemberProperty.Name));
            codeMemberProperty.SetStatements.Add(BuildEntityCollectionAttributeSet(attributeNameRef));
            codeMemberProperty.SetStatements.Add(this.InvokeOnPropertyChanged(codeMemberProperty.Name));

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, nullable, iCodeGenerationServiceProvider);

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
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForRelationship = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRelationship(oneToMany, otherEntityMetadata, iCodeGenerationServiceProvider);

            var entityRole = oneToMany.ReferencingEntity == entityMetadata.LogicalName ? new EntityRole?(EntityRole.Referenced) : new EntityRole?();

            var codeMemberProperty = this.BuildClassProperty(IEnumerable(typeForRelationship), iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, oneToMany, entityRole, iCodeGenerationServiceProvider));

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "OneToMany", oneToMany.SchemaName, iCodeGenerationServiceProvider);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntities", relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntities", relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipOneToMany(entityMetadata, oneToMany, entityRole, iCodeGenerationServiceProvider);

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

        private CodeTypeMemberCollection BuildManyToManyRelationships(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var memberCollection = new CodeTypeMemberCollection();

            if (entityMetadata.ManyToManyRelationships == null)
            {
                return memberCollection;
            }

            const string regionName = "ManyToMany Relationships";

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var manyToMany in entityMetadata.ManyToManyRelationships.OfType<ManyToManyRelationshipMetadata>().OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntityMetadata = GetEntityMetadata(entityMetadata.LogicalName != manyToMany.Entity1LogicalName ? manyToMany.Entity1LogicalName : manyToMany.Entity2LogicalName, iCodeGenerationServiceProvider);
                if (otherEntityMetadata != null)
                {
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntityMetadata, iCodeGenerationServiceProvider)
                        && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToMany, otherEntityMetadata, CodeGenerationRelationshipType.ManyToManyRelationship, iCodeGenerationServiceProvider)
                    )
                    {
                        if (otherEntityMetadata.LogicalName != entityMetadata.LogicalName)
                        {
                            var nameForRelationship = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?(), iCodeGenerationServiceProvider);
                            var many = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship, new EntityRole?(), iCodeGenerationServiceProvider);

                            if (many != null)
                            {
                                if (startCodeRegionDirective == null)
                                {
                                    startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                    {
                                        StartDirectives =
                                        {
                                            startCodeRegionDirective,
                                        },
                                    });
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many));
                            }
                        }
                        else
                        {
                            var nameForRelationship1 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);
                            var many1 = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship1, new EntityRole?(EntityRole.Referencing), iCodeGenerationServiceProvider);

                            if (many1 != null)
                            {
                                if (startCodeRegionDirective == null)
                                {
                                    startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                    {
                                        StartDirectives =
                                        {
                                            startCodeRegionDirective,
                                        },
                                    });
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many1));
                            }

                            var nameForRelationship2 = iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToMany, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);
                            var many2 = this.BuildManyToManyProperty(entityMetadata, otherEntityMetadata, manyToMany, nameForRelationship2, new EntityRole?(EntityRole.Referenced), iCodeGenerationServiceProvider);

                            if (many2 != null)
                            {
                                if (startCodeRegionDirective == null)
                                {
                                    startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                                    memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                                    {
                                        StartDirectives =
                                        {
                                            startCodeRegionDirective,
                                        },
                                    });
                                }

                                memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(many2));
                            }
                        }
                    }
                }
            }

            if (startCodeRegionDirective != null)
            {
                memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                {
                    EndDirectives =
                    {
                        new CodeRegionDirective(CodeRegionMode.End, regionName),
                    },
                });
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

            var codeMemberProperty = this.BuildClassProperty(IEnumerable(typeForRelationship), propertyName);

            var relationshipNameRef = this.BuildRelationshipNameRef(entityMetadata, "ManyToMany", manyToMany.SchemaName, iCodeGenerationServiceProvider);

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntities", relationshipNameRef, typeForRelationship, entityRole));
            codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntities", relationshipNameRef, typeForRelationship, propertyName, entityRole));

            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToMany(entityMetadata, manyToMany, entityRole, iCodeGenerationServiceProvider);

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

            CodeRegionDirective startCodeRegionDirective = null;

            foreach (var manyToOne in entityMetadata.ManyToOneRelationships.OrderBy(metadata => metadata.SchemaName))
            {
                var otherEntity = GetEntityMetadata(manyToOne.ReferencedEntity, iCodeGenerationServiceProvider);

                if (otherEntity != null
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(otherEntity, iCodeGenerationServiceProvider)
                    && iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateRelationship(manyToOne, otherEntity, CodeGenerationRelationshipType.ManyToOneRelationship, iCodeGenerationServiceProvider)
                )
                {
                    var one = this.BuildManyToOneProperty(entityMetadata, otherEntity, manyToOne, iCodeGenerationServiceProvider);
                    if (one != null)
                    {
                        if (startCodeRegionDirective == null)
                        {
                            startCodeRegionDirective = new CodeRegionDirective(CodeRegionMode.Start, regionName);

                            memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                            {
                                StartDirectives =
                                {
                                    startCodeRegionDirective,
                                },
                            });
                        }

                        memberCollection.Add(InsertInPropertyDebuggerNonUserCodeAttributeInGetAndSet(one));
                    }
                }
            }

            if (startCodeRegionDirective != null)
            {
                memberCollection.Add(new CodeSnippetTypeMember(string.Empty)
                {
                    EndDirectives =
                    {
                        new CodeRegionDirective(CodeRegionMode.End, regionName),
                    },
                });
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

            var codeMemberProperty = this.BuildClassProperty(typeForRelationship, iCodeGenerationServiceProvider.NamingService.GetNameForRelationship(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider));

            codeMemberProperty.GetStatements.Add(BuildRelationshipGet("GetRelatedEntity", relationshipNameRef, typeForRelationship, entityRole));

            if (_config.MakeAllPropertiesEditable
                || attributeMetadata.IsValidForCreate.GetValueOrDefault()
                || attributeMetadata.IsValidForUpdate.GetValueOrDefault()
            )
            {
                codeMemberProperty.SetStatements.AddRange(this.BuildRelationshipSet("SetRelatedEntity", relationshipNameRef, typeForRelationship, codeMemberProperty.Name, entityRole));
            }

            string attributePropertyName = iCodeGenerationServiceProvider.NamingService.GetNameForAttribute(entityMetadata, attributeMetadata, iCodeGenerationServiceProvider);

            var attributeNameRef = this.BuildAttributeNameRef(entityMetadata, attributePropertyName, attributeMetadata.LogicalName, iCodeGenerationServiceProvider);

            codeMemberProperty.CustomAttributes.Add(Attribute(AttributeLogicalNameAttribute, AttributeArg(attributeNameRef)));
            codeMemberProperty.CustomAttributes.Add(BuildRelationshipSchemaNameAttribute(relationshipNameRef, entityRole));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForRelationshipManyToOne(entityMetadata, manyToOne, entityRole, iCodeGenerationServiceProvider);

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
                    if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata1, iCodeGenerationServiceProvider))
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

            var codeMemberProperty = this.BuildClassProperty(IQueryable(typeForEntity), iCodeGenerationServiceProvider.NamingService.GetNameForEntitySet(entity, iCodeGenerationServiceProvider), (CodeStatement)Return(ThisMethodInvoke("CreateQuery", typeForEntity)));

            var comments = iCodeGenerationServiceProvider.NamingService.GetCommentsForEntitySet(entity, iCodeGenerationServiceProvider);

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

        private CodeTypeDeclarationCollection BuildSdkMessagesEnumerable(
            IEnumerable<CodeGenerationSdkMessage> sdkMessages
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessage in sdkMessages.OrderBy(m => m.Name))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessage(sdkMessage, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(this.BuildSdkMessage(sdkMessage, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildSdkMessage(
            CodeGenerationSdkMessage message
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            foreach (var sdkMessagePair in message.SdkMessagePairs.Values.OrderBy(p => p.Request.Name))
            {
                if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateSdkMessagePair(sdkMessagePair, iCodeGenerationServiceProvider))
                {
                    declarationCollection.AddRange(this.BuildSdkMessagePair(sdkMessagePair, iCodeGenerationServiceProvider));
                }
            }

            return declarationCollection;
        }

        private CodeTypeDeclarationCollection BuildSdkMessagePair(
            CodeGenerationSdkMessagePair sdkMessagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var declarationCollection = new CodeTypeDeclarationCollection();

            declarationCollection.Add(this.BuildSdkMessageRequest(sdkMessagePair, sdkMessagePair.Request, iCodeGenerationServiceProvider, out var namespaceRef, out var requestNameRef));
            declarationCollection.Add(this.BuildSdkMessageResponse(sdkMessagePair, sdkMessagePair.Response, namespaceRef, requestNameRef, iCodeGenerationServiceProvider));

            return declarationCollection;
        }

        private CodeTypeDeclaration BuildSdkMessageRequest(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageRequest sdkMessageRequest
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
            , out CodeExpression sdkMessageNamespaceAttributeRef
            , out CodeExpression sdkMessageRequestNameAttributeRef
        )
        {
            string requestClassName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), RequestClassSuffix);

            var requestClass = this.Class(requestClassName, RequestClassBaseType);

            requestClass.Members.Add(this.BuildClassConstant(SdkMessageNamespaceFieldName, typeof(string), messagePair.MessageNamespace));
            requestClass.Members.Add(this.BuildClassConstant(SdkMessageRequestNameFieldName, typeof(string), messagePair.Request.Name));

            List<CodeMemberProperty> genericMembers = new List<CodeMemberProperty>();
            List<CodeMemberProperty> allMembers = new List<CodeMemberProperty>();

            var statementsAssignDefaultParameters = new CodeStatementCollection();

            if (sdkMessageRequest.RequestFields != null & sdkMessageRequest.RequestFields.Count > 0)
            {
                foreach (var field in sdkMessageRequest.RequestFields.Values)
                {
                    var requestField = this.BuildRequestFieldProperty(sdkMessageRequest, field, iCodeGenerationServiceProvider);

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

            requestClass.CustomAttributes.Add(Attribute(typeof(DataContractAttribute), AttributeArg("Namespace", sdkMessageRequestNameAttributeRef)));
            requestClass.CustomAttributes.Add(Attribute(typeof(RequestProxyAttribute), AttributeArg(null, sdkMessageNamespaceAttributeRef)));

            return requestClass;
        }

        private CodeTypeDeclaration BuildSdkMessageResponse(
            CodeGenerationSdkMessagePair messagePair
            , CodeGenerationSdkMessageResponse sdkMessageResponse
            , CodeExpression sdkMessageNamespaceAttributeRef
            , CodeExpression sdkMessageRequestNameAttributeRef
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var codeTypeDeclaration = this.Class(string.Format(CultureInfo.InvariantCulture, "{0}{1}", iCodeGenerationServiceProvider.NamingService.GetNameForMessagePair(messagePair, iCodeGenerationServiceProvider), ResponseClassSuffix)
                , ResponseClassBaseType
                , Attribute(typeof(DataContractAttribute), AttributeArg("Namespace", sdkMessageNamespaceAttributeRef))
                , Attribute(typeof(ResponseProxyAttribute), AttributeArg(null, sdkMessageRequestNameAttributeRef))
            );

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
            , Entities.SdkMessageRequestField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var typeForRequestField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForRequestField(request, field, iCodeGenerationServiceProvider);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForRequestField(request, field, iCodeGenerationServiceProvider);

            CodeExpression properyNameRef = StringLiteral(propertyName);

            if (_config.GenerateAttributesWithNameOf)
            {
                properyNameRef = new CodeSnippetExpression($"nameof({propertyName})");
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

        private CodeMemberProperty BuildResponseFieldProperty(
            CodeGenerationSdkMessageResponse response
            , Entities.SdkMessageResponseField field
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            var forResponseField = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForResponseField(field, iCodeGenerationServiceProvider);

            string propertyName = iCodeGenerationServiceProvider.NamingService.GetNameForResponseField(response, field, iCodeGenerationServiceProvider);

            CodeExpression properyNameRef = StringLiteral(propertyName);

            if (_config.GenerateAttributesWithNameOf)
            {
                properyNameRef = new CodeSnippetExpression($"nameof({propertyName})");
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

        private CodeExpression BuildAttributeNameRef(
            EntityMetadata entityMetadata
            , string attributePropertyName
            , string attributeLogicalName
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._config.UseSchemaConstInCSharpAttributes)
            {
                var typeRef = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata, iCodeGenerationServiceProvider);

                var className = typeRef.BaseType + ".Schema.Attributes";

                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                attributePropertyName = attributePropertyName.ToLower();

                if (!provider.IsValidIdentifier(attributePropertyName))
                {
                    attributePropertyName = "@" + attributePropertyName;
                }

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

        private CodeMemberProperty BuildClassProperty(
            CodeTypeReference type
            , string propertyName
            , params CodeStatement[] stmts
        )
        {
            var codeMemberProperty = new CodeMemberProperty
            {
                Type = type,
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

        private static CodeMethodInvokeExpression EnumIsDefined(CodeTypeReference type, string variableName)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(typeof(Enum))), nameof(System.Enum.IsDefined), new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression(VarRef(variableName), "Value")
            });
        }

        private static CodeMethodInvokeExpression ConvertEnum(CodeTypeReference type, string variableName)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(typeof(Enum))), nameof(System.Enum.ToObject), new CodeExpression[2]
            {
                new CodeTypeOfExpression(type),
                new CodePropertyReferenceExpression(VarRef(variableName), "Value")
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

        private static CodeBinaryOperatorExpression ExpressionNull(CodeExpression expression)
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

        private static CodeBinaryOperatorExpression And(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanAnd, right);
        }
    }
}