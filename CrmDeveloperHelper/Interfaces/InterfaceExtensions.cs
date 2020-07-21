using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public static class InterfaceExtensions
    {
        public static OrganizationServiceExtentedLocker Lock(this IOrganizationServiceExtented service)
        {
            return new OrganizationServiceExtentedLocker(service);
        }
    }
}
