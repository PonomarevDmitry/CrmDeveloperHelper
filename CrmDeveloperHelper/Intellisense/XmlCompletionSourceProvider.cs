using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Name("FetchXml")]
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("XML")]
    [Order(Before = "default")]
    [Order(Before = "high")]
    [Order(Before = Priority.High)]
    [Order(Before = Priority.Default)]
    public class XmlCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import(typeof(ITextStructureNavigatorSelectorService))]
        public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import(typeof(IClassifierAggregatorService))]
        public IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        [Import(typeof(IGlyphService))]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new XmlCompletionSource(this, textBuffer, ClassifierAggregatorService, NavigatorService, GlyphService);
        }
    }
}