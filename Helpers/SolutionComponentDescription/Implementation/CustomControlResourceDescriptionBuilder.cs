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
            var list = GetEntities<CustomControlResource>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("ControlName", "Name", "WebResourceName", "WebResourceType", "Id", "IsManaged", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                handler.AddLine(
                    EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.customcontrolid + "." + AppModule.Schema.Attributes.name)
                    , entity.Name
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(entity, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
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
            var customControlResource = GetEntity<CustomControlResource>(component.ObjectId.Value);

            if (customControlResource != null)
            {
                return string.Format("ControlName {0}    Name {1}    WebResourceName {2}    WebResourceType {3}    Id {4}    IsManaged {5}    SolutionName {6}"
                    , EntityDescriptionHandler.GetAttributeString(customControlResource, CustomControlResource.Schema.Attributes.customcontrolid + "." + AppModule.Schema.Attributes.name)
                    , customControlResource.Name
                    , EntityDescriptionHandler.GetAttributeString(customControlResource, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.name)
                    , EntityDescriptionHandler.GetAttributeString(customControlResource, CustomControlResource.Schema.Attributes.webresourceid + "." + WebResource.Schema.Attributes.webresourcetype)
                    , customControlResource.Id.ToString()
                    , customControlResource.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(customControlResource, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
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