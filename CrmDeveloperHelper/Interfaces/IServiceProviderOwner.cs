using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IServiceProviderOwner
    {
        IServiceProvider ServiceProvider { get; }
    }
}