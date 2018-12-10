using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class RibbonLocation
    {
        public string Id { get; set; }

        public List<RibbonLocationControl> Controls { get; private set; } = new List<RibbonLocationControl>();
    }
}
