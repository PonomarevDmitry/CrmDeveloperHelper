using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract class WindowWithOutputAndCommonConfig : WindowBase
    {
        protected readonly IWriteToOutput _iWriteToOutput;

        protected readonly CommonConfiguration _commonConfig;

        public WindowWithOutputAndCommonConfig(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput = iWriteToOutput ?? throw new ArgumentNullException(nameof(iWriteToOutput));
            this._commonConfig = commonConfig ?? throw new ArgumentNullException(nameof(commonConfig));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();
        }
    }
}
