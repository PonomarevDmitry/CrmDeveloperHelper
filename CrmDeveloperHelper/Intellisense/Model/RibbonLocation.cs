using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class RibbonLocation
    {
        public string Id { get; set; }

        public RibbonGroupTemplate Template { get; set; }

        public List<RibbonLocationControl> Controls { get; private set; } = new List<RibbonLocationControl>();
    }
}
