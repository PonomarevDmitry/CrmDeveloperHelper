using Microsoft.Xrm.Sdk.Query;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    public static class ColumnSetInstances
    {
        public static ColumnSet AllColumns { get; private set; }

        public static ColumnSet None { get; private set; }

        static ColumnSetInstances()
        {
            AllColumns = new ColumnSet(true);

            None = new ColumnSet(false);
        }
    }
}
