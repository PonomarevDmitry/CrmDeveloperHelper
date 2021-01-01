using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class FormGridTeamTemplate : FormControl
    {
        public string GridEntity { get; set; }

        public Guid TeamTemplateId { get; set; }

        public FormGridTeamTemplate()
        {
        }
    }
}
