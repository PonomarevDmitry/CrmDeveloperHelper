using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class LabelDifferenceResult
    {
        public List<LabelString> LabelsOnlyIn1 { get; private set; }

        public List<LabelString> LabelsOnlyIn2 { get; private set; }

        public List<LabelStringDifference> LabelDifference { get; private set; }

        public LabelDifferenceResult()
        {
            this.LabelsOnlyIn1 = new List<LabelString>();
            this.LabelsOnlyIn2 = new List<LabelString>();
            this.LabelDifference = new List<LabelStringDifference>();
        }

        public bool IsEmpty
        {
            get
            {
                return this.LabelDifference.Count == 0 && this.LabelsOnlyIn1.Count == 0 && this.LabelsOnlyIn2.Count == 0;
            }
        }
    }
}