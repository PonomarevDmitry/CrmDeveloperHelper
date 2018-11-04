using Microsoft.Xrm.Sdk;
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
    public class CustomControlResourceDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public CustomControlResourceDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.CustomControlResource)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.CustomControlResource;

        public override int ComponentTypeValue => (int)ComponentType.CustomControlResource;

        public override string EntityLogicalName => CustomControlResource.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => CustomControlResource.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    CustomControlResource.Schema.Attributes.name
                    , CustomControlResource.Schema.Attributes.customcontrolid
                    , CustomControlResource.Schema.Attributes.webresourceid
                    , CustomControlResource.Schema.Attributes.ismanaged
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
                        LinkFromEntityName = CustomControlResource.EntityLogicalName,
                        LinkFromAttributeName = CustomControlResource.Schema.Attributes.customcontrolid,

                        LinkToEntityName = CustomControl.EntityLogicalName,
                        LinkToAttributeName = CustomControl.PrimaryIdAttribute,

                        EntityAlias = CustomControlResource.Schema.Attributes.customcontrolid,

                        Columns = new ColumnSet(CustomControl.Schema.Attributes.name),
                    },

                    new LinkEntity()
                    {
                        LinkFromEntityName = CustomControlResource.EntityLogicalName,
                        LinkFromAttributeName = CustomControlResource.Schema.Attributes.webresourceid,

                        LinkToEntityName = WebResource.EntityLogicalName,
                        LinkToAttributeName = WebResource.PrimaryIdAttribute,

                        EntityAlias = CustomControlResource.Schema.Attributes.webresourceid,

                        Columns = new ColumnSet(WebResource.Schema.Attributes.name, WebResource.Schema.Attributes.webresourcetype),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = AttributeMap.EntityLogicalName,
                        LinkFromAttributeName = AttributeMap.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = AttributeMap.EntityLogicalName,
                        LinkFromAttributeName = AttributeMap.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

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
            handler.SetHeader("ControlName", "Name", "WebResourceName", "WebResourceType", "Id", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<CustomControlResource>();

            List<string> values = new List<string>();

            values.AddRange(new[]
            {
                EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.customcontrolid + "." + AppModule.Schema.Attributes.name)
                , entity.Name
                , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                , entity.Id.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<CustomControlResource>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} - {1} - {2} - {3}"
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.customcontrolid + "." + CustomControl.Schema.Attributes.name)
                    , entity.Name
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { CustomControlResource.Schema.Attributes.customcontrolid + "." + CustomControl.Schema.Attributes.name, "ControlName" }
                    , { CustomControlResource.Schema.Attributes.name, "Name" }
                    , { CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name, "WebResourceName" }
                    , { CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype, "WebResourceType" }
                    , { CustomControlResource.Schema.EntityPrimaryIdAttribute, "Id" }
                    , { CustomControlResource.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}