using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Privilege
    {
        public List<string> LinkedEntities { get; set; }

        public string LinkedEntitiesSorted
        {
            get
            {
                if (this.LinkedEntities != null)
                {
                    return string.Join(",", this.LinkedEntities);
                }

                return string.Empty;
            }
        }
    }
}