using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SystemUser
    {
        public override string ToString()
        {
            return this.FullName ?? Id.ToString("B");
        }

        private static SystemUser _emptySystemUser = null;

        public static SystemUser EmptySystemUser
        {
            get
            {
                if (_emptySystemUser == null)
                {
                    _emptySystemUser = new SystemUser() { Id = Guid.Empty };
                }

                return _emptySystemUser;
            }
        }
    }
}