using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class EntityMapDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public EntityMapDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.EntityMap)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.EntityMap;

        public override int ComponentTypeValue => (int)ComponentType.EntityMap;

        public override string EntityLogicalName => EntityMap.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => EntityMap.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    EntityMap.Schema.Attributes.sourceentityname
                    , EntityMap.Schema.Attributes.targetentityname
                    , EntityMap.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<EntityMap>(components.Select(c => c.ObjectId));

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

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var entityMap = GetEntity<EntityMap>(component.ObjectId.Value);

            if (entityMap != null)
            {
                return string.Format("EntityMap {0} -> {1}    IsManaged {2}    SolutionName {3}"
                    , entityMap.SourceEntityName
                    , entityMap.TargetEntityName
                    , entityMap.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entityMap, "solution.uniquename")
                    );
            }

            return component.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<EntityMap>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} -> {1}"
                    , entity.SourceEntityName
                    , entity.TargetEntityName
                    );
            }

            return component.ObjectId.ToString();
        }
    }
}