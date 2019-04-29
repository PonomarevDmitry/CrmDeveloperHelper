using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    internal sealed class CodeWriterFilterService : ICodeWriterFilterService
    {
        private readonly HashSet<string> _excludedNamespaces = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private readonly string _messageNamespace;

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

            return true;

            //if (entityMetadata.IsIntersect.GetValueOrDefault()
            //    || string.Equals(entityMetadata.LogicalName, "activityparty", StringComparison.InvariantCultureIgnoreCase)
            //    //|| string.Equals(entityMetadata.LogicalName, "calendarrule", StringComparison.InvariantCultureIgnoreCase)
            //)
            //{
            //    return true;
            //}

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
                && !attributeMetadata.LogicalName.EndsWith("_url", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return !attributeMetadata.LogicalName.EndsWith("_timestamp", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public bool GenerateRelationship(
            RelationshipMetadataBase relationshipMetadata
            , EntityMetadata otherEntityMetadata
            , CodeGenerationRelationshipType relationshipType
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (otherEntityMetadata == null)
            {
                return false;
            }

            if (relationshipType == CodeGenerationRelationshipType.ManyToManyRelationship)
            {
                if (!_config.GenerateManyToMany)
                {
                    return false;
                }
            }
            else if (relationshipType == CodeGenerationRelationshipType.OneToManyRelationship)
            {
                if (!_config.GenerateOneToMany)
                {
                    return false;
                }
            }
            else if (relationshipType == CodeGenerationRelationshipType.ManyToOneRelationship)
            {
                if (!_config.GenerateManyToOne)
                {
                    return false;
                }
            }
            else
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
            return this._config.GenerateServiceContext;
        }

        public bool GenerateSdkMessage(
            CodeGenerationSdkMessage message
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return (this._config.GenerateMessages || this._config.GenerateCustomActions) 
                && (!message.IsPrivate && message.SdkMessageFilters.Count != 0);
        }

        public bool GenerateSdkMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (!this._config.GenerateMessages && !this._config.GenerateCustomActions
                || _excludedNamespaces.Contains(messagePair.MessageNamespace)
                || this._config.GenerateCustomActions && !messagePair.Message.IsCustomAction
            )
            {
                return false;
            }

            if (string.IsNullOrEmpty(this._messageNamespace))
            {
                return true;
            }

            return string.Equals(this._messageNamespace, messagePair.MessageNamespace, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
