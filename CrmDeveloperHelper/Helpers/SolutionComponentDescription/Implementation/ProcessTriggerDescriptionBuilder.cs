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

        protected override FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            FormatTextTableHandler handler = new FormatTextTableHandler();
            handler.SetHeader("PrimaryEntityTypeCode", "ProcessName", "Event", "PipelineStage", "FormName", "Scope", "MethodId", "ControlName", "ControlType", "IsCustomizable", "Behavior");

            action(handler, withUrls, withManaged, withSolutionInfo);

            return handler;
        }

        protected override List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var entity = entityInput.ToEntity<ProcessTrigger>();

            List<string> values = new List<string>();

            entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.pipelinestage, out string pipelinestage);

            entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.scope, out string scope);

            entity.FormattedValues.TryGetValue(ProcessTrigger.Schema.Attributes.controltype, out string controltype);

            values.AddRange(new[]
            {
                entity.PrimaryEntityTypeCode
                , entity.ProcessId?.Name
                , entity.Event
                , pipelinestage
                , entity.FormId?.Name
                , scope
                , entity.MethodId?.ToString()
                , entity.ControlName
                , controltype
                , entity.IsCustomizable?.Value.ToString()
                , behavior
            });

            action(values, entity, withUrls, withManaged, withSolutionInfo);

            return values;
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