using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithSingleConnection : WindowWithOutput
    {
        protected readonly IOrganizationServiceExtented _service;

        protected WindowWithSingleConnection(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
        ) : base(iWriteToOutput)
        {
            this._service = service;

            ForbidDisposing(_service);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            AllowDisposingAndTryDisposeService(_service);
        }

        protected abstract void ToggleControls(bool enabled, string statusFormat, params object[] args);
    }
}