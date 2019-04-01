using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PrivilegeEqualityComparer : IEqualityComparer<Privilege>
    {
        public bool Equals(Privilege x, Privilege y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(x.Name, y.Name);
        }

        public int GetHashCode(Privilege obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
