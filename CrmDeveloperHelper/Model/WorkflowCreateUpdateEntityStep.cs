using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WorkflowCreateUpdateEntityStep
    {
        public string DisplayName { get; set; }

        public string EntityName { get; set; }

        //public string EntityId { get; set; }

        public WorkflowCreateUpdateEntityStepType StepType { get; set; } = WorkflowCreateUpdateEntityStepType.Create;

        public List<WorkflowSetEntityPropertyStep> SetEntityPropertySteps { get; private set; } = new List<WorkflowSetEntityPropertyStep>();
    }
}