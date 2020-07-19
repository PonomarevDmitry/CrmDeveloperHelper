using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
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

        public ConnectionData ConnectionData => this._control.ConnectionData;

        public FetchXmlExecutorToolWindowPane()
        {
            this.Caption = Properties.CommandNames.FetchXmlExecutorDefaultName;

            this._control = new FetchXmlExecutorControl();

            this._control.ConnectionChanged += _control_ConnectionChanged;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Content = this._control;
        }

        private void _control_ConnectionChanged(object sender, EventArgs e)
        {
            SetCaption();
        }

        public void SetSource(string filePath, ConnectionData connectionData, IWriteToOutput iWriteToOutput)
        {
            _control.SetSource(filePath, connectionData, iWriteToOutput);

            SetCaption();
        }

        private void SetCaption()
        {
            this.Caption = string.Format(Properties.CommandNames.FetchXmlExecutorNameFormat2, Path.GetFileName(FilePath), this._control.ConnectionData?.Name);
        }

        public void Execute()
        {
            var task = this._control.Execute();
        }

        protected override void OnClose()
        {
            base.OnClose();

            _control.DetachFromSourceCollection();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _control.DetachFromSourceCollection();
            }
        }
    }
}