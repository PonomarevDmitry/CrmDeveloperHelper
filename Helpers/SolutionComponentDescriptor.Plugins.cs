using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionPluginAssembly(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<PluginAssembly>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("Name", "IsManged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string name = entity.Name;

                table.AddLine(name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionPluginAssemblySingle(PluginAssembly entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string name = entity.Name;

                return string.Format("PluginAssembly {0}    IsManged {1}    SolutionName {2}"
                    , name
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionPluginType(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<PluginType>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            var table = new FormatTextTableHandler();
            table.SetHeader("PluginAssembly", "PluginType", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                table.AddLine(
                    entity.AssemblyName
                    , entity.TypeName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionPluginTypeSingle(PluginType entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("PluginAssembly {0}    PluginType {1}    IsManged {2}    SolutionName {3}"
                    , entity.AssemblyName
                    , entity.TypeName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSDKMessageProcessingStep(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SdkMessageProcessingStep>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();

            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "FilteringAttributes");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageProcessingStep>()))
            {
                handler.AddLine(
                    entity.EventHandler?.Name ?? "Unknown"
                    , entity.PrimaryObjectTypeCodeName
                    , entity.SecondaryObjectTypeCodeName
                    , entity.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(entity.Stage.Value, entity.Mode.Value)
                    , entity.Rank.ToString()
                    , entity.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.FilteringAttributesStringsSorted
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSDKMessageProcessingStepSingle(SdkMessageProcessingStep entity, SolutionComponent component)
        {
            if (entity != null && entity.LogicalName == SdkMessageProcessingStep.EntityLogicalName)
            {
                var step = entity.ToEntity<SdkMessageProcessingStep>();

                FormatTextTableHandler handler = new FormatTextTableHandler();

                handler.AddLine(
                    step.EventHandler?.Name ?? "Unknown"
                    , step.PrimaryObjectTypeCodeName
                    , step.SecondaryObjectTypeCodeName
                    , step.SdkMessageId?.Name ?? "Unknown"
                    , SdkMessageProcessingStepRepository.GetStageName(step.Stage.Value, step.Mode.Value)
                    , step.Rank.ToString()
                    , step.FormattedValues[SdkMessageProcessingStep.Schema.Attributes.statuscode]
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , step.FilteringAttributesStringsSorted
                );

                var str = handler.GetFormatedLines(false).FirstOrDefault();

                return string.Format("SdkMessageProcessingStep {0}", str);
            }

            return component.ToString();
        }

        private void GenerateDescriptionSDKMessageProcessingStepImage(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SdkMessageProcessingStepImage>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    notFinded.ForEach(item => builder.AppendFormat(formatSpacer, item.ToString()).AppendLine());
                }
            }

            FormatTextTableHandler handler = new FormatTextTableHandler();

            handler.SetHeader("PluginType", "Primary Entity", "Secondary Entity", "Message", "Stage", "Rank", "Status", "ImageType", "Name", "EntityAlias", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Attributes");

            foreach (var entity in list.Select(ent => ent.ToEntity<SdkMessageProcessingStepImage>()))
            {
                string pluginType = entity.Contains("sdkmessageprocessingstep.eventhandler") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null";

                string sdkMessage = entity.Contains("sdkmessageprocessingstep.sdkmessageid") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null";
                int stage = entity.Contains("sdkmessageprocessingstep.stage") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
                int mode = entity.Contains("sdkmessageprocessingstep.mode") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;
                int rank = entity.Contains("sdkmessageprocessingstep.rank") ? (int)entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0;
                string status = entity.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? entity.FormattedValues["sdkmessageprocessingstep.statuscode"] : "";

                handler.AddLine(
                    pluginType
                    , entity.PrimaryObjectTypeCodeName
                    , entity.SecondaryObjectTypeCodeName
                    , sdkMessage
                    , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                    , rank.ToString()
                    , status
                    , entity.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                    , entity.Name
                    , entity.EntityAlias
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.Attributes1StringsSorted
                );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSDKMessageProcessingStepImageSingle(SdkMessageProcessingStepImage entity, SolutionComponent component)
        {
            if (entity != null && entity.LogicalName == SdkMessageProcessingStepImage.EntityLogicalName)
            {
                var image = entity.ToEntity<SdkMessageProcessingStepImage>();

                FormatTextTableHandler handler = new FormatTextTableHandler();

                string pluginType = entity.Contains("sdkmessageprocessingstep.eventhandler") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.eventhandler").Value as EntityReference).Name : "Null";

                string sdkMessage = entity.Contains("sdkmessageprocessingstep.sdkmessageid") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.sdkmessageid").Value as EntityReference).Name : "Null";
                int stage = entity.Contains("sdkmessageprocessingstep.stage") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.stage").Value as OptionSetValue).Value : 0;
                int mode = entity.Contains("sdkmessageprocessingstep.mode") ? (entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.mode").Value as OptionSetValue).Value : 0;
                int rank = entity.Contains("sdkmessageprocessingstep.rank") ? (int)entity.GetAttributeValue<AliasedValue>("sdkmessageprocessingstep.rank").Value : 0;
                string status = entity.FormattedValues.ContainsKey("sdkmessageprocessingstep.statuscode") ? entity.FormattedValues["sdkmessageprocessingstep.statuscode"] : "";

                handler.AddLine(
                    pluginType
                    , entity.PrimaryObjectTypeCodeName
                    , entity.SecondaryObjectTypeCodeName
                    , sdkMessage
                    , SdkMessageProcessingStepRepository.GetStageName(stage, mode)
                    , rank.ToString()
                    , status
                    , entity.FormattedValues[SdkMessageProcessingStepImage.Schema.Attributes.imagetype]
                    , image.Name
                    , image.EntityAlias
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    , entity.Attributes1StringsSorted
                );

                var str = handler.GetFormatedLines(false).FirstOrDefault();

                return string.Format("SdkMessageProcessingStepImage {0}", str);
            }

            return component.ToString();
        }
    }
}
