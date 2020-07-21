using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithSingleConnection : WindowWithOutput
    {
        protected readonly IOrganizationServiceExtented _service;
        private readonly OrganizationServiceExtentedLocker _serviceLock;

        protected WindowWithSingleConnection(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput)
        {
            this._service = service;

            this._serviceLock = _service.Lock();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _serviceLock.Dispose();
        }

        protected abstract void ToggleControls(bool enabled, string statusFormat, params object[] args);
    }
}