using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowEntitySelect : WindowBase
    {
        private IWriteToOutput _iWriteToOutput;

        private bool _controlsEnabled = true;

        private ObservableCollection<Entity> _itemsSource;

        private Func<string, Task<IEnumerable<Entity>>> _listGetter;

        private readonly string _entityName;

        private readonly ConnectionData _connectionData;

        public Entity SelectedEntity { get; private set; }

        public WindowEntitySelect(
            IWriteToOutput outputWindow
            , ConnectionData connectionData
            , string entityName
            , Func<string, Task<IEnumerable<Entity>>> listGetter
            , IEnumerable<DataGridColumn> columns
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowEntitySelect_{0}", entityName);
            lstVwEntities.Name = string.Format("lstVwEntities{0}", entityName);

            this._listGetter = listGetter;
            this._entityName = entityName;
            this._connectionData = connectionData;
            this._iWriteToOutput = outputWindow;

            foreach (var column in columns)
            {
                lstVwEntities.Columns.Add(column);
            }

            this.tSSLblConnectionName.Content = this._connectionData.Name;

            this._itemsSource = new ObservableCollection<Entity>();

            this.lstVwEntities.ItemsSource = _itemsSource;

            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.Focus();

            ShowExistingEntities();
        }

        private async Task ShowExistingEntities()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntities);

            this._itemsSource.Clear();

            string filter = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                filter = txtBFilterEnitity.Text.Trim();
            });

            IEnumerable<Entity> list = Enumerable.Empty<Entity>();

            try
            {
                if (_listGetter != null)
                {
                    list = await _listGetter(filter);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                list = Enumerable.Empty<Entity>();
            }

            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list)
                {
                    _itemsSource.Add(entity);
                }

                if (_itemsSource.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, list.Count());
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(this.btnSelectEntity, enabled);

            ToggleProgressBar(enabled);

            if (enabled)
            {
                UpdateButtonsEnable();
            }
        }

        private void ToggleProgressBar(bool enabled)
        {
            if (tSProgressBar == null)
            {
                return;
            }

            this.tSProgressBar.Dispatcher.Invoke(() =>
            {
                tSProgressBar.IsIndeterminate = !enabled;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                if (c is TextBox)
                {
                    ((TextBox)c).IsReadOnly = !enabled;
                }
                else
                {
                    c.IsEnabled = enabled;
                }
            });
        }

        private void UpdateButtonsEnable()
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

        private Entity GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<Entity>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<Entity>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is Entity item)
                {
                    SelectEntityAction(item);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void SelectEntityAction(Entity Entity)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            if (Entity == null)
            {
                return;
            }

            this.SelectedEntity = Entity;

            this.DialogResult = true;

            this.Close();
        }

        private void btnSelectEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            SelectEntityAction(entity);
        }

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

            Clipboard.SetText(entity.Id.ToString());
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

            Clipboard.SetText(url);
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _connectionData.OpenEntityMetadataInWeb(_entityName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _connectionData.OpenEntityListInWeb(_entityName);
        }
    }
}