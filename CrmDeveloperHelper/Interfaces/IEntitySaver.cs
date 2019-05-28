using Microsoft.Xrm.Sdk;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces
{
    public interface IEntitySaver
    {
        Task<Guid> UpsertAsync(Entity entity, Action<string> updateStatus);
    }
}
