using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense
{
    public class CustomUrlTagger : ITagger<UrlTag>
    {
        ITextBuffer _buffer;

        internal CustomUrlTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        private char[] _trimChars = "_.,!@#%:;&*()[]{}?'\"".ToArray();

        private const string patternUrl = @"(openinvisualstudio|openinvisualstudiopath|openintexteditor|selectfileinfolder|showdifference|opensolution)\:\/\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9:;@!=&%#_\*\-\.\,\?\/\\\+\$\(\)\[\]\{\}]*)?";

        private Regex regexUrl = new Regex(patternUrl, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IEnumerable<ITagSpan<UrlTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            HashSet<Span> hashSpans = new HashSet<Span>();

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();

                int curLoc = containingLine.Start.Position;

                var line = containingLine.GetText();

                if (regexUrl.IsMatch(line))
                {
                    var matchs = regexUrl.Matches(line);

                    foreach (var item in matchs.OfType<Match>())
                    {
                        var value = item.Value.TrimEnd(_trimChars);

                        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
                        {
                            var span = new Span(curLoc + item.Index, value.Length);

                            if (hashSpans.Add(span))
                            {
                                var tokenSpan = new SnapshotSpan(curSpan.Snapshot, span);

                                if (tokenSpan.IntersectsWith(curSpan))
                                {
                                    yield return new TagSpan<UrlTag>(tokenSpan, new UrlTag(uri));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}