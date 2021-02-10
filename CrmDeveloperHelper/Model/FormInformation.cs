using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormInformation
    {
        public Guid? FormId { get; set; }

        public string FormName { get; set; }

        public int? FormType { get; set; }

        public string FormTypeName { get; set; }

        public FormTab Header { get; set; }

        public FormTab Footer { get; set; }

        public List<FormTab> Tabs { get; private set; }

        public List<FormParameter> FormParameters { get; private set; }

        public FormInformation()
        {
            this.Tabs = new List<FormTab>();
            this.FormParameters = new List<FormParameter>();
        }
    }
}
