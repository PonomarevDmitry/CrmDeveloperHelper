using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WorkflowEntityFieldString
    {
        public string StepName { get; private set; }

        public string OriginalString { get; private set; }

        public List<string> EntityFields { get; private set; }

        public WorkflowEntityFieldString(string stepName, string originalString)
        {
            this.StepName = stepName;
            this.OriginalString = originalString;

            this.EntityFields = new List<string>();
        }
    }
}
