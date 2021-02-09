using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowEntityEditor : WindowWithSingleConnection
    {
        private readonly CommonConfiguration _commonConfig;

        protected readonly string _entityName;

        protected readonly Guid? _entityId;

        protected readonly HashSet<string> _ignoredAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private EntityMetadata _entityMetadata;

        private EditingState _editingState = EditingState.Creating;

        private readonly AttributeMetadataControlFactory _controlFactory = new AttributeMetadataControlFactory();

        private readonly List<UserControl> _listAttributeControls = new List<UserControl>();

        private Func<AttributeMetadata, bool> _attributeChecker;

        public WindowEntityEditor(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string entityName
            , Guid? entityId
            , Entity entity
        ) : base(iWriteToOutput, service)
        {
            IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this.Name = string.Format("WindowEntityEditor_{0}", entityName);

            this._commonConfig = commonConfig;
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

            var task = RetrieveEntityInformation(entity);
        }

        public enum EditingState
        {
            Creating,
            Editing
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

        private async Task RetrieveEntityInformation(Entity entity)
        {
            try
            {
                ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, _entityName);

                var repositoryEntityMetadata = new EntityMetadataRepository(_service);

                this._entityMetadata = await repositoryEntityMetadata.GetEntityMetadataWithAttributesAsync(_entityName);

                ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, _entityName);

                if (this._entityMetadata != null)
                {
                    if (_entityId.HasValue && _entityId != Guid.Empty)
                    {
                        ToggleControls(false, Properties.OutputStrings.GettingEntityFormat1, _entityId);

                        var repositoryGeneric = new GenericRepository(_service, this._entityMetadata);

                        entity = await repositoryGeneric.GetEntityByIdAsync(_entityId.Value, ColumnSetInstances.AllColumns);

                        ToggleControls(true, Properties.OutputStrings.GettingEntityCompletedFormat1, _entityId);

                        if (entity != null)
                        {
                            this._editingState = EditingState.Editing;

                            SetWindowTitle(string.Format("Edit Entity {0} - {1}", _entityName, _entityId));

                            this._attributeChecker = a => a.IsValidForUpdate.GetValueOrDefault() && !_ignoredAttributes.Contains(a.LogicalName);
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

                    if (entity != null)
                    {
                        var allwaysAddToEntity = this._editingState == EditingState.Creating;

                        base.SwitchEntityDatesToLocalTime(new[] { entity });

                        foreach (var attributeValue in entity.Attributes.OrderBy(a => a.Key))
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
                                    control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, this._entityMetadata, attributeMetadata, entity, attributeValue.Value, allwaysAddToEntity, true);

                                    if (control is IAttributeMetadataControl<AttributeMetadata> attributeControl)
                                    {
                                        attributeControl.RemoveControlClicked += AttributeControl_RemoveControlClicked;
                                    }
                                });

                                if (control != null)
                                {
                                    _listAttributeControls.Add(control);
                                }
                            }
                        }
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

        protected override void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(tSProgressBar
                , btnSaveEntity
                , btnAddNewAttribute
                , btnAddMultipleNewAttribute
                , lstVwAttributes
                , mIEntityInformation
            );

            ToggleControl(_listAttributeControls);

            this.Dispatcher.Invoke(() =>
            {
                ActivateControls(mIEntityInformation.Items.OfType<Control>(), IsControlsEnabled && _editingState == EditingState.Editing, "mIEntityInstance");
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

            ToggleControls(false, Properties.OutputStrings.SavingEntityFormat1, _entityName);

            if (_entityId.HasValue && _entityId != Guid.Empty)
            {
                updateEntity.Id = _entityId.Value;

                if (_editingState == EditingState.Editing)
                {
                    _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, _entityName, _entityId.Value);
                }
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

        protected override Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            FilterEntityAttributes(null);

            return base.OnRefreshList(e);
        }

        private void mIOpenEntityInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!_entityId.HasValue || _entityId == Guid.Empty)
            {
                return;
            }

            _service.ConnectionData.OpenEntityInstanceInWeb(_entityName, _entityId.Value);
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
            if (!_entityId.HasValue || _entityId == Guid.Empty)
            {
                return;
            }

            var url = _service.ConnectionData.GetEntityInstanceUrl(_entityName, _entityId.Value);

            ClipboardHelper.SetText(url);
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
                control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, this._entityMetadata, attributeMetadata, null, null, true, false);

                if (control != null && control is IAttributeMetadataControl<AttributeMetadata> attributeControl)
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

        private void btnAddMultipleNewAttribute_Click(object sender, RoutedEventArgs e)
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

            var form = new WindowAttributeMultiSelect(_iWriteToOutput, _service, _entityMetadata.MetadataId.Value, availableAttributes, string.Empty);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            var selectedAttributes = form.GetAttributeMetadatas();

            if (!selectedAttributes.Any(a => string.IsNullOrEmpty(a.AttributeOf) && _attributeChecker(a)))
            {
                return;
            }

            foreach (var attributeMetadata in selectedAttributes.Where(a => string.IsNullOrEmpty(a.AttributeOf) && _attributeChecker(a)))
            {
                UserControl control = null;

                this.Dispatcher.Invoke(() =>
                {
                    control = _controlFactory.CreateControlForAttribute(this._iWriteToOutput, _service, this._entityMetadata, attributeMetadata, null, null, true, false);

                    if (control != null && control is IAttributeMetadataControl<AttributeMetadata> attributeControl)
                    {
                        attributeControl.RemoveControlClicked += AttributeControl_RemoveControlClicked;
                    }
                });

                if (control != null)
                {
                    _listAttributeControls.Add(control);
                }
            }

            FilterEntityAttributes(null);
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
