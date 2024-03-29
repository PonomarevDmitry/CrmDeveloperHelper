using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowEntityBulkTransfer : WindowWithSingleConnection
    {
        protected readonly ReadOnlyCollection<Entity> _entityCollection;

        private EntityMetadata _entityMetadata;

        private readonly CommonConfiguration _commonConfig;

        private Dictionary<string, EntityMetadata> _cacheEntityMetadata = new Dictionary<string, EntityMetadata>(StringComparer.InvariantCultureIgnoreCase);

        private HashSet<string> _cacheEntityNotExists = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private HashSet<string> _cacheMessageEntityAttributeNotExists = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private readonly List<EntityReferenceMappingControl> _listLookupMappingControls = new List<EntityReferenceMappingControl>();

        private readonly Dictionary<EntityReference, EntityReferenceMappingControl> _dictLookupMapping = new Dictionary<EntityReference, EntityReferenceMappingControl>(new EntityReferenceEqualityComparer());

        private readonly Func<AttributeMetadata, bool> _attributeChecker;

        public WindowEntityBulkTransfer(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , EntityMetadata entityMetadata
            , IEnumerable<Entity> entityCollection
        ) : base(iWriteToOutput, service)
        {
            IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this.Name = string.Format("WindowEntityBulkTransfer_{0}", entityMetadata.LogicalName);

            _cacheEntityMetadata.Add(entityMetadata.LogicalName, entityMetadata);

            this._entityMetadata = entityMetadata;
            this._commonConfig = commonConfig;

            this._entityCollection = new ReadOnlyCollection<Entity>(entityCollection.Where(i => i.Id != Guid.Empty).Distinct().ToList());

            this.tSSLblConnectionName.Content = this._service.ConnectionData.Name;

            FillExplorersMenuItems();

            DecreaseInit();

            this._attributeChecker = a => a.IsValidForUpdate.GetValueOrDefault();

            var task = RetrieveEntityInformation();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, () => Task.FromResult(_service)
                , getEntityName: () => _entityMetadata.LogicalName
            );

            explorersHelper.FillExplorers(miExplorers);
        }

        private async Task RetrieveEntityInformation()
        {
            try
            {
                foreach (var entity in _entityCollection)
                {
                    ToggleControls(false, Properties.OutputStrings.FilteringEntityInstanceAttributesFormat2, _entityMetadata.LogicalName, entity.Id);

                    foreach (var attributeKey in entity.Attributes.Keys.ToList())
                    {
                        var attributeMetadata = _entityMetadata.Attributes.FirstOrDefault(a => string.Equals(attributeKey, a.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                        if (attributeMetadata == null)
                        {
                            if (_cacheMessageEntityAttributeNotExists.Add(attributeKey))
                            {
                                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.EntityAttributeNotExistsInConnectionFormat3, _entityMetadata.LogicalName, attributeKey, _service.ConnectionData.Name);
                            }

                            entity.Attributes.Remove(attributeKey);
                            continue;
                        }

                        if (!attributeMetadata.IsValidForCreate.GetValueOrDefault()
                            && !attributeMetadata.IsValidForUpdate.GetValueOrDefault()
                        )
                        {
                            entity.Attributes.Remove(attributeKey);
                            continue;
                        }

                        if (entity.Attributes[attributeKey] != null
                            && entity.Attributes[attributeKey] is EntityReference entityReference
                            && attributeMetadata is LookupAttributeMetadata lookupAttributeMetadata
                            && !_dictLookupMapping.ContainsKey(entityReference)
                        )
                        {
                            var targetEntityMetadata = await GetEntityMetadata(entityReference.LogicalName);

                            if (targetEntityMetadata == null)
                            {
                                entity.Attributes.Remove(attributeKey);
                                continue;
                            }

                            EntityReference targetEntityReference = null;

                            var repositoryGeneric = new GenericRepository(_service, targetEntityMetadata);

                            ColumnSet columnSet = ColumnSetInstances.None;

                            if (!string.IsNullOrEmpty(targetEntityMetadata.PrimaryNameAttribute))
                            {
                                columnSet.AddColumn(targetEntityMetadata.PrimaryNameAttribute);
                            }

                            if (targetEntityReference == null)
                            {
                                ToggleControls(false, Properties.OutputStrings.TryingToFindEntityByIdFormat2, entityReference.LogicalName, entityReference.Id);

                                var targetEntity = await repositoryGeneric.GetEntityByIdAsync(entityReference.Id, columnSet);

                                if (targetEntity != null)
                                {
                                    targetEntityReference = targetEntity.ToEntityReference();

                                    if (!string.IsNullOrEmpty(targetEntityMetadata.PrimaryNameAttribute)
                                        && targetEntity.Attributes.ContainsKey(targetEntityMetadata.PrimaryNameAttribute)
                                        && targetEntity.Attributes[targetEntityMetadata.PrimaryNameAttribute] != null
                                        && targetEntity.Attributes[targetEntityMetadata.PrimaryNameAttribute] is string entityReferenceName
                                    )
                                    {
                                        targetEntityReference.Name = entityReferenceName;
                                    }

                                    ToggleControls(true, Properties.OutputStrings.TryingToFindEntityByIdFoundedFormat2, entityReference.LogicalName, entityReference.Id);
                                }
                                else
                                {
                                    ToggleControls(true, Properties.OutputStrings.TryingToFindEntityByIdNotFoundedFormat2, entityReference.LogicalName, entityReference.Id);
                                }
                            }

                            if (targetEntityReference == null
                                && !string.IsNullOrEmpty(targetEntityMetadata.PrimaryNameAttribute)
                                && !string.IsNullOrEmpty(entityReference.Name)
                            )
                            {
                                ToggleControls(false, Properties.OutputStrings.TryingToFindEntityByNameFormat4, entityReference.LogicalName, entityReference.Id, targetEntityMetadata.PrimaryNameAttribute, entityReference.Name);

                                var targetEntity = await repositoryGeneric.GetEntityByNameFieldAsync(entityReference.Name, columnSet);

                                if (targetEntity != null)
                                {
                                    targetEntityReference = targetEntity.ToEntityReference();

                                    if (targetEntity.Attributes.ContainsKey(targetEntityMetadata.PrimaryNameAttribute)
                                        && targetEntity.Attributes[targetEntityMetadata.PrimaryNameAttribute] != null
                                        && targetEntity.Attributes[targetEntityMetadata.PrimaryNameAttribute] is string entityReferenceName
                                    )
                                    {
                                        targetEntityReference.Name = entityReferenceName;
                                    }

                                    ToggleControls(true, Properties.OutputStrings.TryingToFindEntityByNameFoundedFormat4, entityReference.LogicalName, entityReference.Id, targetEntityMetadata.PrimaryNameAttribute, entityReference.Name);
                                }
                                else
                                {
                                    ToggleControls(true, Properties.OutputStrings.TryingToFindEntityByNameNotFoundedFormat4, entityReference.LogicalName, entityReference.Id, targetEntityMetadata.PrimaryNameAttribute, entityReference.Name);
                                }
                            }

                            EntityReferenceMappingControl control = null;

                            this.Dispatcher.Invoke(() =>
                            {
                                control = new EntityReferenceMappingControl(this._iWriteToOutput, _service, lookupAttributeMetadata, entityReference, targetEntityReference);
                            });

                            _dictLookupMapping.Add(entityReference, control);
                            _listLookupMappingControls.Add(control);
                        }
                    }

                    ToggleControls(true, Properties.OutputStrings.FilteringEntityInstanceAttributesCompletedFormat2, _entityMetadata.LogicalName, entity.Id);
                }

                this.lstVwLookupMapping.Dispatcher.Invoke(() =>
                {
                    int index = 0;

                    foreach (var item in _listLookupMappingControls.OrderBy(a => a.AttributeMetadata.LogicalName))
                    {
                        if (item is UserControl control)
                        {
                            var itemRowDef = new RowDefinition()
                            {
                                Height = new GridLength(10, GridUnitType.Auto),
                            };

                            lstVwLookupMapping.RowDefinitions.Add(itemRowDef);

                            control.VerticalAlignment = VerticalAlignment.Stretch;
                            control.HorizontalAlignment = HorizontalAlignment.Stretch;

                            Grid.SetRow(control, index);
                            lstVwLookupMapping.Children.Add(control);

                            index++;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex);
            }
        }

        private async Task<EntityMetadata> GetEntityMetadata(string entityName)
        {
            if (_cacheEntityMetadata.ContainsKey(entityName))
            {
                return _cacheEntityMetadata[entityName];
            }

            if (_cacheEntityNotExists.Contains(entityName))
            {
                return null;
            }

            var repositoryEntityMetadata = new EntityMetadataRepository(_service);

            ToggleControls(false, Properties.OutputStrings.GettingEntityMetadataFormat1, entityName);

            var entityMetadata = await repositoryEntityMetadata.GetEntityMetadataAsync(entityName);

            ToggleControls(true, Properties.OutputStrings.GettingEntityMetadataCompletedFormat1, entityName);

            if (entityMetadata != null)
            {
                _cacheEntityMetadata[entityName] = entityMetadata;
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.EntityNotExistsInConnectionFormat2, entityName, _service.ConnectionData.Name);
                _cacheEntityNotExists.Add(entityName);
            }

            return entityMetadata;
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
                , btnTransferEntities
                , lstVwLookupMapping
                , mIEntityInformation
            );

            ToggleControl(_listLookupMappingControls);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnTransferEntities_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            if (_entityMetadata == null
                || !_entityCollection.Any()
            )
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.SavingEntitiesFormat2, _entityMetadata.LogicalName, _entityCollection.Count);

            bool hasError = false;

            int number = 1;

            foreach (var entity in _entityCollection)
            {
                var updateEntity = new Entity(_entityMetadata.LogicalName)
                {
                    Id = entity.Id,
                };

                foreach (var attribute in entity.Attributes)
                {
                    if (attribute.Value != null)
                    {
                        if (attribute.Value is EntityReference entityReference
                            && _dictLookupMapping.ContainsKey(entityReference)
                        )
                        {
                            updateEntity.Attributes[attribute.Key] = _dictLookupMapping[entityReference].CurrentValue;
                        }
                        else
                        {
                            updateEntity.Attributes[attribute.Key] = attribute.Value;
                        }
                    }
                }

                try
                {
                    _iWriteToOutput.WriteToOutput(_service.ConnectionData, Properties.OutputStrings.SavingEntityInstanceFormat4, number, _entityCollection.Count, _entityMetadata.LogicalName, entity.Id);

                    _iWriteToOutput.WriteToOutputEntityInstance(_service.ConnectionData, updateEntity);

                    await _service.UpsertAsync(updateEntity);
                }
                catch (Exception ex)
                {
                    hasError = true;

                    _iWriteToOutput.WriteErrorToOutput(_service.ConnectionData, ex, Properties.OutputStrings.SavingEntityInstanceFailedFormat2, _entityMetadata.LogicalName, entity.Id);
                }

                number++;
            }

            ToggleControls(true, Properties.OutputStrings.SavingEntitiesCompletedFormat2, _entityMetadata.LogicalName, _entityCollection.Count);

            if (!hasError)
            {
                this.Close();
            }
        }

        private void mIOpenEntityInstanceCustomizationInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityMetadataInWeb(_entityMetadata.LogicalName);
        }

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            this._iWriteToOutput.OpenFetchXmlFile(_service.ConnectionData, _commonConfig, _entityMetadata.LogicalName);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            _service.ConnectionData.OpenEntityInstanceListInWeb(_entityMetadata.LogicalName);
        }

        private class EntityReferenceEqualityComparer : IEqualityComparer<EntityReference>
        {
            public bool Equals(EntityReference x, EntityReference y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                else if (x == null || y == null)
                {
                    return false;
                }
                else if (
                    x.Id == y.Id
                    && string.Equals(x.LogicalName ?? string.Empty, y.LogicalName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(EntityReference obj)
            {
                return
                    StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.LogicalName ?? string.Empty)
                    ^ obj.Id.GetHashCode()
                    ;
            }
        }
    }
}