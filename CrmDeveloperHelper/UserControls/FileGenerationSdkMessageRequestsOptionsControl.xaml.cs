using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class FileGenerationSdkMessageRequestsOptionsControl : UserControl
    {
        public FileGenerationSdkMessageRequestsOptionsControl()
        {
            InitializeComponent();
        }

        public void BindFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            chBSdkMessageRequestAttributesWithNameOf.DataContext = fileGenerationOptions;
            chBSdkMessageRequestMakeAllPropertiesEditable.DataContext = fileGenerationOptions;
            chBSdkMessageRequestWithDebuggerNonUserCode.DataContext = fileGenerationOptions;

            txtBNamespaceSdkMessagesCSharp.DataContext = fileGenerationOptions;
            txtBNamespaceSdkMessagesJavaScript.DataContext = fileGenerationOptions;
        }

        public event EventHandler<EventArgs> CloseClicked;

        private void OnCloseClicked()
        {
            CloseClicked?.Invoke(this, new EventArgs());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    e.Handled = true;

                    OnCloseClicked();

                    return;
                }
            }

            base.OnKeyDown(e);
        }
    }
}