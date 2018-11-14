using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WorkflowSetEntityPropertyStep
    {
        public string EntityName { get; set; }

        public string Attribute { get; set; }

        //public string Value { get; set; }

        //public WorkflowSetEntityPropertyStepType StepType { get; set; } = WorkflowSetEntityPropertyStepType.Nullify;
    }
}
