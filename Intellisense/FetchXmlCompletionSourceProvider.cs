using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(ICompletionSourceProvider))]
    [Name("FetchXml")]
    [ContentType("XML")]
    public class FetchXmlCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import(typeof(ITextStructureNavigatorSelectorService))]
        public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import(typeof(IClassifierAggregatorService))]
        public IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        [Import(typeof(IGlyphService))]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new FetchXmlCompletionSource(this, textBuffer, ClassifierAggregatorService, NavigatorService, GlyphService);
        }
    }
}