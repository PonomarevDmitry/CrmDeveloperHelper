using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class CrmPathCompletionSource : ICompletionSource, IDisposable
    {
        private const string SourceNameMoniker = "CrmPath.{D6770026-9F25-4079-AC50-3C9D4B36EF71}";

        private readonly CrmPathCompletionSourceProvider _sourceProvider;

        private ITextBuffer _buffer;
        private readonly IClassifier _classifier;
        private ITextStructureNavigatorSelectorService _navigator;

        private readonly ImageSource _defaultGlyph;
        private readonly ImageSource _builtInGlyph;
        private readonly IGlyphService _glyphService;

        public CrmPathCompletionSource(CrmPathCompletionSourceProvider sourceProvider, ITextBuffer buffer, IClassifierAggregatorService classifier, ITextStructureNavigatorSelectorService navigator, IGlyphService glyphService)
        {
            _sourceProvider = sourceProvider;

            _buffer = buffer;
            _classifier = classifier.GetClassifier(buffer);
            _navigator = navigator;
            _glyphService = glyphService;
            _defaultGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
            _builtInGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupProperty, StandardGlyphItem.TotalGlyphItems);
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_isDisposed)
                return;

            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig?.CurrentConnectionData == null)
            {
                return;
            }

            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            var triggerPoint = session.GetTriggerPoint(snapshot);

            if (triggerPoint == null)
            {
                return;
            }

            var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

            SnapshotSpan extent = FindTokenSpanAtPosition(session).GetSpan(snapshot);

            ITrackingSpan applicableTo = null;

            if (extent.GetText() == ".")
            {
                extent = new SnapshotSpan(extent.Start + 1, extent.End);
            }

            var line = triggerPoint.Value.GetContainingLine().Extent;

            var relativePath = string.Empty;

            if (line.Start <= extent.Start)
            {
                relativePath = new SnapshotSpan(line.Start, extent.Start).GetText().TrimEnd('.');
            }

            applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

            var intellisenseData = repository.GetEntitiesIntellisenseData();

            if (intellisenseData == null
                || intellisenseData.Entities == null
                )
            {
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    FillEntityNamesInList(completionSets, applicableTo, intellisenseData, false);
                }
                else
                {
                    var usedEntities = GetUsedEntitiesFromPath(relativePath, intellisenseData);

                    if (usedEntities.Any())
                    {
                        repository.GetEntityDataForNamesAsync(usedEntities);
                    }

                    var entityNameHash = GetEntityNameFromPath(relativePath, intellisenseData, out bool isGuid);

                    if (entityNameHash != null)
                    {
                        if (entityNameHash.Count == 1)
                        {
                            var entityName = entityNameHash.First();

                            var entityData = repository.GetEntityAttributeIntellisense(entityName);

                            if (entityData != null)
                            {
                                FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);

                                FillMultiLinkForEntity(completionSets, applicableTo, intellisenseData, entityData);
                            }
                        }
                        else if (entityNameHash.Count > 0)
                        {
                            FillMultiLinkEntityForEntities(completionSets, applicableTo, entityNameHash, intellisenseData);
                        }
                        else if (isGuid)
                        {
                            FillMultiLinkEntityForAll(completionSets, applicableTo, intellisenseData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private HashSet<string> GetEntityNameFromPath(string relativePath, ConnectionIntellisenseData intellisenseData, out bool isGuid)
        {
            isGuid = false;

            if (intellisenseData == null)
            {
                return null;
            }

            relativePath = relativePath ?? string.Empty;

            relativePath = relativePath.Trim();

            IEnumerable<string> fields = relativePath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

            var sourceEntityName = fields.FirstOrDefault();
            fields = fields.Skip(1);

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            GetEntitiesByRelativePathRecursive(result, sourceEntityName, fields, intellisenseData, out isGuid);

            return result;
        }

        private void GetEntitiesByRelativePathRecursive(HashSet<string> result, string sourceEntityName, IEnumerable<string> fields, ConnectionIntellisenseData intellisenseData, out bool isGuid)
        {
            isGuid = false;

            if (!fields.Any())
            {
                if (!string.IsNullOrEmpty(sourceEntityName))
                {
                    result.Add(sourceEntityName);
                }

                return;
            }

            var attributeName = fields.First();
            fields = fields.Skip(1);

            if (attributeName.StartsWith("multi ", StringComparison.OrdinalIgnoreCase))
            {
                attributeName = attributeName.Substring(6).Trim();

                string[] split = attributeName.Split(new[] { ' ' });

                if (split.Length == 2)
                {
                    string referenceAttribute = split[0];
                    string logicalName = split[1];

                    var nextEntityName = GetNextEntityName(logicalName, referenceAttribute, intellisenseData);

                    if (!string.IsNullOrEmpty(nextEntityName))
                    {
                        if (!fields.Any())
                        {
                            result.Add(nextEntityName);
                            return;
                        }

                        GetEntitiesByRelativePathRecursive(result, nextEntityName, fields, intellisenseData, out isGuid);
                        return;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sourceEntityName) && intellisenseData.Entities.ContainsKey(sourceEntityName))
                {
                    var sourceEntity = intellisenseData.Entities[sourceEntityName];

                    if (sourceEntity.Attributes.ContainsKey(attributeName))
                    {
                        var attribute = sourceEntity.Attributes[attributeName];

                        if (attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup
                            || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Customer
                            || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Owner
                            )
                        {
                            if (attribute.Targets != null && attribute.Targets.Count > 0)
                            {
                                if (!fields.Any())
                                {
                                    foreach (var nextEntityName in attribute.Targets)
                                    {
                                        result.Add(nextEntityName);
                                    }

                                    return;
                                }

                                if (attribute.Targets.Count == 1)
                                {
                                    var nextEntityName = attribute.Targets.First();

                                    if (!fields.Any())
                                    {
                                        result.Add(nextEntityName);
                                        return;
                                    }

                                    GetEntitiesByRelativePathRecursive(result, nextEntityName, fields, intellisenseData, out isGuid);
                                    return;
                                }
                            }
                        }
                        else if (attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Uniqueidentifier)
                        {
                            if (sourceEntity.IsIntersectEntity)
                            {
                                var rel = sourceEntity?.ManyToManyRelationships?.Values?.FirstOrDefault(r => string.Equals(r.IntersectEntityName, sourceEntity.EntityLogicalName, StringComparison.OrdinalIgnoreCase));

                                if (rel != null)
                                {
                                    if (string.Equals(rel.Entity1IntersectAttributeName, attributeName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!fields.Any())
                                        {
                                            result.Add(rel.Entity1Name);
                                            return;
                                        }

                                        GetEntitiesByRelativePathRecursive(result, rel.Entity1Name, fields, intellisenseData, out isGuid);
                                        return;
                                    }
                                    else if (string.Equals(rel.Entity2IntersectAttributeName, attributeName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!fields.Any())
                                        {
                                            result.Add(rel.Entity2Name);
                                            return;
                                        }

                                        GetEntitiesByRelativePathRecursive(result, rel.Entity2Name, fields, intellisenseData, out isGuid);
                                        return;
                                    }
                                }
                            }

                            if (!fields.Any())
                            {
                                if (!string.Equals(sourceEntity.EntityPrimaryIdAttribute, attribute.LogicalName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isGuid = true;
                                    return;
                                }
                            }

                            GetEntitiesByRelativePathRecursive(result, null, fields, intellisenseData, out isGuid);
                        }
                    }
                }
            }
        }

        private string GetNextEntityName(string logicalName, string referenceAttribute, ConnectionIntellisenseData intellisenseData)
        {
            if (!intellisenseData.Entities.ContainsKey(logicalName))
            {
                return null;
            }

            var entityData = intellisenseData.Entities[logicalName];

            if (entityData.Attributes == null
                || !entityData.Attributes.ContainsKey(referenceAttribute)
                )
            {
                return null;
            }

            var attribute = entityData.Attributes[referenceAttribute];

            if (attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Customer
                || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Uniqueidentifier
                || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup
                || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Owner
                )
            {
                return logicalName;
            }

            return null;
        }

        private HashSet<string> GetUsedEntitiesFromPath(string relativePath, ConnectionIntellisenseData intellisenseData)
        {
            if (intellisenseData == null)
            {
                return null;
            }

            relativePath = relativePath ?? string.Empty;

            relativePath = relativePath.Trim();

            IEnumerable<string> fields = relativePath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

            var sourceEntityName = fields.FirstOrDefault();
            fields = fields.Skip(1);

            var result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                sourceEntityName
            };

            GetUsedEntitiesByRelativePathRecursive(result, sourceEntityName, fields, intellisenseData);

            return result;
        }

        private void GetUsedEntitiesByRelativePathRecursive(HashSet<string> result, string sourceEntityName, IEnumerable<string> fields, ConnectionIntellisenseData intellisenseData)
        {
            if (!fields.Any())
            {
                if (!string.IsNullOrEmpty(sourceEntityName))
                {
                    result.Add(sourceEntityName);
                }

                return;
            }

            var attributeName = fields.First();
            fields = fields.Skip(1);

            if (attributeName.StartsWith("multi ", StringComparison.OrdinalIgnoreCase))
            {
                attributeName = attributeName.Substring(6).Trim();

                string[] split = attributeName.Split(new[] { ' ' });

                if (split.Length == 2)
                {
                    string referenceAttribute = split[0];
                    string logicalName = split[1];

                    var nextEntityName = GetNextEntityName(logicalName, referenceAttribute, intellisenseData);

                    if (!string.IsNullOrEmpty(nextEntityName))
                    {
                        result.Add(nextEntityName);
                    }

                    GetUsedEntitiesByRelativePathRecursive(result, nextEntityName, fields, intellisenseData);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sourceEntityName) && intellisenseData.Entities.ContainsKey(sourceEntityName))
                {
                    var sourceEntity = intellisenseData.Entities[sourceEntityName];

                    if (sourceEntity.Attributes.ContainsKey(attributeName))
                    {
                        var attribute = sourceEntity.Attributes[attributeName];

                        if (attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup
                            || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Customer
                            || attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Owner
                            )
                        {
                            if (attribute.Targets != null && attribute.Targets.Count > 0)
                            {
                                foreach (var nextEntityName in attribute.Targets)
                                {
                                    result.Add(nextEntityName);

                                    GetUsedEntitiesByRelativePathRecursive(result, nextEntityName, fields, intellisenseData);
                                }
                            }
                        }
                        else if (attribute.AttributeType == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Uniqueidentifier)
                        {
                            if (sourceEntity.IsIntersectEntity)
                            {
                                var rel = sourceEntity?.ManyToManyRelationships?.Values?.FirstOrDefault(r => string.Equals(r.IntersectEntityName, sourceEntity.EntityLogicalName, StringComparison.OrdinalIgnoreCase));

                                if (rel != null)
                                {
                                    result.Add(rel.Entity1Name);
                                    result.Add(rel.Entity2Name);

                                    GetUsedEntitiesByRelativePathRecursive(result, rel.Entity1Name, fields, intellisenseData);

                                    GetUsedEntitiesByRelativePathRecursive(result, rel.Entity2Name, fields, intellisenseData);

                                    return;
                                }
                            }

                            GetUsedEntitiesByRelativePathRecursive(result, null, fields, intellisenseData);
                        }
                    }
                }
            }
        }

        private void FillEntityNamesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseData intellisenseData, bool isObjectTypeCode)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var keys = intellisenseData.Entities.Keys.ToList();

            foreach (var entityName in keys.OrderBy(s => s))
            {
                var entityData = intellisenseData.Entities[entityName];

                string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

                List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(entityData);

                var insertionText = entityData.EntityLogicalName;

                if (isObjectTypeCode)
                {
                    insertionText = entityData.ObjectTypeCode.ToString();
                }

                list.Add(CreateCompletion(entityDescription, insertionText, CrmIntellisenseCommon.CreateEntityDescription(entityData), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityIntellisenseDataAttributes(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, EntityIntellisenseData entityData)
        {
            if (entityData == null
                || entityData.Attributes == null
                )
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

            foreach (var attribute in entityData.AttributesOrdered())
            {
                string attributeDescription = CrmIntellisenseCommon.GetDisplayTextAttribute(entityData.EntityLogicalName, attribute);

                List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForAttribute(attribute);

                list.Add(CreateCompletion(attributeDescription, attribute.LogicalName, CrmIntellisenseCommon.CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0} Attributes", entityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillMultiLinkEntityForEntities(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, HashSet<string> entityNameHash, ConnectionIntellisenseData intellisenseData)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var entities = intellisenseData.Entities.Values.Where(e => entityNameHash.Contains(e.EntityLogicalName)).OrderBy(e => e.EntityLogicalName).ToList();

            FillAttributeReferencedEntities(completionSets, applicableTo, entities);
        }

        private void FillMultiLinkEntityForAll(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseData intellisenseData)
        {
            var entities = intellisenseData.Entities.Values.Where(e => !string.IsNullOrEmpty(e.EntityPrimaryIdAttribute)).OrderBy(e => e.EntityLogicalName).ToList();

            FillAttributeReferencedEntities(completionSets, applicableTo, entities);
        }

        private void FillAttributeReferencedEntities(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, IEnumerable<EntityIntellisenseData> entities)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var entityData in entities)
            {
                if (entityData.Attributes != null)
                {
                    string entityDescription = CrmIntellisenseCommon.GetDisplayTextEntity(entityData);

                    if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute)
                         && entityData.Attributes != null
                         && entityData.Attributes.ContainsKey(entityData.EntityPrimaryIdAttribute)
                         && entityData.Attributes[entityData.EntityPrimaryIdAttribute].AttributeType == AttributeTypeCode.Uniqueidentifier
                         )
                    {
                        List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(entityData);
                        compareValues.Add(entityData.EntityPrimaryIdAttribute);

                        var insertText = string.Format("multi {0} {1}", entityData.EntityPrimaryIdAttribute, entityData.EntityLogicalName);

                        list.Add(CreateCompletion(CrmIntellisenseCommon.GetDisplayTextEntity(entityData), insertText, CrmIntellisenseCommon.CreateEntityDescription(entityData), _defaultGlyph, compareValues));
                    }

                    foreach (var attribute in entityData.Attributes
                        .Values
                        .Where(e => !string.Equals(e.LogicalName, entityData.EntityPrimaryIdAttribute, StringComparison.InvariantCultureIgnoreCase) && e.AttributeType == AttributeTypeCode.Uniqueidentifier)
                        .OrderBy(e => e.LogicalName)
                        )
                    {
                        List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(entityData);

                        CrmIntellisenseCommon.FillCompareValuesForAttribute(compareValues, attribute);

                        var insertText = string.Format("multi {0} {1}", entityData.EntityPrimaryIdAttribute, entityData.EntityLogicalName);

                        list.Add(CreateCompletion(CrmIntellisenseCommon.GetDisplayTextEntityAndAttribute(entityData, attribute), insertText, CrmIntellisenseCommon.CreateEntityAndAttributeDescription(entityData, attribute), _defaultGlyph, compareValues));
                    }
                }
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillMultiLinkForEntity(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseData intellisenseData, EntityIntellisenseData entityData)
        {
            TupleList<string, string> childEntities = new TupleList<string, string>();

            if (entityData.OneToManyRelationships != null)
            {
                foreach (var item in entityData.OneToManyRelationships.Values)
                {
                    childEntities.Add(item.ChildEntityName, item.ChildEntityAttributeName);
                }
            }

            if (!entityData.IsIntersectEntity && entityData.ManyToManyRelationships != null)
            {
                foreach (var item in entityData.ManyToManyRelationships.Values)
                {
                    if (string.Equals(entityData.EntityLogicalName, item.Entity1Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        childEntities.Add(item.IntersectEntityName, item.Entity1IntersectAttributeName);
                    }
                    else if (string.Equals(entityData.EntityLogicalName, item.Entity2Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        childEntities.Add(item.IntersectEntityName, item.Entity2IntersectAttributeName);
                    }
                }
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var relation in childEntities.OrderBy(e => e.Item1).ThenBy(e => e.Item2))
            {
                if (intellisenseData.Entities.ContainsKey(relation.Item1))
                {
                    var childEntityData = intellisenseData.Entities[relation.Item1];

                    if (childEntityData.Attributes != null && childEntityData.Attributes.ContainsKey(relation.Item2))
                    {
                        var childAttribute = childEntityData.Attributes[relation.Item2];

                        List<string> compareValues = CrmIntellisenseCommon.GetCompareValuesForEntity(childEntityData);

                        compareValues.Add(relation.Item2);

                        var insertText = string.Format("multi {0} {1}", relation.Item2, relation.Item1);

                        list.Add(CreateCompletion(CrmIntellisenseCommon.GetDisplayTextEntityAndAttribute(childEntityData, childAttribute), insertText, CrmIntellisenseCommon.CreateEntityAndAttributeDescription(childEntityData, childAttribute), _defaultGlyph, compareValues));
                    }
                }
            }

            if (list.Count > 0)
            {
                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Child Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private ITrackingSpan FindTokenSpanAtPosition(ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = _navigator.GetTextStructureNavigator(_buffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }

        private CrmCompletion CreateCompletion(string displayText, string insertionText, string description, ImageSource glyph, IEnumerable<string> compareValues)
        {
            if (glyph == null)
                glyph = _defaultGlyph;

            return new CrmCompletion(displayText, insertionText, description, glyph, null, compareValues);
        }

        #region IDisposable Support

        private bool _isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {

                }

                _isDisposed = true;
            }
        }

        // ~FetchXmlCompletionSource() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}