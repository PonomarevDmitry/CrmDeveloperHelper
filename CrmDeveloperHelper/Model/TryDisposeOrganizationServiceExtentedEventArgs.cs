using System;
using System.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class TryDisposeOrganizationServiceExtentedEventArgs : EventArgs
    {
        private readonly CancellationTokenSource _source;

        public bool IsDisposingCanceled => _source.Token.IsCancellationRequested;

        public TryDisposeOrganizationServiceExtentedEventArgs()
        {
            this._source = new CancellationTokenSource();
        }

        public void PreventDispose()
        {
            this._source.Cancel();
        }
    }
}