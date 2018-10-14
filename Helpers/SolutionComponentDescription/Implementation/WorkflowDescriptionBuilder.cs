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
    public class WorkflowDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public WorkflowDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.Workflow)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.Workflow;

        public override int ComponentTypeValue => (int)ComponentType.Workflow;

        public override string EntityLogicalName => Workflow.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => Workflow.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    Workflow.Schema.Attributes.primaryentity
                    , Workflow.Schema.Attributes.category
                    , Workflow.Schema.Attributes.name
                    , Workflow.Schema.Attributes.uniquename
                    , Workflow.Schema.Attributes.businessprocesstype
                    , Workflow.Schema.Attributes.scope
                    , Workflow.Schema.Attributes.mode
                    , Workflow.Schema.Attributes.statuscode
                    , Workflow.Schema.Attributes.ismanaged
                    , Workflow.Schema.Attributes.iscustomizable
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<Workflow>(components.Select(c => c.ObjectId));

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

            handler.SetHeader("Entity", "Category", "Name", "Type", "Scope", "Mode", "StatusCode", "IsManaged", "IsCustomizable", "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged", "Url");

            foreach (var entity in list
                    .OrderBy(entity => entity.PrimaryEntity)
                    .ThenBy(entity => entity.Category?.Value)
                    .ThenBy(entity => entity.Name)
                    .ThenBy(entity =>
                    {
                        var op = entity.BusinessProcessType;
                        return (op != null) ? (int?)op.Value : null;
                    })
                )
            {
                CreateOneWorkFlowDescription(entity, handler, withUrls);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        public override string GenerateDescriptionSingle(SolutionComponent component, bool withUrls)
        {
            var workflow = GetEntity<Workflow>(component.ObjectId.Value);

            if (workflow != null)
            {
                StringBuilder builder = new StringBuilder();

                string primaryentity = workflow.PrimaryEntity;
                string name = workflow.Name;
                string category = string.Empty;
                string businessprocesstype = string.Empty;
                string scope = string.Empty;
                string mode = string.Empty;

                builder.AppendFormat("Workflow {0}", primaryentity);

                if (workflow.Contains(Workflow.Schema.Attributes.category) && workflow.Attributes[Workflow.Schema.Attributes.category] != null)
                {
                    category = workflow.FormattedValues[Workflow.Schema.Attributes.category];

                    builder.AppendFormat(" {0}", category);
                }

                builder.AppendFormat("    {0}", name);

                var uniqueName = workflow.UniqueName;

                if (!string.IsNullOrEmpty(uniqueName))
                {
                    builder.AppendFormat("    UniqueName {0}", uniqueName);
                }

                if (workflow.Contains(Workflow.Schema.Attributes.businessprocesstype) && workflow.Attributes[Workflow.Schema.Attributes.businessprocesstype] != null)
                {
                    businessprocesstype = workflow.FormattedValues[Workflow.Schema.Attributes.businessprocesstype];

                    builder.AppendFormat("    {0}", businessprocesstype);
                }

                if (workflow.Contains(Workflow.Schema.Attributes.scope) && workflow.Attributes[Workflow.Schema.Attributes.scope] != null)
                {
                    scope = workflow.FormattedValues[Workflow.Schema.Attributes.scope];

                    builder.AppendFormat("    {0}", scope);
                }

                if (workflow.Contains(Workflow.Schema.Attributes.mode) && workflow.Attributes[Workflow.Schema.Attributes.mode] != null)
                {
                    mode = workflow.FormattedValues[Workflow.Schema.Attributes.mode];

                    builder.AppendFormat("    {0}", mode);
                }

                builder.AppendFormat("    {0}", workflow.FormattedValues[Workflow.Schema.Attributes.statuscode]);

                builder.AppendFormat("    IsManaged {0}", workflow.IsManaged.ToString());

                builder.AppendFormat("    IsCustomizable {0}", workflow.IsCustomizable?.Value.ToString());

                builder.AppendFormat("    SolutionName {0}", EntityDescriptionHandler.GetAttributeString(workflow, "solution.uniquename"));

                if (withUrls)
                {
                    builder.AppendFormat("    Url {0}", _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Workflow, workflow.Id, null, null));
                }

                return builder.ToString();
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        private void CreateOneWorkFlowDescription(Workflow entity, FormatTextTableHandler handler, bool withUrls)
        {
            //"workflowid", "name", "uniquename", "businessprocesstype", "category", "mode", "primaryentity", "scope"

            string name = entity.Name;

            var uniqueName = entity.UniqueName;

            if (!string.IsNullOrEmpty(uniqueName))
            {
                name += string.Format("    (UniqueName \"{0}\")", uniqueName);
            }

            string primaryentity = entity.PrimaryEntity;
            string category = string.Empty;
            string businessprocesstype = string.Empty;
            string scope = string.Empty;
            string mode = string.Empty;
            string statusCodeString = entity.FormattedValues[Workflow.Schema.Attributes.statuscode];

            if (entity.Contains(Workflow.Schema.Attributes.category) && entity.Attributes[Workflow.Schema.Attributes.category] != null)
            {
                //var option = entity.Category.Value;

                //category = GetWorkFlowCategory(option);

                category = entity.FormattedValues[Workflow.Schema.Attributes.category];
            }

            //if (entity.Contains("uniquename") && entity.Attributes["uniquename"] != null)
            //{
            //    string uniquename = entity.UniqueName;

            //    if (!string.IsNullOrEmpty(uniquename))
            //    {
            //        builder.AppendFormat("; UniqueName: '{0}'", uniquename);
            //    }
            //}

            if (entity.Contains(Workflow.Schema.Attributes.businessprocesstype) && entity.Attributes[Workflow.Schema.Attributes.businessprocesstype] != null)
            {
                //var option = entity.BusinessProcessType.Value;

                //businessprocesstype = GetWorkFlowBusinessProcessType(option);

                businessprocesstype = entity.FormattedValues[Workflow.Schema.Attributes.businessprocesstype];
            }

            if (entity.Contains(Workflow.Schema.Attributes.scope) && entity.Attributes[Workflow.Schema.Attributes.scope] != null)
            {
                scope = entity.FormattedValues[Workflow.Schema.Attributes.scope];
            }

            if (entity.Contains(Workflow.Schema.Attributes.mode) && entity.Attributes[Workflow.Schema.Attributes.mode] != null)
            {
                mode = entity.FormattedValues[Workflow.Schema.Attributes.mode];
            }

            handler.AddLine(primaryentity
                , category
                , name
                , businessprocesstype
                , scope
                , mode
                , statusCodeString
                , entity.IsManaged.ToString()
                , entity.IsCustomizable?.Value.ToString()
                , EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename")
                , EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged")
                , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename")
                , EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged")
                , withUrls ? _service.ConnectionData?.GetSolutionComponentUrl(ComponentType.Workflow, entity.Id, null, null) : string.Empty
            );
        }

        public override string GetDisplayName(SolutionComponent component)
        {
            var entity = GetEntity<Workflow>(component.ObjectId.Value);

            if (entity != null)
            {
                return entity.UniqueName;
            }

            return base.GetDisplayName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { Workflow.Schema.Attributes.primaryentity, "EntityName" }
                    , { Workflow.Schema.Attributes.category, "Category" }
                    , { Workflow.Schema.Attributes.name, "Name" }
                    , { Workflow.Schema.Attributes.uniquename, "UniqueName" }
                    , { Workflow.Schema.Attributes.businessprocesstype, "Type" }
                    , { Workflow.Schema.Attributes.scope, "Scope" }
                    , { Workflow.Schema.Attributes.mode, "Mode" }
                    , { Workflow.Schema.Attributes.statuscode, "StatusCode" }
                    , { Workflow.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { Workflow.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}