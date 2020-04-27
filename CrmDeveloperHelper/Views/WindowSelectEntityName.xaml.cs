using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityName : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        public string EntityTypeName { get; private set; }

        public int? EntityTypeCode { get; private set; }

        public WindowSelectEntityName(
            ConnectionData connectionData
            , string windowTitle
        )
        {
            InitializeComponent();

            this.Title = windowTitle;

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            LoadEntityNames(connectionData);

            cmBConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBConnection.SelectedItem = connectionData;

            cmBEntityTypeNameOrCode.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityNames(ConnectionData connectionData)
        {
            cmBEntityTypeNameOrCode.Dispatcher.Invoke(() =>
            {
                LoadEntityNames(cmBEntityTypeNameOrCode, connectionData);
            });
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
            StringBuilder message = new StringBuilder();

            string entityName = string.Empty;
            int? entityTypeCode = null;

            string text = cmBEntityTypeNameOrCode.Text?.Trim(' ', '<', '>');

            if (!string.IsNullOrEmpty(text))
            {
                if (int.TryParse(text, out int tempInt))
                {
                    entityTypeCode = tempInt;
                }
                else
                {
                    entityName = text;
                }
            }

            if (string.IsNullOrEmpty(entityName) && !entityTypeCode.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.EntityNameIsEmpty);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.EntityTypeName = entityName;
            this.EntityTypeCode = entityTypeCode;

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
            LoadEntityNames(GetConnectionData());
        }
    }
}
