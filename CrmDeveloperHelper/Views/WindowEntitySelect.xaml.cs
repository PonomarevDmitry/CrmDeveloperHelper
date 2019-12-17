using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public abstract partial class WindowEntitySelect : WindowBase
    {
        protected readonly IWriteToOutput _iWriteToOutput;

        protected readonly string _entityName;

        protected readonly ConnectionData _connectionData;

        public WindowEntitySelect(
            IWriteToOutput outputWindow
            , ConnectionData connectionData
            , string entityName
            , IEnumerable<DataGridColumn> columns
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowEntitySelect_{0}", entityName);
            lstVwEntities.Name = string.Format("lstVwEntities{0}", entityName);

            this._entityName = entityName;
            this._connectionData = connectionData;
            this._iWriteToOutput = outputWindow;

            foreach (var column in columns)
            {
                lstVwEntities.Columns.Add(column);
            }

            this.tSSLblConnectionName.Content = this._connectionData.Name;

            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.Focus();
        }

        protected abstract Task ShowExistingEntities();

        protected void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(_connectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.btnSelectEntity);

            UpdateButtonsEnable();
        }

        protected void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwEntities.SelectedItems.Count > 0;

                    UIElement[] list = { btnSelectEntity };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingEntities();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Entity item = GetItemFromRoutedDataContext<Entity>(e);

                if (item != null)
                {
                    SelectEntityAction(item);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected abstract void SelectEntityAction(Entity Entity);

        protected abstract void btnSelectEntity_Click(object sender, RoutedEventArgs e);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenEntityInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            _connectionData.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
        }

        private void mICopyEntityInstanceIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            ClipboardHelper.SetText(entity.Id.ToString());
        }

        private void mICopyEntityInstanceUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            var url = _connectionData.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

            ClipboardHelper.SetText(url);
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _connectionData.OpenEntityMetadataInWeb(_entityName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _connectionData.OpenEntityInstanceListInWeb(_entityName);
        }
    }
}