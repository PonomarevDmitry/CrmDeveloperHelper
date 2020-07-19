using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithOutput : WindowBase
    {
        protected readonly IWriteToOutput _iWriteToOutput;

        protected WindowWithOutput(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput ?? throw new ArgumentNullException(nameof(iWriteToOutput));
        }
    }
}