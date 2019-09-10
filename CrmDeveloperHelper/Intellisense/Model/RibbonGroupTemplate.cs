using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class RibbonGroupTemplate
    {
        public string Id { get; set; }

        public List<string> TemplateAliases { get; private set; } = new List<string>();
    }
}
