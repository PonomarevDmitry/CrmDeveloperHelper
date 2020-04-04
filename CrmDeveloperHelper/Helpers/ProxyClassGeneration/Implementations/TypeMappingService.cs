using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Implementations
{
    internal sealed class TypeMappingService : ITypeMappingService
    {
        private readonly string _namespace;

        internal TypeMappingService(string @namespace)
        {
            this._namespace = @namespace;
        }

        public CodeTypeReference GetTypeForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return this.TypeRef(iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider));
        }

        public CodeTypeReference GetTypeForOptionSet(EntityMetadata entityMetadata, OptionSetMetadata optionSetMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return this.TypeRef(iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entityMetadata, optionSetMetadata, iCodeGenerationServiceProvider));
        }

        private static readonly Dictionary<AttributeTypeCode, Type> _attributeTypeMapping = new Dictionary<AttributeTypeCode, Type>
        {
            { AttributeTypeCode.Boolean, typeof(bool) },

            { AttributeTypeCode.ManagedProperty, typeof(BooleanManagedProperty) },
            { AttributeTypeCode.CalendarRules, typeof(object) },

            { AttributeTypeCode.DateTime, typeof(DateTime) },

            { AttributeTypeCode.Double, typeof(double) },
            { AttributeTypeCode.Integer, typeof(int) },
            { AttributeTypeCode.BigInt, typeof(long) },

            { AttributeTypeCode.Decimal, typeof(decimal) },
            { AttributeTypeCode.Money, typeof(Money) },

            { AttributeTypeCode.EntityName, typeof(string) },

            { AttributeTypeCode.Customer, typeof(EntityReference) },
            { AttributeTypeCode.Lookup, typeof(EntityReference) },
            { AttributeTypeCode.Owner, typeof(EntityReference) },

            { AttributeTypeCode.Memo, typeof(string) },
            { AttributeTypeCode.String, typeof(string) },

            { AttributeTypeCode.Picklist, typeof(OptionSetValue) },
            { AttributeTypeCode.State, typeof(OptionSetValue) },
            { AttributeTypeCode.Status, typeof(OptionSetValue) },

            { AttributeTypeCode.Uniqueidentifier, typeof(Guid) }
        };

        public CodeTypeReference GetTypeForAttributeType(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            Type type = typeof(object);

            if (attributeMetadata.AttributeType.HasValue)
            {
                AttributeTypeCode key = attributeMetadata.AttributeType.Value;

                if (key == AttributeTypeCode.PartyList)
                {
                    return this.BuildCodeTypeReferenceForPartyList(iCodeGenerationServiceProvider);
                }

                if (_attributeTypeMapping.ContainsKey(key))
                {
                    type = _attributeTypeMapping[key];
                }
                else if (attributeMetadata is ImageAttributeMetadata)
                {
                    type = typeof(byte[]);
                }
                else if (attributeMetadata is MultiSelectPicklistAttributeMetadata)
                {
                    type = typeof(OptionSetValueCollection);
                }
                //else
                //{
                //    OptionSetMetadataBase attributeOptionSet = TypeMappingService.GetAttributeOptionSet(attributeMetadata);
                //    if (attributeOptionSet != null)
                //    {
                //        CodeTypeReference codeTypeReference = this.BuildCodeTypeReferenceForOptionSet(attributeMetadata.LogicalName, entityMetadata, attributeMetadata, attributeOptionSet, iCodeGenerationServiceProvider);
                //        if (!codeTypeReference.BaseType.Equals("System.Object"))
                //        {
                //            return codeTypeReference;
                //        }

                //        if (key.Equals(AttributeTypeCode.Picklist) || key.Equals(AttributeTypeCode.Status))
                //        {
                //            type = typeof(OptionSetValue);
                //        }
                //    }
                //}
            }

            if (type.IsValueType)
            {
                type = typeof(Nullable<>).MakeGenericType(type);
            }

            return TypeMappingService.TypeRef(type);
        }

        private CodeTypeReference BuildCodeTypeReferenceForPartyList(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            EntityMetadata entityMetadata = iCodeGenerationServiceProvider.MetadataProviderService.GetEntityMetadata("activityparty");

            if (entityMetadata != null)
            {
                if (!iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateEntity(entityMetadata, iCodeGenerationServiceProvider))
                {
                    entityMetadata = null;
                }
            }

            if (entityMetadata == null)
            {
                return TypeMappingService.TypeRef(typeof(IEnumerable<>), TypeMappingService.TypeRef(typeof(Entity)));
            }

            return TypeMappingService.TypeRef(typeof(IEnumerable<>), this.TypeRef(iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider)));
        }

        private CodeTypeReference BuildCodeTypeReferenceForOptionSet(
            string attributeName
            , EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , OptionSetMetadata attributeOptionSet
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (iCodeGenerationServiceProvider.CodeWriterFilterService.GenerateOptionSet(attributeOptionSet, attributeMetadata, iCodeGenerationServiceProvider))
            {
                string nameForOptionSet = iCodeGenerationServiceProvider.NamingService.GetNameForOptionSet(entityMetadata, attributeOptionSet, iCodeGenerationServiceProvider);

                CodeGenerationType typeForOptionSet = iCodeGenerationServiceProvider.CodeGenerationService.GetTypeForOptionSet(entityMetadata, attributeOptionSet, iCodeGenerationServiceProvider);

                switch (typeForOptionSet)
                {
                    case CodeGenerationType.Class:
                        return this.TypeRef(nameForOptionSet);

                    case CodeGenerationType.Enum:
                    case CodeGenerationType.Struct:
                        return TypeMappingService.TypeRef(typeof(Nullable<>), this.TypeRef(nameForOptionSet));
                }
            }
            return TypeMappingService.TypeRef(typeof(object));
        }

        public CodeTypeReference GetTypeForRelationship(
            RelationshipMetadataBase relationshipMetadata
            , EntityMetadata otherEntityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return this.TypeRef(iCodeGenerationServiceProvider.NamingService.GetNameForEntity(otherEntityMetadata, iCodeGenerationServiceProvider));
        }

        public CodeTypeReference GetTypeForRequestField(CodeGenerationSdkMessageRequest request, Entities.SdkMessageRequestField requestField, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            var isGeneric = request.MessagePair.Message.IsGeneric(requestField);

            return this.GetTypeForField(requestField.ClrParser, isGeneric);
        }

        public CodeTypeReference GetTypeForResponseField(Entities.SdkMessageResponseField responseField, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return this.GetTypeForField(responseField.ClrFormatter, false);
        }

        public static OptionSetMetadata GetAttributeOptionSet(AttributeMetadata attribute)
        {
            if (attribute is EnumAttributeMetadata enumAttributeMetadata
                && !(attribute is EntityNameAttributeMetadata)
            )
            {
                return enumAttributeMetadata.OptionSet;
            }

            return null;
        }

        private CodeTypeReference GetTypeForField(string clrFormatter, bool isGeneric)
        {
            CodeTypeReference codeTypeReference = TypeMappingService.TypeRef(typeof(object));

            if (isGeneric)
            {
                codeTypeReference = new CodeTypeReference(new CodeTypeParameter("T"));
            }
            else if (!string.IsNullOrEmpty(clrFormatter))
            {
                Type type = Type.GetType(clrFormatter, false);
                if (type != null)
                {
                    codeTypeReference = TypeMappingService.TypeRef(type);
                }
                else
                {
                    string[] strArray = clrFormatter.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strArray != null && strArray.Length > 0)
                    {
                        codeTypeReference = new CodeTypeReference(strArray[0]);
                    }
                }
            }
            return codeTypeReference;
        }

        private CodeTypeReference TypeRef(string typeName)
        {
            if (!string.IsNullOrWhiteSpace(this._namespace))
            {
                return new CodeTypeReference(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this._namespace, typeName));
            }

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
    }
}