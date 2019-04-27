using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    internal sealed class NamingService : INamingService
    {
        private static Regex nameRegex = new Regex("[a-z0-9_]*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private const string ConflictResolutionSuffix = "1";

        private const string ReferencingReflexiveRelationshipPrefix = "Referencing";
        private const string ReferencedReflexiveRelationshipPrefix = "Referenced";

        private readonly string _serviceContextName;

        private readonly Dictionary<string, int> _nameMap;
        private readonly Dictionary<string, string> _knowNames;

        private readonly List<string> _reservedAttributeNames;

        internal NamingService(string serviceContextName)
        {
            this._serviceContextName = string.IsNullOrWhiteSpace(serviceContextName) ? typeof(OrganizationServiceContext).Name + "1" : serviceContextName;

            this._nameMap = new Dictionary<string, int>();
            this._knowNames = new Dictionary<string, string>();
            this._reservedAttributeNames = new List<string>();

            foreach (MemberInfo property in typeof(Entity).GetProperties())
            {
                this._reservedAttributeNames.Add(property.Name);
            }
        }

        string INamingService.GetNameForOptionSet(
            EntityMetadata entityMetadata
            , OptionSetMetadataBase optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(optionSetMetadata.MetadataId.Value.ToString()))
            {
                return this._knowNames[optionSetMetadata.MetadataId.Value.ToString()];
            }

            string str = optionSetMetadata.OptionSetType.Value != OptionSetType.State ? this.CreateValidTypeName(optionSetMetadata.Name) : this.CreateValidTypeName(entityMetadata.SchemaName + "State");
            this._knowNames.Add(optionSetMetadata.MetadataId.Value.ToString(), str);
            return str;
        }

        string INamingService.GetNameForOption(
            OptionSetMetadataBase optionSetMetadata
            , OptionMetadata optionMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(optionSetMetadata.MetadataId.Value.ToString() + optionMetadata.Value.Value.ToString(CultureInfo.InvariantCulture)))
            {
                return this._knowNames[optionSetMetadata.MetadataId.Value.ToString() + optionMetadata.Value.Value.ToString(CultureInfo.InvariantCulture)];
            }

            string name = string.Empty;
            StateOptionMetadata stateOptionMetadata = optionMetadata as StateOptionMetadata;
            if (stateOptionMetadata != null)
            {
                name = stateOptionMetadata.InvariantName;
            }
            else
            {
                foreach (LocalizedLabel localizedLabel in optionMetadata.Label.LocalizedLabels)
                {
                    if (localizedLabel.LanguageCode == 1033)
                    {
                        name = localizedLabel.Label;
                    }
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                name = string.Format(CultureInfo.InvariantCulture, "UnknownLabel{0}", (object)optionMetadata.Value.Value);
            }

            string validName = NamingService.CreateValidName(name);

            this._knowNames.Add(optionSetMetadata.MetadataId.Value.ToString() + optionMetadata.Value.Value.ToString(CultureInfo.InvariantCulture), validName);

            return validName;
        }

        string INamingService.GetNameForEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(entityMetadata.MetadataId.Value.ToString()))
            {
                return this._knowNames[entityMetadata.MetadataId.Value.ToString()];
            }

            string validTypeName = this.CreateValidTypeName(string.IsNullOrEmpty(StaticNamingService.GetNameForEntity(entityMetadata)) ? entityMetadata.SchemaName : StaticNamingService.GetNameForEntity(entityMetadata));
            this._knowNames.Add(entityMetadata.MetadataId.Value.ToString(), validTypeName);
            return validTypeName;
        }

        string INamingService.GetNameForAttribute(
            EntityMetadata entityMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(entityMetadata.MetadataId.Value.ToString() + attributeMetadata.MetadataId.Value))
            {
                return this._knowNames[entityMetadata.MetadataId.Value.ToString() + attributeMetadata.MetadataId.Value];
            }

            string validName = NamingService.CreateValidName(StaticNamingService.GetNameForAttribute(attributeMetadata) ?? attributeMetadata.SchemaName);
            INamingService service = iCodeGenerationServiceProvider.NamingService;
            if (this._reservedAttributeNames.Contains(validName) || validName == service.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider))
            {
                validName += "1";
            }

            this._knowNames.Add(entityMetadata.MetadataId.Value.ToString() + attributeMetadata.MetadataId.Value, validName);
            return validName;
        }

        string INamingService.GetNameForRelationship(
            EntityMetadata entityMetadata
            , RelationshipMetadataBase relationshipMetadata
            , EntityRole? reflexiveRole
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            string str = reflexiveRole.HasValue ? reflexiveRole.Value.ToString() : string.Empty;
            if (this._knowNames.ContainsKey(entityMetadata.MetadataId.Value.ToString() + relationshipMetadata.MetadataId.Value + str))
            {
                return this._knowNames[entityMetadata.MetadataId.Value.ToString() + relationshipMetadata.MetadataId.Value + str];
            }

            string validName = NamingService.CreateValidName(!reflexiveRole.HasValue ? relationshipMetadata.SchemaName : (reflexiveRole.Value == EntityRole.Referenced ? "Referenced" + relationshipMetadata.SchemaName : "Referencing" + relationshipMetadata.SchemaName));
            Dictionary<string, string> dictionary = this._knowNames.Where<KeyValuePair<string, string>>(d => d.Key.StartsWith(entityMetadata.MetadataId.Value.ToString())).ToDictionary(d => d.Key, d => d.Value);
            INamingService service = iCodeGenerationServiceProvider.NamingService;
            if (this._reservedAttributeNames.Contains(validName) || validName == service.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider) || dictionary.ContainsValue(validName))
            {
                validName += "1";
            }

            this._knowNames.Add(entityMetadata.MetadataId.Value.ToString() + relationshipMetadata.MetadataId.Value + str, validName);
            return validName;
        }

        string INamingService.GetNameForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return this._serviceContextName;
        }

        string INamingService.GetNameForEntitySet(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return iCodeGenerationServiceProvider.NamingService.GetNameForEntity(entityMetadata, iCodeGenerationServiceProvider) + "Set";
        }

        string INamingService.GetNameForMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(messagePair.Id.ToString()))
            {
                return this._knowNames[messagePair.Id.ToString()];
            }

            string validTypeName = this.CreateValidTypeName(messagePair.Request.Name);
            this._knowNames.Add(messagePair.Id.ToString(), validTypeName);
            return validTypeName;
        }

        string INamingService.GetNameForRequestField(
            CodeGenerationSdkMessageRequest request
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageRequestField requestField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(request.Id.ToString() + requestField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)))
            {
                return this._knowNames[request.Id.ToString() + requestField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)];
            }

            string validName = NamingService.CreateValidName(requestField.Name);
            this._knowNames.Add(request.Id.ToString() + requestField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), validName);
            return validName;
        }

        string INamingService.GetNameForResponseField(
            CodeGenerationSdkMessageResponse response
            , Nav.Common.VSPackages.CrmDeveloperHelper.Entities.SdkMessageResponseField responseField
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (this._knowNames.ContainsKey(response.Id.ToString() + responseField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)))
            {
                return this._knowNames[response.Id.ToString() + responseField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)];
            }

            string validName = NamingService.CreateValidName(responseField.Name);
            this._knowNames.Add(response.Id.ToString() + responseField.Position.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), validName);
            return validName;
        }

        private string CreateValidTypeName(string name)
        {
            string validName = NamingService.CreateValidName(name);

            if (this._nameMap.ContainsKey(validName))
            {
                int num = ++this._nameMap[validName];
                return string.Format(CultureInfo.InvariantCulture, "{0}{1}", validName, num);
            }

            this._nameMap.Add(name, 0);

            return validName;
        }

        private static string CreateValidName(string name)
        {
            string input = name.Replace("$", "CurrencySymbol_").Replace("(", "_");

            StringBuilder stringBuilder = new StringBuilder();
            for (Match match = NamingService.nameRegex.Match(input); match.Success; match = match.NextMatch())
            {
                stringBuilder.Append(match.Value);
            }

            return stringBuilder.ToString();
        }

        IEnumerable<string> INamingService.GetCommentsForEntity(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            string name = entityMetadata.Description != null
                && entityMetadata.Description.UserLocalizedLabel != null
                ? entityMetadata.Description.UserLocalizedLabel.Label
                : (entityMetadata.Description.LocalizedLabels.Any() ? entityMetadata.Description.LocalizedLabels.First().Label : string.Empty);

            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<string>();
            }

            return new[] { name };
        }

        IEnumerable<string> INamingService.GetCommentsForEntityDefaultConstructor(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { $"Default Constructor {entityMetadata.LogicalName}" };
        }

        IEnumerable<string> INamingService.GetCommentsForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            string name = attributeMetadata.Description != null
                && attributeMetadata.Description.UserLocalizedLabel != null
                ? attributeMetadata.Description.UserLocalizedLabel.Label
                : (attributeMetadata.Description.LocalizedLabels.Any() ? attributeMetadata.Description.LocalizedLabels.First().Label : string.Empty);

            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<string>();
            }

            return new[] { name };
        }

        IEnumerable<string> INamingService.GetCommentsForRelationshipOneToMany(EntityMetadata entityMetadata, OneToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { "1:N " + relationshipMetadata.SchemaName };
        }

        IEnumerable<string> INamingService.GetCommentsForRelationshipManyToOne(EntityMetadata entityMetadata, OneToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { "N:1 " + relationshipMetadata.SchemaName };
        }

        IEnumerable<string> INamingService.GetCommentsForRelationshipManyToMany(EntityMetadata entityMetadata, ManyToManyRelationshipMetadata relationshipMetadata, EntityRole? reflexiveRole, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { "N:N " + relationshipMetadata.SchemaName };
        }

        IEnumerable<string> INamingService.GetCommentsForServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { "Represents a source of entities bound to a CRM service. It tracks and manages changes made to the retrieved entities." };
        }

        IEnumerable<string> INamingService.GetCommentsForEntitySet(EntityMetadata entityMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            CodeTypeReference typeForEntity = iCodeGenerationServiceProvider.TypeMappingService.GetTypeForEntity(entityMetadata, iCodeGenerationServiceProvider);

            return new[] { string.Format(CultureInfo.InvariantCulture, "Gets a binding to the set of all <see cref=\"{0}\"/> entities.", typeForEntity.BaseType) };
        }

        IEnumerable<string> INamingService.GetCommentsForServiceContextConstructor(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return new[] { "Constructor" };
        }

        IEnumerable<string> INamingService.GetCommentsForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string> INamingService.GetCommentsForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string> INamingService.GetCommentsForMessagePair(CodeGenerationSdkMessagePair messagePair, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string> INamingService.GetCommentsForRequestField(CodeGenerationSdkMessageRequest request, SdkMessageRequestField requestField, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string> INamingService.GetCommentsForResponseField(CodeGenerationSdkMessageResponse response, SdkMessageResponseField responseField, ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return Enumerable.Empty<string>();
        }
    }
}
