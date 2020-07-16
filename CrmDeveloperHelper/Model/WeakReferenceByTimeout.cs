using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class WeakReferenceByTimeOut<T> : IDisposable where T : class
    {
        private readonly TimeSpan _timeout;

        private T _value;

        private Task _disposeTask;
        private CancellationTokenSource _cancellationTokenSource;

        public WeakReferenceByTimeOut(T value, TimeSpan timeout)
        {
            this._value = value;
            this._timeout = timeout;

            StartNewDisposeTask();
        }

        private void StartNewDisposeTask()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._disposeTask = Task.Delay(this._timeout, _cancellationTokenSource.Token);

            _disposeTask.ContinueWith(DisposeWeakReferenceByTimeout, _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        public bool TryGetTarget(out T target)
        {
            if (!this._cancellationTokenSource.IsCancellationRequested)
            {
                this._cancellationTokenSource.Cancel();
            }

            if (disposedValue || this._value == null)
            {
                target = null;
                return false;
            }

            StartNewDisposeTask();

            target = this._value;
            return true;
        }

        private void DisposeWeakReferenceByTimeout(Task task)
        {
            if (task.IsCanceled)
            {
                return;
            }

            if (disposedValue)
            {
                return;
            }

            this.Dispose(true);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            disposedValue = true;

            if (!disposing)
            {
                return;
            }

            if (!this._cancellationTokenSource.IsCancellationRequested)
            {
                this._cancellationTokenSource.Cancel();
            }

            this._disposeTask?.Dispose();
            this._cancellationTokenSource?.Dispose();

            this._disposeTask = null;
            this._cancellationTokenSource = null;
            this._value = null;
        }

        ~WeakReferenceByTimeOut()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
