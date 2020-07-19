using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls;
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
    public partial class WindowEntityBulkEditor : WindowWithSingleConnection
    {
        private readonly CommonConfiguration _commonConfig;

        protected readonly string _entityName;

        protected readonly HashSet<string> _ignoredAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        protected readonly IReadOnlyCollection<Guid> _entityIds;

        private EntityMetadata _entityMetadata;

        private readonly AttributeMetadataControlFactory _controlFactory = new AttributeMetadataControlFactory();

        private readonly List<UserControl> _listAttributeControls = new List<UserControl>();

        private readonly Func<AttributeMetadata, bool> _attributeChecker;

        public WindowEntityBulkEditor(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityName
            , IEnumerable<Guid> entityIds
        ) : base(iWriteToOutput, service)
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowEntityBulkEditor_{0}", entityName);

            this._commonConfig = commonConfig;

            this._entityName = entityName;

            this._entityIds = new ReadOnlyCollection<Guid>(entityIds.Where(i => i != Guid.Empty).Distinct().ToList());

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            WindowEntityEditor.FillIgnoredAttributes(_entityName, _ignoredAttributes);

            FillExplorersMenuItems();

            DecreaseInit();

            txtBFilterAttribute.SelectionStart = txtBFilterAttribute.Text.Length;
            txtBFilterAttribute.SelectionLength = 0;
            txtBFilterAttribute.Focus();

            this._attributeChecker = a => a.IsValidForUpdate.GetValueOrDefault() && !_ignoredAttributes.Contains(a.LogicalName);

            var task = RetrieveEntityInformation();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service)
                , getEntityName: () => this._entityName
            );

            explorersHelper.FillExplorers(miExplorers);
        }

        private async Task RetrieveEntityInformation()
        {
            try
            {
                ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, _entityName);

                var repositoryEntityMetadata = new EntityMetadataRepository(_service);

                this._entityMetadata = await repositoryEntityMetadata.GetEntityMetadataAsync(_entityName);

                ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, _entityName);

                FilterEntityAttributes(null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void FilterEntityAttributes(UserControl selectedControl)
        {
            ToggleControls(false, Properties.OutputStrings.FilteringAttributesFormat1, _entityName);

            this.lstVwAttributes.Dispatcher.Invoke(() =>
            {
                lstVwAttributes.RowDefinitions.Clear();
                lstVwAttributes.Children.Clear();
            });

            var list = _listAttributeControls.OfType<IAttributeMetadataControl<AttributeMetadata>>();

            var textName = string.Empty;

            this.txtBFilterAttribute.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterAttribute.Text.Trim();
            });

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.AttributeMetadata.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        (selectedControl != null && ent == selectedControl)
                        || ent.AttributeMetadata.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.AttributeMetadata.DisplayName != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                    );
                }
            }

            this.lstVwAttributes.Dispatcher.Invoke(() =>
            {
                int index = 0;

                foreach (var item in list.OrderBy(a => a.AttributeMetadata.LogicalName))
                {
                    if (item is UserControl control)
                    {
                        var itemRowDef = new RowDefinition()
                        {
                            Height = new GridLength(10, GridUnitType.Auto),
                        };

                        lstVwAttributes.RowDefinitions.Add(itemRowDef);

                        control.VerticalAlignment = VerticalAlignment.Stretch;
                        control.HorizontalAlignment = HorizontalAlignment.Stretch;

                        Grid.SetRow(control, index);
                        lstVwAttributes.Children.Add(control);

                        index++;

                        if (item is MemoAttributeMetadataControl
                            || item is MultiSelectPicklistAttributeMetadataControl
                        )
                        {
                            itemRowDef.Height = new GridLength(200, GridUnitType.Pixel);

                            lstVwAttributes.RowDefinitions.Add(new RowDefinition()
                            {
                                Height = new GridLength(5),
                            });

                            var splitter = new GridSplitter()
                            {
                                VerticalAlignment = VerticalAlignment.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Height = 5,
                            };

                            Grid.SetRow(splitter, index);
                            lstVwAttributes.Children.Add(splitter);

                            index++;
                        }
                    }
                }

                var last = lstVwAttributes.Children.OfType<UIElement>().LastOrDefault();

                if (last != null && last is GridSplitter)
                {
                    lstVwAttributes.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(30),
                    });
                }

                if (selectedControl != null)
                {
                    selectedControl.BringIntoView();
                    selectedControl.Focus();
                }
            });

            ToggleControls(true, Properties.OutputStrings.FilteringAttributesCompletedFormat1, _entityName);
        }

        protected void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(_service.ConnectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(tSProgressBar
                , btnSaveEntity
                , btnAddNewAttribute
                , lstVwAttributes
                , mIEntityInformation
            );

            ToggleControl(_listAttributeControls);
        }

        private void txtBFilterAttribute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterEntityAttributes(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnSaveEntity_Click(object sender, RoutedEventArgs e)
        {
            if (!_entityIds.Any())
            {
                return;
            }

            var list = _listAttributeControls.OfType<IAttributeMetadataControl<AttributeMetadata>>().ToList();

            {
                var testEntity = new Entity(_entityName);

                foreach (var item in list)
                {
                    item.AddChangedAttribute(testEntity);
                }

                if (!testEntity.Attributes.Any())
                {
                    _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.NoChangesInEntityFormat1, _entityName);
                    _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                    return;
                }
            }

            ToggleControls(false, Properties.OutputStrings.SavingEntitiesFormat1, _entityName);

            bool hasError = false;

            var saver = new EntitySaverFactory().GetEntitySaver(_entityName, _service);

            foreach (var id in _entityIds)
            {
                var updateEntity = new Entity(_entityName)
                {
                    Id = id,
                };

                foreach (var item in list)
                {
                    item.AddChangedAttribute(updateEntity);
                }

                try
                {
                    _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.SavingEntityInstanceFormat2, _entityName, id);

                    _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, updateEntity);

                    await saver.UpsertAsync(updateEntity, (s) => UpdateStatus(s));
                }
                catch (Exception ex)
                {
                    hasError = true;

                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex, Properties.OutputStrings.SavingEntityInstanceFailedFormat2, _entityName, id);
                }
            }

            ToggleControls(true, Properties.OutputStrings.SavingEntitiesCompletedFormat1, _entityName);

            if (!hasError)
            {
                this.Close();
            }
        }

        protected override Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            FilterEntityAttributes(null);

            return base.OnRefreshList(e);
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityMetadataInWeb(_entityName);
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            this._iWriteToOutput.OpenFetchXmlFile(_service.ConnectionData, _commonConfig, _entityName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityInstanceListInWeb(_entityName);
        }

        private void btnAddNewAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (_entityMetadata == null
                || _entityMetadata.Attributes == null
            )
            {
                return;
            }

            var currentAttributes = new HashSet<string>(_listAttributeControls.OfType<IAttributeMetadataControl<AttributeMetadata>>().Select(c => c.AttributeMetadata.LogicalName), StringComparer.InvariantCultureIgnoreCase);

            var availableAttributes = _entityMetadata.Attributes.Where(a => string.IsNullOrEmpty(a.AttributeOf) && _attributeChecker(a) && !currentAttributes.Contains(a.LogicalName)).ToList();

            if (!availableAttributes.Any())
            {
                return;
            }

            var form = new WindowAttributeSelect(_iWriteToOutput, _service, _entityMetadata.MetadataId.Value, availableAttributes);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            var attributeMetadata = form.SelectedAttributeMetadata;

            if (attributeMetadata == null
                || !string.IsNullOrEmpty(attributeMetadata.AttributeOf)
                || !_attributeChecker(attributeMetadata)
            )
            {
                return;
            }

            UserControl control = null;

            this.Dispatcher.Invoke(() =>
            {
                control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, true, _entityMetadata, attributeMetadata, null, null);

                if (control is IAttributeMetadataControl<AttributeMetadata> attributeControl)
                {
                    attributeControl.RemoveControlClicked += AttributeControl_RemoveControlClicked;
                }
            });

            if (control != null)
            {
                _listAttributeControls.Add(control);
            }

            FilterEntityAttributes(control);
        }

        private void AttributeControl_RemoveControlClicked(object sender, EventArgs e)
        {
            if (sender is IAttributeMetadataControl<AttributeMetadata> attributeControl)
            {
                attributeControl.RemoveControlClicked -= AttributeControl_RemoveControlClicked;
                attributeControl.RemoveControlClicked -= AttributeControl_RemoveControlClicked;
            }

            if (sender is UserControl control
                && _listAttributeControls.Contains(control)
            )
            {
                _listAttributeControls.Remove(control);
            }

            FilterEntityAttributes(null);
        }
    }
}