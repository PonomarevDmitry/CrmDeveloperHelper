using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class OrganizationServiceExtentedLocker : IDisposable
    {
        private List<IOrganizationServiceExtented> _serviceList = new List<IOrganizationServiceExtented>();

        public OrganizationServiceExtentedLocker()
        {

        }

        public OrganizationServiceExtentedLocker(IOrganizationServiceExtented service)
        {
            Lock(service);
        }

        public void Lock(IOrganizationServiceExtented service)
        {
            if (service == null)
            {
                return;
            }

            this._serviceList.Add(service);

            service.TryingDispose -= this.service_TringDispose;
            service.TryingDispose -= this.service_TringDispose;
            service.TryingDispose += this.service_TringDispose;
        }

        private void service_TringDispose(object sender, TryDisposeOrganizationServiceExtentedEventArgs e)
        {
            e.PreventDispose();
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

            if (disposing)
            {
                if (this._serviceList != null)
                {
                    foreach (var service in this._serviceList)
                    {
                        service.TryingDispose -= this.service_TringDispose;
                        service.TryingDispose -= this.service_TringDispose;

                        service.TryDispose();
                    }

                    this._serviceList.Clear();
                }
            }

            this._serviceList = null;
        }

        ~OrganizationServiceExtentedLocker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
