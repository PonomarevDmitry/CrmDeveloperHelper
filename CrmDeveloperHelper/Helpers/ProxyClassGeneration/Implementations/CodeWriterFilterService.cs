using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Interfaces;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration.Implementations
{
    internal sealed class CodeWriterFilterService : ICodeWriterFilterService
    {
        private readonly CreateFileCSharpConfiguration _config;

        public CodeWriterFilterService(CreateFileCSharpConfiguration config)
        {
            this._config = config;
        }

        public bool GenerateOptionSet(
            OptionSetMetadata optionSetMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (IgnoreOptionSet(optionSetMetadata, attributeMetadata, iCodeGenerationServiceProvider))
            {
                return false;
            }

            if (optionSetMetadata.OptionSetType == OptionSetType.State
                || optionSetMetadata.OptionSetType == OptionSetType.Status
            )
            {
                return _config.GenerateStatus;
            }

            if (optionSetMetadata.IsGlobal.GetValueOrDefault())
            {
                return _config.GenerateGlobalOptionSet;
            }
            else
            {
                return _config.GenerateLocalOptionSet;
            }
        }

        public bool IgnoreOptionSet(
            OptionSetMetadata optionSetMetadata
            , AttributeMetadata attributeMetadata
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            if (optionSetMetadata.Options == null
                || !optionSetMetadata.Options.Any(o => o.Value.HasValue)
            )
            {
                return true;
            }

            if (attributeMetadata != null
                && CreateFileHandler.IgnoreAttribute(attributeMetadata.EntityLogicalName, attributeMetadata.LogicalName)
            )
            {
                return true;
            }

            return false;
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
            return true;
        }

        public bool GenerateSdkMessagePair(
            CodeGenerationSdkMessagePair messagePair
            , ICodeGenerationServiceProvider iCodeGenerationServiceProvider
        )
        {
            return true;
        }
    }
}
