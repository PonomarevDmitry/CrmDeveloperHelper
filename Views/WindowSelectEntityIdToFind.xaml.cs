using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityIdToFind : WindowBase
    {
        private CommonConfiguration _commonConfig;

        public Guid EntityId { get; private set; }

        public string EntityTypeName { get; private set; }

        public int? EntityTypeCode { get; private set; }

        public WindowSelectEntityIdToFind(CommonConfiguration commonConfig, ConnectionData connectionData, string windowTitle)
        {
            InitializeComponent();

            this.Title = windowTitle;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._commonConfig = commonConfig;

            LoadConfigs(commonConfig, connectionData);

            txtBEntityId.Focus();
        }

        protected override void OnClosed(EventArgs e)
        {
            this._commonConfig.Save();

            base.OnClosed(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadConfigs(CommonConfiguration commonConfig, ConnectionData connectionData)
        {
            txtBFolder.DataContext = commonConfig;

            cmBFileAction.DataContext = commonConfig;

            if (connectionData.IntellisenseData != null
                && connectionData.IntellisenseData.Entities != null
                )
            {
                foreach (var item in connectionData.IntellisenseData.Entities.Keys.OrderBy(s => s))
                {
                    cmBEntityTypeName.Items.Add(item);
                }
            }
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

            string folder = txtBFolder.Text.Trim();

            if (!string.IsNullOrEmpty(folder))
            {
                if (!Directory.Exists(folder))
                {
                    if (message.Length > 0) { message.AppendLine(); }

                    message.Append(Properties.MessageBoxStrings.FolderDoesNotExists);
                }
            }
            else
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append("Folder or Text is empty.");
            }

            string entityName = string.Empty;
            int? entityTypeCode = null;
            Guid? entityId = null;

            TryParseUrl(out var urlEntityName, out var urlObjectTypeCode, out var urlEntityId);

            string textEntityName = cmBEntityTypeName.Text?.Trim(' ', '<', '>');
            string textEntityTypeCode = txtBEntityTypeCode.Text?.Trim(' ', '<', '>');
            string textEntityId = txtBEntityId.Text?.Trim(' ', '<', '>');

            if (!string.IsNullOrEmpty(textEntityName))
            {
                entityName = textEntityName;
            }
            else if (!string.IsNullOrEmpty(urlEntityName))
            {
                entityName = urlEntityName;
            }

            if (!string.IsNullOrEmpty(textEntityTypeCode) && int.TryParse(textEntityTypeCode, out int tempInt))
            {
                entityTypeCode = tempInt;
            }
            else if (urlObjectTypeCode.HasValue)
            {
                entityTypeCode = urlObjectTypeCode;
            }

            if (!string.IsNullOrEmpty(textEntityId) && Guid.TryParse(textEntityId, out Guid tempGuid))
            {
                entityId = tempGuid;
            }
            else if (urlEntityId.HasValue)
            {
                entityId = urlEntityId;
            }

            if (!entityId.HasValue)
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append("Cannot parse text to valid Guid.");
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

        private void TryParseUrl(out string urlEntityName, out int? urlObjectTypeCode, out Guid? urlEntityId)
        {
            urlObjectTypeCode = null;
            urlEntityName = string.Empty;
            urlEntityId = null;

            var textUrl = txtBEntityUrl.Text.Trim();

            var split = textUrl.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var url in split)
            {
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                var temp = url;

                temp = temp.Trim(' ', '<', '>');

                if (string.IsNullOrEmpty(temp))
                {
                    continue;
                }

                if (Uri.TryCreate(temp, UriKind.Absolute, out var uri))
                {
                    var query = HttpUtility.ParseQueryString(uri.Query);

                    if (query.AllKeys.Contains("id", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("id")) && Guid.TryParse(query.Get("id"), out Guid guid))
                        {
                            urlEntityId = guid;
                        }
                    }

                    if (query.AllKeys.Contains("etn", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("etn")))
                        {
                            urlEntityName = query.Get("etn");
                        }
                    }

                    if (query.AllKeys.Contains("etc", StringComparer.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(query.Get("etc")) && int.TryParse(query.Get("etc"), out int tempInt))
                        {
                            urlObjectTypeCode = tempInt;
                        }
                    }

                    return;
                }
            }
        }

        private void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }
    }
}
