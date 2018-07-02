using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormTab
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShowLabel { get; set; }

        public string Visible { get; set; }

        public string Expanded { get; set; }

        public List<LabelString> Labels { get; private set; }

        public List<FormSection> Sections { get; private set; }

        public FormTab()
        {
            this.Labels = new List<LabelString>();
            this.Sections = new List<FormSection>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}