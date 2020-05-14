using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowEntityEditor : WindowWithOutputAndCommonConfig
    {
        protected readonly string _entityName;

        protected readonly Guid _entityId;

        protected readonly IOrganizationServiceExtented _service;

        protected readonly HashSet<string> _ignoredAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private EntityMetadata _entityMetadata;

        private Entity _entityInstance;

        private readonly AttributeMetadataControlFactory _controlFactory = new AttributeMetadataControlFactory();

        private readonly List<UserControl> _listAttributeControls = new List<UserControl>();

        private Func<AttributeMetadata, bool> _attributeChecker;

        public WindowEntityEditor(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityName
            , Guid entityId
        ) : base(iWriteToOutput, commonConfig)
        {
            IncreaseInit();

            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this.Name = string.Format("WindowEntityEditor_{0}", entityName);

            this._service = service;
            this._entityName = entityName;
            this._entityId = entityId;

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            FillIgnoredAttributes(_entityName, _ignoredAttributes);

            ActivateControls(mIEntityInformation.Items.OfType<Control>(), false, "mIEntityInstance");

            FillExplorersMenuItems();

            DecreaseInit();

            txtBFilterAttribute.SelectionStart = txtBFilterAttribute.Text.Length;
            txtBFilterAttribute.SelectionLength = 0;
            txtBFilterAttribute.Focus();

            this._attributeChecker = a => a.IsValidForCreate.GetValueOrDefault() && !_ignoredAttributes.Contains(a.LogicalName);

            var task = RetrieveEntityInformation();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service)
             , getEntityName: () => _entityName
            );

            explorersHelper.FillExplorers(miExplorers);
        }

        public static void FillIgnoredAttributes(string entityName, HashSet<string> ignoredAttributes)
        {
            switch (entityName.ToLower())
            {
                case PluginAssembly.EntityLogicalName:
                    ignoredAttributes.Add(PluginAssembly.Schema.Attributes.content);
                    break;

                case WebResource.EntityLogicalName:
                    ignoredAttributes.Add(WebResource.Schema.Attributes.content);
                    break;
            }
        }

        private async Task RetrieveEntityInformation()
        {
            try
            {
                ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, _entityName);

                var repositoryEntityMetadata = new EntityMetadataRepository(_service);

                this._entityMetadata = await repositoryEntityMetadata.GetEntityMetadataWithAttributesAsync(_entityName);

                ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, _entityName);

                if (this._entityMetadata != null)
                {
                    if (_entityId != Guid.Empty)
                    {
                        ToggleControls(false, Properties.OutputStrings.GettingEntityFormat1, _entityId);

                        var repositoryGeneric = new GenericRepository(_service, this._entityMetadata);

                        this._entityInstance = await repositoryGeneric.GetEntityByIdAsync(_entityId, new ColumnSet(true));

                        ToggleControls(true, Properties.OutputStrings.GettingEntityCompletedFormat1, _entityId);

                        if (this._entityInstance != null)
                        {
                            base.SwitchEntityDatesToLocalTime(new[] { this._entityInstance });

                            SetWindowTitle(string.Format("Edit Entity {0} - {1}", _entityName, _entityId));

                            this.Dispatcher.Invoke(() =>
                            {
                                btnSaveAsCopyEntity.IsEnabled = true;
                                btnSaveAsCopyEntity.Visibility = Visibility.Visible;
                            });

                            this._attributeChecker = a => a.IsValidForUpdate.GetValueOrDefault() && !_ignoredAttributes.Contains(a.LogicalName);

                            foreach (var attributeValue in this._entityInstance.Attributes.OrderBy(a => a.Key))
                            {
                                var attributeMetadata = this._entityMetadata.Attributes.FirstOrDefault(a => string.Equals(a.LogicalName, attributeValue.Key, StringComparison.InvariantCultureIgnoreCase));

                                if (attributeMetadata != null
                                    && string.IsNullOrEmpty(attributeMetadata.AttributeOf)
                                    && _attributeChecker(attributeMetadata)
                                )
                                {
                                    UserControl control = null;

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, false, this._entityMetadata, attributeMetadata, _entityInstance, attributeValue.Value);
                                    });

                                    if (control != null)
                                    {
                                        _listAttributeControls.Add(control);
                                    }
                                }
                            }
                        }
                        else
                        {
                            SetWindowTitle(string.Format("Create Entity {0} - {1}", _entityName, _entityId));
                        }
                    }
                    else
                    {
                        SetWindowTitle(string.Format("Create Entity {0}", _entityName));
                    }
                }

                FilterEntityAttributes(null);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private void SetWindowTitle(string title)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Title = title;
            });
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

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
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

            this.Dispatcher.Invoke(() =>
            {
                btnSaveAsCopyEntity.IsEnabled = IsControlsEnabled && _entityInstance != null;

                ActivateControls(mIEntityInformation.Items.OfType<Control>(), IsControlsEnabled && _entityInstance != null, "mIEntityInstance");
            });
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
            var updateEntity = new Entity(_entityName);

            var list = _listAttributeControls.OfType<IAttributeMetadataControl<AttributeMetadata>>().ToList();

            foreach (var item in list)
            {
                item.AddChangedAttribute(updateEntity);
            }

            if (!updateEntity.Attributes.Any())
            {
                _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.NoChangesInEntityFormat1, _entityName);
                _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                return;
            }

            if (_entityId != Guid.Empty)
            {
                updateEntity.Id = _entityId;
            }

            ToggleControls(false, Properties.OutputStrings.SavingEntityFormat1, _entityName);

            if (_entityInstance != null)
            {
                _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, _entityInstance);
            }

            try
            {
                var saver = new EntitySaverFactory().GetEntitySaver(_entityName, _service);

                var tempEntityId = await saver.UpsertAsync(updateEntity, (s) => UpdateStatus(s));

                _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, _entityName, tempEntityId);

                ToggleControls(true, Properties.OutputStrings.SavingEntityCompletedFormat1, _entityName);

                this.Close();
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.OutputStrings.SavingEntityFailedFormat1, _entityName);

                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async void btnSaveAsCopyEntity_Click(object sender, RoutedEventArgs e)
        {
            var updateEntity = new Entity(_entityName);

            var list = _listAttributeControls.OfType<IAttributeMetadataControl<AttributeMetadata>>().ToList();

            foreach (var item in list)
            {
                item.AddAttribute(updateEntity);
            }

            if (!updateEntity.Attributes.Any())
            {
                _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.NoChangesInEntityFormat1, _entityName);
                _iWriteToOutput.ActivateOutputWindow(_service.ConnectionData);
                return;
            }

            ToggleControls(false, Properties.OutputStrings.SavingEntityFormat1, _entityName);

            if (_entityInstance != null)
            {
                _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, _entityInstance);
            }

            try
            {
                var saver = new EntitySaverFactory().GetEntitySaver(_entityName, _service);

                var tempEntityId = await saver.UpsertAsync(updateEntity, (s) => UpdateStatus(s));

                _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, _entityName, tempEntityId);

                ToggleControls(true, Properties.OutputStrings.SavingEntityCompletedFormat1, _entityName);
            }
            catch (Exception ex)
            {
                ToggleControls(true, Properties.OutputStrings.SavingEntityFailedFormat1, _entityName);

                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        protected override Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            FilterEntityAttributes(null);

            return base.OnRefreshList(e);
        }

        private void mIOpenEntityInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (_entityId == Guid.Empty)
            {
                return;
            }

            _service.ConnectionData.OpenEntityInstanceInWeb(_entityName, _entityId);
        }

        private void mICopyEntityInstanceIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (_entityId == Guid.Empty)
            {
                return;
            }

            ClipboardHelper.SetText(_entityId.ToString());
        }

        private void mICopyEntityInstanceUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (_entityId == Guid.Empty)
            {
                return;
            }

            var url = _service.ConnectionData.GetEntityInstanceUrl(_entityName, _entityId);

            ClipboardHelper.SetText(url);
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityMetadataInWeb(_entityName);
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
                control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, false, this._entityMetadata, attributeMetadata, _entityInstance, null);
            });

            if (control != null)
            {
                _listAttributeControls.Add(control);
            }

            FilterEntityAttributes(control);
        }
    }
}