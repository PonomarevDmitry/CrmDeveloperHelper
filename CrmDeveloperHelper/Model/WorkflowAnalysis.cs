using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WorkflowAnalysis
    {
        public List<WorkflowCreateUpdateEntityStep> CreateUpdateEntitySteps { get; private set; } = new List<WorkflowCreateUpdateEntityStep>();

        public List<WorkflowGetEntityPropertyStep> UsedEntityAttributes { get; private set; } = new List<WorkflowGetEntityPropertyStep>();
    }
}
