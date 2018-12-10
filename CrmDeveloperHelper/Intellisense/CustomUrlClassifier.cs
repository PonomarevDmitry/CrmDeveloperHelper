using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class CustomUrlClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;

        ITagAggregator<UrlTag> _aggregator;

        IClassificationTypeRegistryService _typeService;


        public CustomUrlClassifier(ITextBuffer buffer, ITagAggregator<UrlTag> aggregator, IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;

            _aggregator = aggregator;

            _typeService = typeService;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);

                yield return new TagSpan<ClassificationTag>(tagSpans[0], new ClassificationTag(_typeService.GetClassificationType("url")));
            }
        }
    }
}