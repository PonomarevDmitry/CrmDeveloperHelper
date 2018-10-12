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
    public class FieldSecurityProfileDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public FieldSecurityProfileDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.FieldSecurityProfile)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.FieldSecurityProfile;

        public override int ComponentTypeValue => (int)ComponentType.FieldSecurityProfile;

        public override string EntityLogicalName => FieldSecurityProfile.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => FieldSecurityProfile.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    FieldSecurityProfile.Schema.Attributes.name
                    , FieldSecurityProfile.Schema.Attributes.description
                    , FieldSecurityProfile.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<FieldSecurityProfile>(components.Select(c => c.ObjectId));

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
            table.SetHeader("Name", "Description", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.Name;

                string desc = entity.Description;

                table.AddLine(name
                    , desc
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var fieldSecurityProfile = GetEntity<FieldSecurityProfile>(component.ObjectId.Value);

            if (fieldSecurityProfile != null)
            {
                string title = fieldSecurityProfile.Name;

                return string.Format("FieldSecurityProfile {0}    IsManaged {1}    SolutionName {2}"
                    , title
                    , fieldSecurityProfile.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(fieldSecurityProfile, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { FieldSecurityProfile.Schema.Attributes.name, "Name" }
                    , { FieldSecurityProfile.Schema.Attributes.description, "Description" }
                    , { FieldSecurityProfile.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}