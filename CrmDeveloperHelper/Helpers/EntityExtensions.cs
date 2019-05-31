using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class EntityExtensions
    {
        public static T Merge<T>(this T target, params T[] additionalEntities) where T : Entity
        {
            if (additionalEntities == null)
            {
                return target;
            }

            if (additionalEntities.Length == 0)
            {
                return target;
            }

            foreach (var item in additionalEntities)
            {
                foreach (var attrKey in item.Attributes.Keys.Where(attrKey => !target.Contains(attrKey)))
                {
                    target.Attributes.Add(attrKey, item[attrKey]);
                }
            }

            return target;
        }

        public static bool IsValidEntityName(this string entityName)
        {
            if (!string.IsNullOrEmpty(entityName)
                && !string.Equals(entityName, "none", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return true;
            }

            return false;
        }
    }
}
