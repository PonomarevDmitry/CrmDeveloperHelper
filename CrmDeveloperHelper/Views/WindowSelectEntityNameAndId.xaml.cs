using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityNameAndId : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        public string EntityTypeName { get; private set; }

        public int? EntityTypeCode { get; private set; }

        public Guid EntityId { get; private set; }

        public WindowSelectEntityNameAndId(
            ConnectionData connectionData
            , string windowTitle
            , string selectionText
        )
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this.Title = windowTitle;

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            LoadEntityNames(cmBEntityTypeNameOrCode, connectionData);

            cmBEntityTypeNameOrCode.Text = selectionText;

            cmBConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBConnection.SelectedItem = connectionData;

            txtBEntityId.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                MakeOkClick();
            }

            base.OnKeyDown(e);
        }

        private void MakeOkClick()
        {
            var message = new StringBuilder();

            string entityName = string.Empty;
            int? entityTypeCode = null;
            Guid? entityId = null;

            {
                string textEntityTypeNameOrCode = cmBEntityTypeNameOrCode.Text?.Trim(' ', '<', '>');

                if (!string.IsNullOrEmpty(textEntityTypeNameOrCode))
                {
                    if (int.TryParse(textEntityTypeNameOrCode, out int tempInt))
                    {
                        entityTypeCode = tempInt;
                    }
                    else
                    {
                        entityName = textEntityTypeNameOrCode;
                    }
                }
            }

            {
                string textEntityId = txtBEntityId.Text?.Trim(' ', '<', '>');

                if (!string.IsNullOrEmpty(textEntityId) && Guid.TryParse(textEntityId, out Guid tempGuid))
                {
                    entityId = tempGuid;
                }
            }

            if (string.IsNullOrEmpty(entityName) && !entityTypeCode.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.EntityNameIsEmpty);
            }

            if (!entityId.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.CannotParseGuid);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.EntityTypeName = entityName;
            this.EntityTypeCode = entityTypeCode;
            this.EntityId = entityId.Value;

            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        public ConnectionData GetConnectionData()
        {
            ConnectionData connectionData = null;

            this.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private void cmBConnection_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadEntityNames(cmBEntityTypeNameOrCode, GetConnectionData());
        }
    }
}
