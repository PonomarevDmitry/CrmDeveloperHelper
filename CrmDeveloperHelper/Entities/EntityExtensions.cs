using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public static class EntityExtensions
    {
        public static T Merge<T>(params T[] targets) where T : Entity
        {
            if (targets == null)
            {
                return null;
            }

            if (targets.Length == 0)
            {
                return null;
            }

            var target = targets.First();

            foreach (var item in targets.Skip(1))
            {
                foreach (var attrKey in item.Attributes.Keys.Where(attrKey => !target.Contains(attrKey)))
                {
                    target.Attributes.Add(attrKey, item[attrKey]);
                }
            }

            return target;
        }
    }
}
