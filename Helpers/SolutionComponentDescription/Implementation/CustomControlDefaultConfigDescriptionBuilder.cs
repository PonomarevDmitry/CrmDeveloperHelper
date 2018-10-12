using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class CustomControlDefaultConfigDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public CustomControlDefaultConfigDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.CustomControlDefaultConfig)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.CustomControlDefaultConfig;

        public override int ComponentTypeValue => (int)ComponentType.CustomControlDefaultConfig;

        public override string EntityLogicalName => CustomControlDefaultConfig.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => CustomControlDefaultConfig.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode
                    , CustomControlDefaultConfig.Schema.Attributes.customcontroldefaultconfigid
                    , CustomControlDefaultConfig.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<CustomControlDefaultConfig>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("EntityName", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    entity.PrimaryEntityTypeCode
                    , entity.Id.ToString()
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
            var customControlDefaultConfig = GetEntity<CustomControlDefaultConfig>(component.ObjectId.Value);

            if (customControlDefaultConfig != null)
            {
                return string.Format("EntityName {0}    Id {1}    IsManaged {2}    SolutionName {3}"
                    , customControlDefaultConfig.PrimaryEntityTypeCode
                    , customControlDefaultConfig.Id.ToString()
                    , customControlDefaultConfig.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(customControlDefaultConfig, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent solutionComponent)
        {
            var entity = GetEntity<CustomControlDefaultConfig>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , entity.PrimaryEntityTypeCode
                    , entity.Id.ToString()
                    );
            }

            return base.GetName(solutionComponent);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { CustomControlDefaultConfig.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                    , { CustomControlDefaultConfig.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { CustomControlDefaultConfig.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}