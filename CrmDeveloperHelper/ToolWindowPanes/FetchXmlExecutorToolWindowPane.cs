using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.ToolWindowPanes
{
    [Guid(PackageGuids.guidCrmDeveloperHelperPackageFetchXmlExecutorToolWindowPaneString)]
    public class FetchXmlExecutorToolWindowPane : ToolWindowPane
    {
        private FetchXmlExecutorControl _control;

        public string FilePath => this._control.FilePath;

        public FetchXmlExecutorToolWindowPane()
        {
            this.Caption = Properties.CommandNames.FetchXmlExecutorDefaultName;

            this._control = new FetchXmlExecutorControl();

            this._control.ConnectionChanged += _control_ConnectionChanged;

            this.Content = this._control;
        }

        private void _control_ConnectionChanged(object sender, EventArgs e)
        {
            SetCaption();
        }

        public void SetSource(string filePath, ConnectionData connectionData)
        {
            _control.SetSource(filePath, connectionData);

            SetCaption();
        }

        private void SetCaption()
        {
            this.Caption = string.Format(Properties.CommandNames.FetchXmlExecutorNameFormat2, Path.GetFileName(FilePath), this._control.ConnectionData?.Name);
        }

        public void Execute() => this._control.Execute();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _control.DetachFromSourceCollection();
            }

            base.Dispose(disposing);
        }
    }
}