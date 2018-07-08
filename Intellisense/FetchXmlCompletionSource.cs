using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public sealed class FetchXmlCompletionSource : ICompletionSource, IDisposable
    {
        private const string SourceNameMoniker = "CrmFetchXml.{8A22EFE3-4F6D-47A8-A511-82DE08260BA7}";

        private const string SourceNameMonikerAll = "CrmFetchXml.{E438F5CE-FBBD-4754-A2F7-F1AEB6752499}";

        private FetchXmlCompletionSourceProvider _sourceProvider;

        private bool _isDisposed;

        private ITextBuffer _buffer;
        private IClassifier _classifier;
        private ITextStructureNavigatorSelectorService _navigator;

        private ImageSource _defaultGlyph;
        private ImageSource _builtInGlyph;
        private IGlyphService _glyphService;

        public FetchXmlCompletionSource(FetchXmlCompletionSourceProvider sourceProvider, ITextBuffer buffer, IClassifierAggregatorService classifier, ITextStructureNavigatorSelectorService navigator, IGlyphService glyphService)
        {
            _sourceProvider = sourceProvider;

            _buffer = buffer;
            _classifier = classifier.GetClassifier(buffer);
            _navigator = navigator;
            _glyphService = glyphService;
            _defaultGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
            _builtInGlyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupProperty, StandardGlyphItem.TotalGlyphItems);

            //_imageService = ExtensibilityToolsPackage.GetGlobalService(typeof(SVsImageService)) as IVsImageService2;
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

            XElement doc = ReadXmlDocument(snapshot.GetText());

            if (doc == null)
            {
                return;
            }

            if (string.Equals(doc.Name.LocalName, "fetch", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "grid", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "savedquery", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForFetchXml(session, completionSets, snapshot, doc, repository);

                FillSessionForGridXml(session, completionSets, snapshot, doc, repository);
            }
            else if (string.Equals(doc.Name.LocalName, "SiteMap", StringComparison.InvariantCultureIgnoreCase))
            {
                var repository = ConnectionIntellisenseDataRepository.GetRepository(connectionConfig.CurrentConnectionData);

                FillSessionForSiteMap(session, completionSets, snapshot, doc, repository);
            }
        }

        private void FillSessionForSiteMap(ICompletionSession session, IList<CompletionSet> completionSets, ITextSnapshot snapshot, XElement doc, ConnectionIntellisenseDataRepository repository)
        {
            SnapshotSpan extent = FindTokenSpanAtPosition(session).GetSpan(snapshot);

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return;
            }

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(extent.Snapshot, 0, extent.Snapshot.Length));

            bool isQuotes = false;

            var containingAttributeValue = spans
                .Where(s => s.Span.Contains(extent.Start)
                && s.Span.Contains(extent)
                && s.ClassificationType.IsOfType("XML Attribute Value"))
                .OrderByDescending(s => s.Span.Start.Position)
                .FirstOrDefault();

            if (containingAttributeValue == null)
            {
                containingAttributeValue = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Quotes")
                    && s.Span.GetText() == "\"\""
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

                if (containingAttributeValue != null)
                {
                    isQuotes = true;
                }
            }

            if (containingAttributeValue == null)
            {
                return;
            }

            ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

            if (currentAttr == null)
            {
                return;
            }

            string currentNodeName = currentXmlNode.Name.LocalName;

            string currentAttributeName = currentAttr.Span.GetText();

            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

            if (isQuotes)
            {
                applicableTo = snapshot.CreateTrackingSpan(extent.Start.Position + 1, 0, SpanTrackingMode.EdgeInclusive);
            }

            try
            {
                if (string.Equals(currentNodeName, "SubArea", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repository, false);
                    }
                    //else if (string.Equals(currentAttributeName, "Sku", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    //FillEntityNamesInList(completionSets, repository);
                    //}
                    //else if (string.Equals(currentAttributeName, "Client", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    //FillEntityNamesInList(completionSets, repository);
                    //}
                }
                else if (string.Equals(currentNodeName, "Privilege", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "Entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repository, false);
                    }
                    //else if (string.Equals(currentAttributeName, "Privilege", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    //FillEntityNamesInList(completionSets, repository);
                    //}
                }
                else if (string.Equals(currentNodeName, "Title", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(currentNodeName, "Description", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    if (string.Equals(currentAttributeName, "LCID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillLCID(completionSets, applicableTo);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private void FillLCID(IList<CompletionSet> completionSets, ITrackingSpan applicableTo)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var keys = LanguageLocale.KnownLocales.Keys;

            foreach (var lcid in keys)
            {
                string entityDescription = string.Format("{0} - {1}", LanguageLocale.KnownLocales[lcid], lcid);

                List<string> compareValues = new List<string>();

                compareValues.Add(LanguageLocale.KnownLocales[lcid]);
                compareValues.Add(lcid.ToString());

                var insertionText = lcid.ToString();

                list.Add(CreateCompletion(entityDescription, insertionText, null, _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(a => a.DisplayText).ToList();

                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All LCID", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillSessionForGridXml(ICompletionSession session, IList<CompletionSet> completionSets, ITextSnapshot snapshot, XElement doc, ConnectionIntellisenseDataRepository repository)
        {
            HashSet<int> usedEntityCodes = GetUsedEntityObjectTypeCodes(doc);

            if (usedEntityCodes.Any())
            {
                repository.GetEntityDataForObjectTypeCodesAsync(usedEntityCodes);
            }

            SnapshotSpan extent = FindTokenSpanAtPosition(session).GetSpan(snapshot);

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return;
            }

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(extent.Snapshot, 0, extent.Snapshot.Length));

            bool isQuotes = false;

            var containingAttributeValue = spans
                .Where(s => s.Span.Contains(extent.Start)
                && s.Span.Contains(extent)
                && s.ClassificationType.IsOfType("XML Attribute Value"))
                .OrderByDescending(s => s.Span.Start.Position)
                .FirstOrDefault();

            if (containingAttributeValue == null)
            {
                containingAttributeValue = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Quotes")
                    && s.Span.GetText() == "\"\""
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

                if (containingAttributeValue != null)
                {
                    isQuotes = true;
                }
            }

            if (containingAttributeValue == null)
            {
                return;
            }

            ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

            if (currentAttr == null)
            {
                return;
            }

            string currentNodeName = currentXmlNode.Name.LocalName;

            string currentAttributeName = currentAttr.Span.GetText();

            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

            if (isQuotes)
            {
                applicableTo = snapshot.CreateTrackingSpan(extent.Start.Position + 1, 0, SpanTrackingMode.EdgeInclusive);
            }

            try
            {
                if (string.Equals(currentNodeName, "grid", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "object", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityNamesInList(completionSets, applicableTo, repository, true);
                    }
                    else if (string.Equals(currentAttributeName, "jump", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityPrimaryAttributeForGrid(completionSets, applicableTo, repository, currentXmlNode, true);

                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
                else if (string.Equals(currentNodeName, "row", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityPrimaryAttributeForGrid(completionSets, applicableTo, repository, currentXmlNode, false);

                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
                else if (string.Equals(currentNodeName, "cell", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillEntityAttributesInListForGrid(completionSets, applicableTo, repository, currentXmlNode);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }

        private void FillSessionForFetchXml(ICompletionSession session, IList<CompletionSet> completionSets, ITextSnapshot snapshot, XElement doc, ConnectionIntellisenseDataRepository repository)
        {
            HashSet<string> usedEntities = GetUsedEntities(doc);

            if (usedEntities.Any())
            {
                repository.GetEntityDataForNamesAsync(usedEntities);
            }

            Dictionary<string, string> aliases = GetEntityAliases(doc);

            SnapshotSpan extent = FindTokenSpanAtPosition(session).GetSpan(snapshot);

            var currentXmlNode = GetCurrentXmlNode(doc, extent);

            if (currentXmlNode == null)
            {
                return;
            }

            var spans = _classifier.GetClassificationSpans(new SnapshotSpan(extent.Snapshot, 0, extent.Snapshot.Length));

            bool isQuotes = false;

            var containingAttributeValue = spans
                .Where(s => s.Span.Contains(extent.Start)
                && s.Span.Contains(extent)
                && s.ClassificationType.IsOfType("XML Attribute Value"))
                .OrderByDescending(s => s.Span.Start.Position)
                .FirstOrDefault();

            if (containingAttributeValue == null)
            {
                containingAttributeValue = spans
                    .Where(s => s.Span.Contains(extent.Start)
                    && s.Span.Contains(extent)
                    && s.ClassificationType.IsOfType("XML Attribute Quotes")
                    && s.Span.GetText() == "\"\""
                    )
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

                if (containingAttributeValue != null)
                {
                    isQuotes = true;
                }
            }

            if (containingAttributeValue != null)
            {
                ClassificationSpan currentAttr = GetCurrentXmlAttributeName(snapshot, containingAttributeValue, spans);

                if (currentAttr == null)
                {
                    return;
                }

                string currentNodeName = currentXmlNode.Name.LocalName;

                string currentAttributeName = currentAttr.Span.GetText();

                ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(extent, SpanTrackingMode.EdgeInclusive);

                if (isQuotes)
                {
                    applicableTo = snapshot.CreateTrackingSpan(extent.Start.Position + 1, 0, SpanTrackingMode.EdgeInclusive);
                }

                try
                {
                    if (string.Equals(currentNodeName, "entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillEntityNamesInList(completionSets, applicableTo, repository, false);
                        }
                    }
                    else if (string.Equals(currentNodeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode);
                        }
                    }
                    else if (string.Equals(currentNodeName, "order", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.Equals(currentAttributeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode);
                        }
                    }
                    else if (string.Equals(currentNodeName, "condition", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.Equals(currentAttributeName, "attribute", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillEntityAttributesInList(completionSets, applicableTo, repository, currentXmlNode);
                        }
                        else if (string.Equals(currentAttributeName, "value", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillEntityAttributeValuesInList(completionSets, applicableTo, repository, currentXmlNode, aliases);
                        }
                        else if (string.Equals(currentAttributeName, "entityname", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillAliases(completionSets, applicableTo, aliases);
                        }
                    }
                    else if (string.Equals(currentNodeName, "link-entity", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.Equals(currentAttributeName, "name", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillLinkedEntityNames(completionSets, applicableTo, repository, currentXmlNode);

                            FillEntityNamesInList(completionSets, applicableTo, repository, false);
                        }
                        else if (string.Equals(currentAttributeName, "from", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillLinkedEntityFromField(completionSets, applicableTo, repository, currentXmlNode);
                        }
                        else if (string.Equals(currentAttributeName, "to", StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillLinkedEntityToField(completionSets, applicableTo, repository, currentXmlNode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(ex);
                }
            }
            else
            {
                if (!string.Equals(currentXmlNode.Name.LocalName, "value", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var nodeCondition = currentXmlNode.Parent;

                if (nodeCondition == null || !string.Equals(nodeCondition.Name.LocalName, "condition", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var listContainingSpans = spans
                    .Where(s => s.Span.Contains(extent.Start)
                        && s.Span.Contains(extent)
                    )
                    .ToList();

                var containingXmlText = listContainingSpans
                    .Where(s => s.ClassificationType.IsOfType("XML Text"))
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

                if (containingXmlText == null)
                {
                    containingXmlText = listContainingSpans
                        .Where(s => s.ClassificationType.IsOfType("XML Delimiter")
                            && (s.Span.GetText().StartsWith(">", StringComparison.InvariantCultureIgnoreCase)
                                || s.Span.GetText().EndsWith("</", StringComparison.InvariantCultureIgnoreCase))
                        )
                        .OrderByDescending(s => s.Span.Start.Position)
                        .FirstOrDefault();

                    if (containingXmlText != null)
                    {
                        isQuotes = true;
                    }
                }

                if (containingXmlText == null)
                {
                    return;
                }

                ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(containingXmlText.Span, SpanTrackingMode.EdgeInclusive);

                if (isQuotes)
                {
                    applicableTo = snapshot.CreateTrackingSpan(containingXmlText.Span.Start.Position + 1, 0, SpanTrackingMode.EdgeInclusive);
                }

                FillEntityAttributeValuesInList(completionSets, applicableTo, repository, nodeCondition, aliases);
            }
        }

        private void FillLinkedEntityFromField(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var parentEntityName = GetParentEntityName(currentXmlNode);

            var linkEntityName = GetAttributeValue(currentXmlNode, "name");

            FillPrimaryEntityAttributes(completionSets, applicableTo, repository, linkEntityName, parentEntityName);
        }

        private void FillLinkedEntityToField(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var parentEntityName = GetParentEntityName(currentXmlNode);

            var linkEntityName = GetAttributeValue(currentXmlNode, "name");

            FillPrimaryEntityAttributes(completionSets, applicableTo, repository, parentEntityName, linkEntityName);
        }

        private void FillPrimaryEntityAttributes(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, string primaryEntityName, string secondaryName)
        {
            if (string.IsNullOrEmpty(primaryEntityName))
            {
                return;
            }

            var primaryEntityData = repository.GetEntityAttributeIntellisense(primaryEntityName);

            if (primaryEntityData == null || primaryEntityData.Attributes == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(secondaryName))
            {
                var secondaryEntityData = repository.GetEntityAttributeIntellisense(secondaryName);

                if (secondaryEntityData != null)
                {
                    var secondaryReferenceEntities = GetReferencedEntities(secondaryEntityData);

                    var primaryReferenceEntities = GetReferencedEntities(primaryEntityData);

                    var commonEntities = new HashSet<string>(secondaryReferenceEntities.Intersect(primaryReferenceEntities), StringComparer.InvariantCultureIgnoreCase);

                    if (commonEntities.Any())
                    {
                        var attributes = GetRefererenceAttributes(primaryEntityData, commonEntities);

                        if (attributes.Any())
                        {
                            List<CrmCompletion> list = new List<CrmCompletion>();

                            string entityDescription = GetDisplayTextEntity(primaryEntityData);

                            foreach (var attrName in attributes)
                            {
                                if (primaryEntityData.Attributes.ContainsKey(attrName))
                                {
                                    var attribute = primaryEntityData.Attributes[attrName];

                                    string attributeDescription = GetDisplayTextAttribute(primaryEntityData.EntityLogicalName, attribute);

                                    List<string> compareValues = GetCompareValuesForAttribute(attribute);

                                    list.Add(CreateCompletion(attributeDescription.ToString(), attribute.LogicalName, CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
                                }
                            }

                            if (list.Count > 0)
                            {
                                list = list.OrderBy(a => a.DisplayText).ToList();

                                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0} Calculated", primaryEntityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                            }
                        }
                    }
                }
            }

            {
                List<CrmCompletion> list = new List<CrmCompletion>();

                string entityDescription = GetDisplayTextEntity(primaryEntityData);

                var keys = primaryEntityData.Attributes.Keys.ToList();

                foreach (var attrName in keys)
                {
                    var attribute = primaryEntityData.Attributes[attrName];

                    if (attribute.AttributeType == AttributeTypeCode.Uniqueidentifier
                        || attribute.AttributeType == AttributeTypeCode.Customer
                        || attribute.AttributeType == AttributeTypeCode.Lookup
                        || attribute.AttributeType == AttributeTypeCode.Owner
                        )
                    {
                        string attributeDescription = GetDisplayTextAttribute(primaryEntityData.EntityLogicalName, attribute);

                        List<string> compareValues = GetCompareValuesForAttribute(attribute);

                        list.Add(CreateCompletion(attributeDescription.ToString(), attribute.LogicalName, CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
                    }
                }

                if (list.Count > 0)
                {
                    list = list.OrderBy(a => a.DisplayText).ToList();

                    completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, string.Format("{0} References", primaryEntityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                }
            }
        }

        private HashSet<string> GetRefererenceAttributes(EntityIntellisenseData entityData, HashSet<string> commonEntities)
        {
            HashSet<string> result = new HashSet<string>();

            if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
            {
                if (commonEntities.Contains(entityData.EntityLogicalName))
                {
                    result.Add(entityData.EntityPrimaryIdAttribute);
                }
            }

            if (entityData.ManyToOneRelationships != null)
            {
                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    if (commonEntities.Contains(item.TargetEntityName))
                    {
                        result.Add(item.BaseAttributeName);
                    }
                }
            }

            if (entityData.IsIntersectEntity && entityData.ManyToManyRelationships != null)
            {
                foreach (var item in entityData.ManyToManyRelationships.Values)
                {
                    if (commonEntities.Contains(item.Entity1Name))
                    {
                        result.Add(item.Entity1IntersectAttributeName);
                    }

                    if (commonEntities.Contains(item.Entity2Name))
                    {
                        result.Add(item.Entity2IntersectAttributeName);
                    }
                }
            }

            return result;
        }

        private HashSet<string> GetReferencedEntities(EntityIntellisenseData entityData)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (!string.IsNullOrEmpty(entityData.EntityPrimaryIdAttribute))
            {
                result.Add(entityData.EntityLogicalName);
            }

            if (entityData.ManyToOneRelationships != null)
            {
                foreach (var item in entityData.ManyToOneRelationships.Values)
                {
                    if (!string.IsNullOrEmpty(item.TargetEntityName))
                    {
                        result.Add(item.TargetEntityName);
                    }
                }
            }

            if (entityData.IsIntersectEntity)
            {
                if (entityData.ManyToManyRelationships != null)
                {
                    foreach (var item in entityData.ManyToManyRelationships.Values)
                    {
                        if (!string.IsNullOrEmpty(item.Entity1Name))
                        {
                            result.Add(item.Entity1Name);
                        }

                        if (!string.IsNullOrEmpty(item.Entity2Name))
                        {
                            result.Add(item.Entity2Name);
                        }
                    }
                }
            }

            return result;
        }

        private void FillLinkedEntityNames(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            var entityName = GetParentEntityName(currentXmlNode);

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var connectionIntellisense = repository.GetEntitiesIntellisenseData();

            if (connectionIntellisense == null || connectionIntellisense.Entities == null)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityName);

            if (entityData == null)
            {
                return;
            }

            HashSet<string> linkedEntities = entityData.GetLinkedEntities(connectionIntellisense);

            foreach (var linkedEntityName in linkedEntities)
            {
                if (connectionIntellisense.Entities.ContainsKey(linkedEntityName))
                {
                    var linkedEntityData = connectionIntellisense.Entities[linkedEntityName];

                    string entityDescription = GetDisplayTextEntity(linkedEntityData);

                    List<string> compareValues = GetCompareValuesForEntity(linkedEntityData);

                    list.Add(CreateCompletion(entityDescription, linkedEntityData.EntityLogicalName, CreateEntityDescription(linkedEntityData), _defaultGlyph, compareValues));
                }
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(a => a.DisplayText).ToList();

                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "Linked Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillAliases(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, Dictionary<string, string> aliases)
        {
            List<CrmCompletion> list = new List<CrmCompletion>();

            foreach (var aliasName in aliases.Keys.OrderBy(s => s))
            {
                list.Add(CreateCompletion(aliasName, aliasName, null, _defaultGlyph, null));
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(a => a.DisplayText).ToList();

                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All Aliases", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityNamesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, bool isObjectTypeCode)
        {
            var connectionIntellisense = repository.GetEntitiesIntellisenseData();

            if (connectionIntellisense == null || connectionIntellisense.Entities == null)
            {
                return;
            }

            List<CrmCompletion> list = new List<CrmCompletion>();

            var keys = connectionIntellisense.Entities.Keys.ToList();

            foreach (var entityName in keys)
            {
                var entityData = connectionIntellisense.Entities[entityName];

                string entityDescription = GetDisplayTextEntity(entityData);

                List<string> compareValues = GetCompareValuesForEntity(entityData);

                var insertionText = entityData.EntityLogicalName;

                if (isObjectTypeCode)
                {
                    insertionText = entityData.ObjectTypeCode.ToString();
                }

                list.Add(CreateCompletion(entityDescription, insertionText, CreateEntityDescription(entityData), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(a => a.DisplayText).ToList();

                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, "All Entities", applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private List<string> GetCompareValuesForEntity(EntityIntellisenseData entityData)
        {
            List<string> result = GetCompareValues(entityData.DisplayName);

            result.Add(entityData.EntityLogicalName);

            if (entityData.IsIntersectEntity)
            {
                result.Add("IntersectEntity");
            }

            if (entityData.ObjectTypeCode.HasValue)
            {
                result.Add(entityData.ObjectTypeCode.Value.ToString());
            }

            return result;
        }

        private List<string> GetCompareValuesForAttribute(AttributeIntellisenseData attribute)
        {
            List<string> result = GetCompareValues(attribute.DisplayName);

            result.Add(attribute.LogicalName);

            if (attribute.AttributeType.HasValue)
            {
                result.Add(attribute.AttributeType.ToString());
            }

            if (attribute.Targets != null && attribute.Targets.Length > 0)
            {
                result.AddRange(attribute.Targets);
            }

            return result;
        }

        private List<string> GetCompareValues(Label label)
        {
            List<string> result = new List<string>();

            if (label != null)
            {
                result.AddRange(label.LocalizedLabels.Where(l => !string.IsNullOrEmpty(l.Label)).Select(l => l.Label).Distinct());
            }

            return result;
        }

        private void FillEntityAttributesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            var entityName = GetParentEntityName(currentXmlNode);

            if (string.IsNullOrEmpty(entityName))
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityName);

            if (entityData == null
                || entityData.Attributes == null
                )
            {
                return;
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);
        }

        private void FillEntityAttributesInListForGrid(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode)
        {
            int? entityTypeCode = GetParentEntityObjectTypeCode(currentXmlNode);

            if (!entityTypeCode.HasValue)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityTypeCode.Value);

            if (entityData == null
               || entityData.Attributes == null
               )
            {
                return;
            }

            FillEntityIntellisenseDataAttributes(completionSets, applicableTo, entityData);
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

            string entityDescription = GetDisplayTextEntity(entityData);

            var keys = entityData.Attributes.Keys.ToList();

            foreach (var attrName in keys)
            {
                var attribute = entityData.Attributes[attrName];

                string attributeDescription = GetDisplayTextAttribute(entityData.EntityLogicalName, attribute);

                List<string> compareValues = GetCompareValuesForAttribute(attribute);

                list.Add(CreateCompletion(attributeDescription, attribute.LogicalName, CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(a => a.DisplayText).ToList();

                completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0} Attributes", entityData.EntityLogicalName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
            }
        }

        private void FillEntityPrimaryAttributeForGrid(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement currentXmlNode, bool isNameAttribute)
        {
            int? entityTypeCode = GetParentEntityObjectTypeCode(currentXmlNode);

            if (!entityTypeCode.HasValue)
            {
                return;
            }

            var entityData = repository.GetEntityAttributeIntellisense(entityTypeCode.Value);

            if (entityData == null
               || entityData.Attributes == null
               )
            {
                return;
            }

            if (isNameAttribute && string.IsNullOrEmpty(entityData.EntityPrimaryNameAttribute))
            {
                return;
            }

            AttributeIntellisenseData attribute = null;

            if (isNameAttribute)
            {
                if (entityData.Attributes.ContainsKey(entityData.EntityPrimaryNameAttribute))
                {
                    attribute = entityData.Attributes[entityData.EntityPrimaryNameAttribute];
                }
            }
            else
            {
                if (entityData.Attributes.ContainsKey(entityData.EntityPrimaryIdAttribute))
                {
                    attribute = entityData.Attributes[entityData.EntityPrimaryIdAttribute];
                }
            }

            if (attribute == null)
            {
                return;
            }

            string entityDescription = GetDisplayTextEntity(entityData);

            string attributeDescription = GetDisplayTextAttribute(entityData.EntityLogicalName, attribute);

            List<CrmCompletion> list = new List<CrmCompletion>();

            List<string> compareValues = GetCompareValuesForAttribute(attribute);

            list.Add(CreateCompletion(attributeDescription, attribute.LogicalName, CreateAttributeDescription(entityDescription, attribute), _defaultGlyph, compareValues));

            var displayName = string.Format("{0} PrimaryIdAttribute", entityData.EntityLogicalName);

            if (isNameAttribute)
            {
                displayName = string.Format("{0} PrimaryNameAttribute", entityData.EntityLogicalName);
            }

            completionSets.Add(new CrmCompletionSet(SourceNameMonikerAll, displayName, applicableTo, list, Enumerable.Empty<CrmCompletion>()));
        }

        private string GetDisplayTextEntity(EntityIntellisenseData entityData)
        {
            StringBuilder result = new StringBuilder(entityData.EntityLogicalName);

            string temp = CreateFileHandler.GetLocalizedLabel(entityData.DisplayName);

            if (!string.IsNullOrEmpty(temp))
            {
                result.AppendFormat(" - {0}", temp);
            }

            if (entityData.IsIntersectEntity)
            {
                result.Append(" - IntersectEntity");
            }

            return result.ToString();
        }

        private string GetDisplayTextAttribute(string entityName, AttributeIntellisenseData attribute)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}.{1}", entityName, attribute.LogicalName);

            string temp = CreateFileHandler.GetLocalizedLabel(attribute.DisplayName);

            if (!string.IsNullOrEmpty(temp))
            {
                result.AppendFormat(" - {0}", temp);
            }

            if (attribute.AttributeType.HasValue)
            {
                result.AppendFormat(" - {0}", attribute.AttributeType.ToString());
            }

            return result.ToString();
        }

        private string GetDisplayTextOptionSetValue(string entityName, string attributeName, OptionMetadata optionMetadata)
        {
            return string.Format("{0}.{1}    {2} - {3}", entityName, attributeName, CreateFileHandler.GetLocalizedLabel(optionMetadata.Label), optionMetadata.Value);
        }

        private void FillEntityAttributeValuesInList(IList<CompletionSet> completionSets, ITrackingSpan applicableTo, ConnectionIntellisenseDataRepository repository, XElement nodeCondition, Dictionary<string, string> aliases)
        {
            string entityName = string.Empty;

            var attributeName = GetAttributeValue(nodeCondition, "attribute");

            var aliasInCondition = GetAttributeValue(nodeCondition, "alias");

            if (!string.IsNullOrEmpty(aliasInCondition) && aliases.ContainsKey(aliasInCondition))
            {
                entityName = aliases[aliasInCondition];
            }
            else
            {
                entityName = GetParentEntityName(nodeCondition);
            }

            if (!string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(attributeName))
            {
                var entityData = repository.GetEntityAttributeIntellisense(entityName);

                if (entityData != null && entityData.Attributes != null)
                {
                    if (entityData.Attributes.ContainsKey(attributeName))
                    {
                        var attributeData = entityData.Attributes[attributeName];

                        if (attributeData.IsBooleanAttribute)
                        {
                            List<CrmCompletion> list = new List<CrmCompletion>();

                            list.Add(CreateCompletion("0", "0", null, _defaultGlyph, Enumerable.Empty<string>()));
                            list.Add(CreateCompletion("1", "1", null, _defaultGlyph, Enumerable.Empty<string>()));
                            list.Add(CreateCompletion("false", "false", null, _defaultGlyph, Enumerable.Empty<string>()));
                            list.Add(CreateCompletion("true", "true", null, _defaultGlyph, Enumerable.Empty<string>()));

                            if (attributeData.OptionSet != null && attributeData.OptionSet.OptionSetMetadata is BooleanOptionSetMetadata boolOptionSet)
                            {
                                string entityDescription = GetDisplayTextEntity(entityData);
                                string attributeDescription = GetDisplayTextAttribute(entityName, attributeData);

                                if (boolOptionSet.FalseOption != null)
                                {
                                    string displayText = GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, boolOptionSet.FalseOption);

                                    List<string> compareValues = GetCompareValues(boolOptionSet.FalseOption.Label);

                                    list.Add(CreateCompletion(displayText, boolOptionSet.FalseOption.Value.ToString(), CreateOptionValueDescription(entityDescription, attributeDescription, boolOptionSet.FalseOption), _defaultGlyph, compareValues));
                                }

                                if (boolOptionSet.TrueOption != null)
                                {
                                    string displayText = GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, boolOptionSet.TrueOption);

                                    List<string> compareValues = GetCompareValues(boolOptionSet.TrueOption.Label);

                                    list.Add(CreateCompletion(displayText, boolOptionSet.TrueOption.Value.ToString(), CreateOptionValueDescription(entityDescription, attributeDescription, boolOptionSet.TrueOption), _defaultGlyph, compareValues));
                                }
                            }

                            completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0}.{1} Values", entityName, attributeName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                        }
                        else if (attributeData.IsEntityNameAttribute)
                        {
                            FillEntityNamesInList(completionSets, applicableTo, repository, false);
                        }
                        else if (attributeData.OptionSet != null)
                        {
                            if (attributeData.OptionSet != null && attributeData.OptionSet.OptionSetMetadata is OptionSetMetadata optionSet)
                            {
                                List<CrmCompletion> list = new List<CrmCompletion>();

                                string entityDescription = GetDisplayTextEntity(entityData);
                                string attributeDescription = GetDisplayTextAttribute(entityName, attributeData);

                                foreach (var item in optionSet.Options.OrderBy(e => e.Value))
                                {
                                    string displayText = GetDisplayTextOptionSetValue(entityData.EntityLogicalName, attributeData.LogicalName, item);

                                    List<string> compareValues = GetCompareValues(item.Label);

                                    list.Add(CreateCompletion(displayText, item.Value.ToString(), CreateOptionValueDescription(entityDescription, attributeDescription, item), _defaultGlyph, compareValues));
                                }

                                if (list.Count > 0)
                                {
                                    list = list.OrderBy(a => a.DisplayText).ToList();

                                    completionSets.Add(new CrmCompletionSet(SourceNameMoniker, string.Format("{0}.{1} Values", entityName, attributeName), applicableTo, list, Enumerable.Empty<CrmCompletion>()));
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetAttributeValue(XElement nodeCondition, string attributeName)
        {
            var attrName = nodeCondition.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, attributeName, StringComparison.InvariantCultureIgnoreCase));

            if (attrName != null)
            {
                if (!string.IsNullOrEmpty(attrName.Value))
                {
                    return attrName.Value;
                }
            }

            return null;
        }

        private HashSet<string> GetUsedEntities(XElement doc)
        {
            HashSet<string> result = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            var entityElements = doc.DescendantsAndSelf().Where(IsEntityOrLinkElement);

            foreach (var entity in entityElements)
            {
                var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));

                if (attrName != null)
                {
                    if (!string.IsNullOrEmpty(attrName.Value))
                    {
                        result.Add(attrName.Value);
                    }
                }
            }

            return result;
        }

        private HashSet<int> GetUsedEntityObjectTypeCodes(XElement doc)
        {
            HashSet<int> result = new HashSet<int>();

            var entityElements = doc.DescendantsAndSelf().Where(IsGridElement);

            foreach (var entity in entityElements)
            {
                var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "object", StringComparison.InvariantCultureIgnoreCase));

                if (attrName != null
                    && !string.IsNullOrEmpty(attrName.Value)
                    && int.TryParse(attrName.Value, out int tempInt)
                    )
                {
                    result.Add(tempInt);
                }
            }

            return result;
        }

        private Dictionary<string, string> GetEntityAliases(XElement doc)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            var entityElements = doc.DescendantsAndSelf().Where(IsEntityOrLinkElement);

            foreach (var entity in entityElements)
            {
                var attrName = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));
                var attrAlias = entity.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "alias", StringComparison.InvariantCultureIgnoreCase));

                if (attrName != null && attrAlias != null)
                {
                    if (!string.IsNullOrEmpty(attrName.Value) && !string.IsNullOrEmpty(attrAlias.Value))
                    {
                        if (!result.ContainsKey(attrName.Value))
                        {
                            result.Add(attrAlias.Value, attrName.Value);
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsEntityOrLinkElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsGridElement(XElement element)
        {
            return string.Equals(element.Name.LocalName, "grid", StringComparison.OrdinalIgnoreCase);
        }

        private string CreateEntityDescription(EntityIntellisenseData entity)
        {
            List<string> lines = new List<string>();

            if (entity.IsIntersectEntity)
            {
                lines.Add("IntersectEntity");

                if (entity.ManyToManyRelationships != null)
                {
                    var relations = entity.ManyToManyRelationships.Values.Where(r => string.Equals(r.IntersectEntityName, entity.EntityLogicalName, StringComparison.InvariantCultureIgnoreCase));

                    foreach (var rel in relations.OrderBy(r => r.Entity1Name).ThenBy(r => r.Entity2Name).ThenBy(r => r.Entity1IntersectAttributeName).ThenBy(r => r.Entity2IntersectAttributeName))
                    {
                        lines.Add(string.Format("{0} - {1}", rel.Entity1Name, rel.Entity2Name));
                    }
                }
            }

            CreateFileHandler.FillLabelEntity(lines, true, entity.DisplayName, entity.DisplayCollectionName, entity.Description);

            return string.Join(System.Environment.NewLine, lines);
        }

        private string CreateAttributeDescription(string entityDescription, AttributeIntellisenseData attribute)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(entityDescription))
            {
                lines.Add(string.Format("Entity:    {0}", entityDescription));
            }

            if (attribute.Targets != null && attribute.Targets.Length > 0)
            {
                if (attribute.Targets.Length <= 6)
                {
                    string targets = string.Join(",", attribute.Targets.OrderBy(s => s));

                    lines.Add(string.Format("Targets:    {0}", targets));
                }
                else
                {
                    lines.Add(string.Format("Targets Count: {0}", attribute.Targets.Length));
                }
            }

            CreateFileHandler.FillLabelDisplayNameAndDescription(lines, true, attribute.DisplayName, attribute.Description);

            return string.Join(System.Environment.NewLine, lines);
        }

        private string CreateOptionValueDescription(string entityDescription, string attributeDescription, OptionMetadata optionMetadata)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(entityDescription))
            {
                lines.Add(string.Format("Entity:      {0}", entityDescription));
            }
            if (!string.IsNullOrEmpty(attributeDescription))
            {
                lines.Add(string.Format("Attribute:      {0}", attributeDescription));
            }

            CreateFileHandler.FillLabelDisplayNameAndDescription(lines, true, optionMetadata.Label, optionMetadata.Description, "    ");

            return string.Join(System.Environment.NewLine, lines);
        }

        private ClassificationSpan GetCurrentXmlAttributeName(ITextSnapshot snapshot, ClassificationSpan containingSpan, IList<ClassificationSpan> spans)
        {
            var currentAttr = spans
                    .Where(s => s.ClassificationType.IsOfType("XML Attribute") && s.Span.Start <= containingSpan.Span.Start)
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

            if (currentAttr != null)
            {
                return currentAttr;
            }

            var allSpans = _classifier.GetClassificationSpans(new SnapshotSpan(containingSpan.Span.Snapshot, 0, containingSpan.Span.Snapshot.Length));

            currentAttr = allSpans
                    .Where(s => s.ClassificationType.IsOfType("XML Name"))
                    .OrderByDescending(s => s.Span.Start.Position)
                    .FirstOrDefault();

            if (currentAttr != null)
            {
                return currentAttr;
            }

            //var containgLine = containingSpan.Span.Start.GetContainingLine();

            //for (int i = containgLine.LineNumber - 1; i >= 0; i--)
            //{
            //    var previousLine = snapshot.GetLineFromLineNumber(i);

            //    SnapshotSpan line = previousLine.Extent;

            //    var previousLineSpans = _classifier.GetClassificationSpans(previousLine.Extent);


            //}

            return null;
        }

        private XElement GetCurrentXmlNode(XElement doc, SnapshotSpan extent)
        {
            IList<ClassificationSpan> spans = _classifier.GetClassificationSpans(new SnapshotSpan(extent.Snapshot, 0, extent.Snapshot.Length));

            var elementSpan = spans
                     .Where(s => s.ClassificationType.IsOfType("XML Name") && s.Span.Start <= extent.Start)
                     .OrderByDescending(s => s.Span.Start.Position)
                     .FirstOrDefault();

            if (elementSpan == null)
            {
                return null;
            }

            var line = elementSpan.Span.Start.Subtract(1).GetContainingLine();

            var lineNumber = line.LineNumber + 1;
            var linePosition = elementSpan.Span.Start.Subtract(1).Position - line.Start.Position + 2;

            var result = doc.DescendantsAndSelf().FirstOrDefault(e => (e as IXmlLineInfo)?.LineNumber == lineNumber && (e as IXmlLineInfo)?.LinePosition == linePosition);

            var list = doc.DescendantsAndSelf().Select(e => new { Element = e, (e as IXmlLineInfo)?.LineNumber, (e as IXmlLineInfo)?.LinePosition }).ToList();

            if (result == null)
            {

            }

            return result;
        }

        private string GetParentEntityName(XElement currentXmlNode)
        {
            var entityElement = currentXmlNode.Ancestors().FirstOrDefault(IsEntityOrLinkEntity);

            if (entityElement != null)
            {
                var attrKey = entityElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "name", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey != null)
                {
                    return attrKey.Value;
                }
            }

            return null;
        }

        private int? GetParentEntityObjectTypeCode(XElement currentXmlNode)
        {
            var entityElement = currentXmlNode.AncestorsAndSelf().FirstOrDefault(IsGridElement);

            if (entityElement != null)
            {
                var attrObject = entityElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "object", StringComparison.InvariantCultureIgnoreCase));

                if (attrObject != null
                    && !string.IsNullOrEmpty(attrObject.Value)
                    && int.TryParse(attrObject.Value, out int tempInt)
                    )
                {
                    return tempInt;
                }
            }

            return null;
        }

        private static bool IsEntityOrLinkEntity(XElement element)
        {
            return string.Equals(element.Name.LocalName, "entity", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(element.Name.LocalName, "link-entity", StringComparison.InvariantCultureIgnoreCase);
        }

        private ITrackingSpan FindTokenSpanAtPosition(ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = _navigator.GetTextStructureNavigator(_buffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }

        private static XElement ReadXmlDocument(string text)
        {
            try
            {
                //XDocument doc = XDocument.Parse(Regex.Replace(text, " xmlns(:[^\"]+)?=\"([^\"]+)\"", string.Empty));
                XDocument doc = XDocument.Parse(text, LoadOptions.SetLineInfo);

                return doc.Root;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private CrmCompletion CreateCompletion(string displayText, string insertionText, string description, ImageSource glyph, IEnumerable<string> compareValues)
        {
            if (glyph == null)
                glyph = _defaultGlyph;

            return new CrmCompletion(displayText, insertionText, description, glyph, null, compareValues);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
    }
}