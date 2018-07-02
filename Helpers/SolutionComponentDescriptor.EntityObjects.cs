using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class SolutionComponentDescriptor
    {
        private void GenerateDescriptionEntityMap(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<EntityMap>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("Source", "", "Target", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.SourceEntityName
                    , "->"
                    , entity.TargetEntityName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionEntityMapSingle(EntityMap entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("EntityMap {0} -> {1}    IsManaged {2}    SolutionName {3}"
                    , entity.SourceEntityName
                    , entity.TargetEntityName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionAttributeMap(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<AttributeMap>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("Source", "Attribute", "", "Target", "Attribute", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname").Value.ToString()
                    , entity.SourceAttributeName
                    , "->"
                    , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname").Value.ToString()
                    , entity.TargetAttributeName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionAttributeMapSingle(AttributeMap entity, SolutionComponent component)
        {
            if (entity != null)
            {
                return string.Format("AttributeMap {0}.{1} -> {2}.{3}    IsManaged {4}    SolutionName {5}"
                    , entity.GetAttributeValue<AliasedValue>("entitymap.sourceentityname").Value.ToString()
                    , entity.SourceAttributeName
                    , entity.GetAttributeValue<AliasedValue>("entitymap.targetentityname").Value.ToString()
                    , entity.TargetAttributeName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSystemForm(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SystemForm>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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

            handler.SetHeader("EntityName", "FormType", "FormName", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list
                .OrderBy(ent => ent.ObjectTypeCode)
                .ThenBy(ent => ent.Type.Value)
                .ThenBy(ent => ent.Name)
                )
            {
                string formName = entity.Name;
                string entityName = entity.ObjectTypeCode;

                string formTypeName = entity.FormattedValues[SystemForm.Schema.Attributes.type];

                handler.AddLine(entityName
                    , string.Format("'{0}'", formTypeName)
                    , string.Format("'{0}'", formName)
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSystemFormSingle(SystemForm entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string formName = entity.Name;
                string entityName = entity.ObjectTypeCode;

                string formTypeName = entity.FormattedValues[SystemForm.Schema.Attributes.type];

                return string.Format("SystemForm     {0}    '{1}'    '{2}'    IsManged {3}    SolutionName {4}"
                    , entityName
                    , formTypeName
                    , formName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSavedQuery(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SavedQuery>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("EntityName", "Name", "QueryType", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string queryName = entity.Name;
                string entityName = entity.ReturnedTypeCode;

                handler.AddLine(entityName
                    , queryName
                    , Repository.SavedQueryRepository.GetQueryTypeName(entity.QueryType.GetValueOrDefault())
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSavedQuerySingle(SavedQuery entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string queryName = entity.Name;
                string entityName = entity.ReturnedTypeCode;

                return string.Format("SavedQuery {0} - '{1}'    QueryType {2}    IsManaged {3}    SolutionName {4}"
                    , entityName
                    , queryName
                    , Repository.SavedQueryRepository.GetQueryTypeName(entity.QueryType.GetValueOrDefault())
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        private void GenerateDescriptionSavedQueryVisualization(StringBuilder builder, List<SolutionComponent> components)
        {
            var list = GetEntities<SavedQueryVisualization>(components[0].ComponentType.Value, components.Select(c => c.ObjectId));

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
            handler.SetHeader("PrimaryEntityTypeCode", "Name", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                string chartName = entity.Name;
                string entityName = entity.PrimaryEntityTypeCode;

                handler.AddLine(entityName
                    , chartName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        private string GenerateDescriptionSavedQueryVisualizationSingle(SavedQueryVisualization entity, SolutionComponent component)
        {
            if (entity != null)
            {
                string chartName = entity.Name;
                string entityName = entity.PrimaryEntityTypeCode;

                return string.Format("SavedQueryVisualization (Chart)     {0}    Name '{1}'    IsManged {2}    SolutionName {3}"
                    , entityName
                    , chartName
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    );
            }

            return component.ToString();
        }
    }
}