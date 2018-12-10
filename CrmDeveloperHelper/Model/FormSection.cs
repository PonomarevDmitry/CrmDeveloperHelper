using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormSection
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShowLabel { get; set; }

        public string Visible { get; set; }

        public List<LabelString> Labels { get; private set; }

        public List<FormControl> Controls { get; private set; }

        public FormSection()
        {
            this.Labels = new List<LabelString>();
            this.Controls = new List<FormControl>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}