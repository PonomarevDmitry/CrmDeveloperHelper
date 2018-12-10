using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class SolutionImageComponentSorter : IComparer<SolutionImageComponent>
    {
        public int Compare(SolutionImageComponent x, SolutionImageComponent y)
        {
            if (x == null) return -1;

            if (y == null) return 1;

            {
                var result = x.ComponentType.CompareTo(y.ComponentType);

                if (result != 0)
                {
                    return result;
                }
            }

            {
                var result = string.Compare(x.ParentSchemaName, y.ParentSchemaName, true);

                if (result != 0)
                {
                    return result;
                }
            }

            {
                var result = string.Compare(x.SchemaName, y.SchemaName, true);

                if (result != 0)
                {
                    return result;
                }
            }

            {
                var result = string.Compare(x.ObjectId.ToString(), y.ObjectId.ToString(), true);

                if (result != 0)
                {
                    return result;
                }
            }

            return string.Compare(x.Description, y.Description, true);
        }
    }
}
