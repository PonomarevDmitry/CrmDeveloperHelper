using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithOutputAndCommonConfig : WindowWithOutput
    {
        protected readonly CommonConfiguration _commonConfig;

        protected WindowWithOutputAndCommonConfig(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig)
            : base(iWriteToOutput)
        {
            this._commonConfig = commonConfig ?? throw new ArgumentNullException(nameof(commonConfig));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }
    }
}
