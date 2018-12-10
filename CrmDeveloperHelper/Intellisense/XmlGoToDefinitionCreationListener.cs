using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("CrmXmlGoToDefinitionHandler")]
    [ContentType("XML")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class XmlGoToDefinitionCreationListener : IVsTextViewCreationListener
    {
        [Import(typeof(IVsEditorAdaptersFactoryService))]
        public IVsEditorAdaptersFactoryService AdapterService { get; set; }

        [Import(typeof(SVsServiceProvider))]
        public SVsServiceProvider ServiceProvider { get; set; }

        [Import(typeof(ITextStructureNavigatorSelectorService))]
        public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import(typeof(IClassifierAggregatorService))]
        public IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);

            if (textView == null)
            {
                return;
            }

            Func<XmlGoToDefinitionCommandHandler> createCommandHandler = () => new XmlGoToDefinitionCommandHandler(textViewAdapter, textView, this);
            textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
        }
    }
}
