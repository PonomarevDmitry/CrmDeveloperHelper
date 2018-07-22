using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(ClassificationTag))]
    public class CustomUrlClassifierProvider : ITaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            ITagAggregator<UrlTag> tagAggregator = aggregatorFactory.CreateTagAggregator<UrlTag>(buffer);

            return new CustomUrlClassifier(buffer, tagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }
}
