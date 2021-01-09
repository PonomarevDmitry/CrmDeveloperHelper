using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class PrivilegeObjectTypeCodeDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public PrivilegeObjectTypeCodeDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.PrivilegeObjectTypeCode)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.PrivilegeObjectTypeCode;

        public override int ComponentTypeValue => (int)ComponentType.PrivilegeObjectTypeCode;

        public override string EntityLogicalName => PrivilegeObjectTypeCodes.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => PrivilegeObjectTypeCodes.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet(
                PrivilegeObjectTypeCodes.Schema.Attributes.privilegeobjecttypecodeid
                , PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid
                , PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode
            );
        }

        protected override QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = this.EntityLogicalName,

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
                        LinkFromEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkFromAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        LinkToEntityName = Privilege.EntityLogicalName,
                        LinkToAttributeName = Privilege.Schema.Attributes.privilegeid,

                        EntityAlias = PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid,

                        Columns = new ColumnSet(Privilege.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkFromAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = PrivilegeObjectTypeCodes.EntityLogicalName,
                        LinkFromAttributeName = PrivilegeObjectTypeCodes.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.EntityPrimaryIdAttribute,

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },
            };

            return query;
        }

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader(PrivilegeObjectTypeCodes.Schema.Headers.privilegeid, PrivilegeObjectTypeCodes.Schema.Headers.objecttypecode, "Behavior");

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<PrivilegeObjectTypeCodes>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid + "." + Privilege.Schema.Attributes.name)
                , entity.ObjectTypeCode
                , behavior
            });

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<PrivilegeObjectTypeCodes>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1}"
                    , entity.ObjectTypeCode
                    , EntityDescriptionHandler.GetAttributeString(entity, PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid + "." + Privilege.Schema.Attributes.name)
                );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    {  PrivilegeObjectTypeCodes.Schema.Attributes.privilegeid + "." + Privilege.Schema.Attributes.name, PrivilegeObjectTypeCodes.Schema.Headers.privilegeid }
                    , { PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode, PrivilegeObjectTypeCodes.Schema.Headers.objecttypecode }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}