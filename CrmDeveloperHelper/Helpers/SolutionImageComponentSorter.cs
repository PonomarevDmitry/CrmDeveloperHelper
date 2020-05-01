using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class SolutionImageComponentSorter : IComparer<SolutionImageComponent>
    {
        private SolutionImageComponentSorter()
        {

        }

        public static SolutionImageComponentSorter Comparer { get; } = new SolutionImageComponentSorter();

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
