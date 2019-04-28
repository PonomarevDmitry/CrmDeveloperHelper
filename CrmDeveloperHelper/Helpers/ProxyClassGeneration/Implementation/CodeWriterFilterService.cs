using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    internal sealed class CodeWriterFilterService : ICodeWriterFilterService
    {
        private readonly List<string> _excludedNamespaces = new List<string>();
        private readonly string _messageNamespace;
        private readonly bool _generateMessages;
        private readonly bool _generateCustomActions;
        private readonly bool _generateServiceContext;

        private readonly CreateFileWithEntityMetadataCSharpConfiguration _config;

        public CodeWriterFilterService(CreateFileWithEntityMetadataCSharpConfiguration config)
        {
            this._config = config;

            _excludedNamespaces.Add("http://schemas.microsoft.com/xrm/2011/contracts");
        }

        //internal CodeWriterFilterService(CrmSvcUtilParameters parameters)
        //{
        //  this._messageNamespace = parameters.MessageNamespace;
        //  this._generateMessages = parameters.GenerateMessages;
        //  this._generateCustomActions = parameters.GenerateCustomActions;
        //  this._generateServiceContext = !string.IsNullOrWhiteSpace(parameters.ServiceContextName);
        //}

        public bool GenerateOptionSet(
            OptionSetMetadataBase optionSetMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (optionSetMetadata.IsGlobal.GetValueOrDefault())
            {
                if (!_config.GenerateGlobalOptionSet)
                {
                    return false;
                }
            }
            else
            {
                if (!_config.GenerateLocalOptionSet)
                {
                    return false;
                }
            }

            return optionSetMetadata.OptionSetType.Value == OptionSetType.State;
        }

        public bool GenerateOption(
            OptionMetadata option
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return true;
        }

        public bool GenerateEntity(
            EntityMetadata entityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (entityMetadata == null)
            {
                return false;
            }

            if (entityMetadata.IsIntersect.GetValueOrDefault()
                || string.Equals(entityMetadata.LogicalName, "activityparty", StringComparison.Ordinal)
                || string.Equals(entityMetadata.LogicalName, "calendarrule", StringComparison.Ordinal)
            )
            {
                return true;
            }

            //IMetadataProviderService service = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));
            //foreach (SdkMessage sdkMessage in (!(service is IMetadataProviderService2) ? service.LoadMetadata() : ((IMetadataProviderService2)service).LoadMetadata(services)).Messages.MessageCollection.Values)
            //{
            //    if (!sdkMessage.IsPrivate)
            //    {
            //        foreach (SdkMessageFilter sdkMessageFilter in sdkMessage.SdkMessageFilters.Values)
            //        {
            //            if (entityMetadata.ObjectTypeCode.HasValue && sdkMessageFilter.PrimaryObjectTypeCode == entityMetadata.ObjectTypeCode.Value
            //                || entityMetadata.ObjectTypeCode.HasValue && sdkMessageFilter.SecondaryObjectTypeCode == entityMetadata.ObjectTypeCode.Value
            //            )
            //            {
            //                return true;
            //            }
            //        }
            //    }
            //}

            return true;
        }

        public bool GenerateAttribute(
            AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (!_config.GenerateAttributes)
            {
                return false;
            }

            return !this.IsNotExposedChildAttribute(attributeMetadata)
                &&
                (
                    attributeMetadata.IsValidForCreate.GetValueOrDefault()
                    || attributeMetadata.IsValidForRead.GetValueOrDefault()
                    || attributeMetadata.IsValidForUpdate.GetValueOrDefault()
                );
        }

        private bool IsNotExposedChildAttribute(AttributeMetadata attributeMetadata)
        {
            if (!string.IsNullOrEmpty(attributeMetadata.AttributeOf)
                && !(attributeMetadata is ImageAttributeMetadata)
                && !attributeMetadata.LogicalName.EndsWith("_url", StringComparison.OrdinalIgnoreCase)
            )
            {
                return !attributeMetadata.LogicalName.EndsWith("_timestamp", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public bool GenerateRelationship(
            RelationshipMetadataBase relationshipMetadata
            , EntityMetadata otherEntityMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (relationshipMetadata.RelationshipType == RelationshipType.OneToManyRelationship)
            {
                if (!_config.GenerateOneToMany)
                {
                    return false;
                }
            }
            else if (relationshipMetadata.RelationshipType == RelationshipType.ManyToManyRelationship)
            {
                if (!_config.GenerateManyToMany)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (otherEntityMetadata == null
                || string.Equals(otherEntityMetadata.LogicalName, "calendarrule", StringComparison.Ordinal)
            )
            {
                return false;
            }

            var generateEntity = GenerateEntity(otherEntityMetadata, iCodeGenerationServiceProvider);

            if (!generateEntity)
            {
                return false;
            }

            return true;
        }

        public bool GenerateServiceContext(ICodeGenerationServiceProvider iCodeGenerationServiceProvider)
        {
            return this._generateServiceContext;
        }

        public bool GenerateSdkMessage(
            CodeGenerationSdkMessage message
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return (this._generateMessages || this._generateCustomActions) && (!message.IsPrivate && message.SdkMessageFilters.Count != 0);
        }

        public bool GenerateSdkMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (!this._generateMessages && !this._generateCustomActions
                || _excludedNamespaces.Contains(messagePair.MessageNamespace.ToUpperInvariant())
                || this._generateCustomActions && !messagePair.Message.IsCustomAction
            )
            {
                return false;
            }

            if (string.IsNullOrEmpty(this._messageNamespace))
            {
                return true;
            }

            return string.Equals(this._messageNamespace, messagePair.MessageNamespace, StringComparison.OrdinalIgnoreCase);
        }
    }
}
