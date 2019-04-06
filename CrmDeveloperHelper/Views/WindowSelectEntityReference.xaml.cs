using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectEntityReference : WindowBase
    {
        private readonly ConnectionData _connectionData;

        public EntityReference SelectedEntityReference { get; private set; }

        public WindowSelectEntityReference(
            ConnectionData connectionData
            , IEnumerable<string> entityNames
        )
        {
            InitializeComponent();

            this._connectionData = connectionData;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            cmBEntityName.Focus();

            LoadEntityNames(entityNames);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadEntityNames(IEnumerable<string> entityNames)
        {
            string text = cmBEntityName.Text;

            cmBEntityName.Items.Clear();

            foreach (var item in entityNames.OrderBy(s => s))
            {
                cmBEntityName.Items.Add(item);
            }

            cmBEntityName.Text = text;

            if (cmBEntityName.Items.Count == 1)
            {
                var item = cmBEntityName.Items[0].ToString();

                cmBEntityName.IsEnabled = false;

                cmBEntityName.SelectedIndex = 0;
                cmBEntityName.Text = item;

                txtBEntityId.Focus();
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

            string entityName = string.Empty;
            Guid? entityId = null;

            TryParseUrl(out var urlEntityName, out var urlObjectTypeCode, out var urlEntityId);

            string textEntityName = cmBEntityName.SelectedItem?.ToString().Trim(' ', '<', '>');
            string textEntityId = txtBEntityId.Text?.Trim(' ', '<', '>');

            if (!string.IsNullOrEmpty(textEntityName))
            {
                entityName = textEntityName;
            }

            if (string.IsNullOrEmpty(entityName)
                && !string.IsNullOrEmpty(urlEntityName)
                )
            {
                entityName = urlEntityName;
            }

            if (string.IsNullOrEmpty(entityName)
                && urlObjectTypeCode.HasValue
                )
            {
                if (_connectionData != null
                    && _connectionData.IntellisenseData != null
                    && _connectionData.IntellisenseData.Entities != null
                )
                {
                    var entityIntellisense = _connectionData.IntellisenseData.Entities.Values.FirstOrDefault(e => e.ObjectTypeCode == urlObjectTypeCode.Value);

                    if (entityIntellisense != null)
                    {
                        entityName = entityIntellisense.EntityLogicalName;
                    }
                }
            }

            if (string.IsNullOrEmpty(entityName))
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.EntityNameIsEmpty);
            }

            if (!string.IsNullOrEmpty(textEntityId) 
                && Guid.TryParse(textEntityId, out Guid tempGuid)
            )
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

                message.Append(Properties.MessageBoxStrings.CannotParseGuid);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.SelectedEntityReference = new EntityReference(entityName, entityId.Value);

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
