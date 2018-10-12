using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class FieldPermissionDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public FieldPermissionDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.FieldPermission)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.FieldPermission;

        public override int ComponentTypeValue => (int)ComponentType.FieldPermission;

        public override string EntityLogicalName => FieldPermission.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => FieldPermission.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    FieldPermission.Schema.Attributes.entityname
                    , FieldPermission.Schema.Attributes.attributelogicalname
                    , FieldPermission.Schema.Attributes.ismanaged
                );
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = FieldPermission.EntityLogicalName,

                ColumnSet = GetColumnSet(),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(this.EntityPrimaryIdAttribute, ConditionOperator.In, idsNotCached.ToArray()),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.fieldsecurityprofileid,

                        LinkToEntityName = FieldSecurityProfile.EntityLogicalName,
                        LinkToAttributeName = FieldSecurityProfile.PrimaryIdAttribute,

                        EntityAlias = FieldSecurityProfile.EntityLogicalName,

                        Columns = new ColumnSet(FieldSecurityProfile.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = FieldPermission.EntityLogicalName,
                        LinkFromAttributeName = FieldPermission.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            return query;
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<FieldPermission>(components.Select(c => c.ObjectId));

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
            table.SetHeader("FieldSecurityProfileName", "Attribute", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                var name = entity.GetAttributeValue<AliasedValue>("fieldsecurityprofile.name").Value.ToString();

                string attr = string.Format("{0}.{1}"
                    , entity.EntityName
                    , entity.AttributeLogicalName
                    );

                table.AddLine(name
                    , attr
                    , entity.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                    , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                    );
            }

            table.GetFormatedLines(true).ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            var fieldPermission = GetEntity<FieldPermission>(solutionComponent.ObjectId.Value);

            if (fieldPermission != null)
            {
                var name = fieldPermission.GetAttributeValue<AliasedValue>("fieldsecurityprofile.name").Value.ToString();

                string attr = string.Format("{0}.{1}"
                    , fieldPermission.EntityName
                    , fieldPermission.AttributeLogicalName
                    );

                return string.Format("FieldSecurityProfile {0}    Attribute {1}    IsManaged {2}    SolutionName {3}"
                    , name
                    , attr
                    , fieldPermission.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(fieldPermission, "solution.uniquename")
                    );
            }

            return solutionComponent.ToString();
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<FieldPermission>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0}.{1}", entity.EntityName, entity.AttributeLogicalName);
            }

            return component.ObjectId.ToString();
        }
    }
}