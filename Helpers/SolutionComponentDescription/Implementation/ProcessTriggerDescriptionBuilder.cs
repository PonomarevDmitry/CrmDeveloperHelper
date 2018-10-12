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
    public class ProcessTriggerDescriptionBuilder : DefaultSolutionComponentDescriptionBuilder
    {
        public ProcessTriggerDescriptionBuilder(IOrganizationServiceExtented service)
            : base(service, (int)ComponentType.ProcessTrigger)
        {

        }

        public override ComponentType? ComponentTypeEnum => ComponentType.ProcessTrigger;

        public override int ComponentTypeValue => (int)ComponentType.ProcessTrigger;

        public override string EntityLogicalName => ProcessTrigger.EntityLogicalName;

        public override string EntityPrimaryIdAttribute => ProcessTrigger.Schema.EntityPrimaryIdAttribute;

        protected override ColumnSet GetColumnSet()
        {
            return new ColumnSet
                (
                    ProcessTrigger.Schema.Attributes.primaryentitytypecode
                    , ProcessTrigger.Schema.Attributes.processid
                    , ProcessTrigger.Schema.Attributes.Event
                    , ProcessTrigger.Schema.Attributes.pipelinestage
                    , ProcessTrigger.Schema.Attributes.formid
                    , ProcessTrigger.Schema.Attributes.scope
                    , ProcessTrigger.Schema.Attributes.methodid
                    , ProcessTrigger.Schema.Attributes.controlname
                    , ProcessTrigger.Schema.Attributes.controltype
                    , ProcessTrigger.Schema.Attributes.iscustomizable
                    , ProcessTrigger.Schema.Attributes.ismanaged
                );
        }

        public override void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            var list = GetEntities<ProcessTrigger>(components.Select(c => c.ObjectId));

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
            handler.SetHeader("PrimaryEntityTypeCode", "ProcessName", "Event", "PipelineStage", "FormName", "Scope", "MethodId", "ControlName", "ControlType", "IsCustomizable", "IsManaged"
                , "SolutionName", "SolutionIsManaged", "SupportingName", "SupportinIsManaged");

            foreach (var entity in list)
            {
                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.pipelinestage, out string pipelinestage);

                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.scope, out string scope);

                entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.controltype, out string controltype);

                handler.AddLine(
                    entity?.PrimaryEntityTypeCode
                    , entity?.ProcessId?.Name
                    , entity?.Event
                    , pipelinestage
                    , entity?.FormId?.Name
                    , scope
                    , entity?.MethodId?.ToString()
                    , entity?.ControlName
                    , controltype
                    , entity?.IsCustomizable?.Value.ToString()
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
            var processTrigger = GetEntity<ProcessTrigger>(component.ObjectId.Value);

            if (processTrigger != null)
            {
                processTrigger.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.pipelinestage, out string pipelinestage);

                processTrigger.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.scope, out string scope);

                processTrigger.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.controltype, out string controltype);

                return string.Format("PrimaryEntityTypeCode {0}    ProcessName {1}    Event {2}    PipelineStage {3}        FormName {4}        Scope {5}        MethodId {6}        ControlName {7}        ControlType {8}        IsManaged {9}        SolutionName {10}"
                    , processTrigger.PrimaryEntityTypeCode
                    , processTrigger.ProcessId?.Name
                    , processTrigger.Event
                    , pipelinestage
                    , processTrigger.FormId?.Name
                    , scope
                    , processTrigger.MethodId?.ToString()
                    , processTrigger.ControlName
                    , controltype
                    , processTrigger.IsManaged.ToString()
                    , EntityDescriptionHandler.GetAttributeString(processTrigger, "solution.uniquename")
                    );
            }

            return base.GenerateDescriptionSingle(component, withUrls);
        }

        public override string GetName(SolutionComponent component)
        {
            var entity = GetEntity<ProcessTrigger>(component.ObjectId.Value);

            if (entity != null)
            {
                return string.Format("{0} {1}"
                    , entity.PrimaryEntityTypeCode
                    , entity.ProcessId?.Name
                    );
            }

            return base.GetName(component);
        }

        public override TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>
                {
                    { ProcessTrigger.Schema.Attributes.primaryentitytypecode, "PrimaryEntityTypeCode" }
                    , { ProcessTrigger.Schema.Attributes.processid, "ProcessName" }
                    , { ProcessTrigger.Schema.Attributes.Event, "Event" }
                    , { ProcessTrigger.Schema.Attributes.pipelinestage, "PipelineStage" }
                    , { ProcessTrigger.Schema.Attributes.formid, "FormName" }
                    , { ProcessTrigger.Schema.Attributes.scope, "Scope" }
                    , { ProcessTrigger.Schema.Attributes.methodid, "MethodId" }
                    , { ProcessTrigger.Schema.Attributes.controlname, "ControlName" }
                    , { ProcessTrigger.Schema.Attributes.controltype, "ControlType" }
                    , { ProcessTrigger.Schema.Attributes.iscustomizable, "IsCustomizable" }
                    , { ProcessTrigger.Schema.Attributes.ismanaged, "IsManaged" }
                    , { "solution.uniquename", "SolutionName" }
                    , { "solution.ismanaged", "SolutionIsManaged" }
                    , { "suppsolution.uniquename", "SupportingName" }
                    , { "suppsolution.ismanaged", "SupportingIsManaged" }
                };
        }
    }
}