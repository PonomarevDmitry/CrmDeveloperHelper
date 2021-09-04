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
    public class WindowEntitySelect<T> : WindowEntitySelect where T : Entity
    {
        private readonly ObservableCollection<T> _itemsSource;

        private readonly Func<string, Task<IEnumerable<T>>> _listGetter;

        public T SelectedEntity { get; private set; }

        public WindowEntitySelect(
            IWriteToOutput outputWindow
            , ConnectionData connectionData
            , string entityName
            , Func<string, Task<IEnumerable<T>>> listGetter
            , IEnumerable<DataGridColumn> columns
        ) : base(outputWindow, connectionData, entityName, columns)
        {
            this._listGetter = listGetter;

            this._itemsSource = new ObservableCollection<T>();

            this.lstVwEntities.ItemsSource = _itemsSource;

            var task = ShowExistingEntities();
        }

        protected override async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingEntities);

            string filter = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
                filter = txtBFilterEnitity.Text.Trim();
            });

            IEnumerable<T> list = Enumerable.Empty<T>();

            try
            {
                if (_listGetter != null)
                {
                    list = await _listGetter(filter);

                    SwitchEntityDatesToLocalTime(list);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(_connectionData, ex);

                list = Enumerable.Empty<T>();
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

            ToggleControls(true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, list.Count());
        }

        private T GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.Cast<T>().Count() == 1
                ? this.lstVwEntities.SelectedItems.Cast<T>().SingleOrDefault() : null;
        }

        protected override void SelectEntityAction(Entity Entity)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (Entity == null)
            {
                return;
            }

            this.SelectedEntity = Entity.ToEntity<T>();

            this.DialogResult = true;

            this.Close();
        }

        protected override void btnSelectEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            SelectEntityAction(entity);
        }
    }
}
