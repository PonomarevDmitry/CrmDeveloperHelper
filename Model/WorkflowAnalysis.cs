using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WorkflowAnalysis
    {
        public List<WorkflowCreateUpdateEntityStep> CreateUpdateEntitySteps { get; private set; } = new List<WorkflowCreateUpdateEntityStep>();

        public List<WorkflowGetEntityPropertyStep> UsedEntityAttributes { get; private set; } = new List<WorkflowGetEntityPropertyStep>();
    }
}
