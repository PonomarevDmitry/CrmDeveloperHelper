using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class LabelReplacer
    {
        private Translation _translation;

        public LabelReplacer(Translation translation)
        {
            this._translation = translation;
        }

        public void FullfillLabelsForWorkflow(XElement doc)
        {
            if (_translation == null)
            {
                return;
            }

            //<Variable x:TypeArguments="x:String" Default="c2fc1ddf-d129-435e-b63e-91ccc0ef716b" Name="stepLabelLabelId" />
            var elements = doc.DescendantsAndSelf().Where(IsLabelVariable).ToList();

            foreach (var el in elements)
            {
                var attr = el.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Default", StringComparison.OrdinalIgnoreCase));

                Guid temp;

                if (Guid.TryParse(attr.Value, out temp))
                {
                    var loc = _translation.LocalizedLabels.FirstOrDefault(a => a.ObjectId == temp);

                    if (loc != null)
                    {
                        attr.Value = string.Join(";", loc.Labels.OrderBy(l => l.LanguageCode).Select(l => string.Format("{0} - {1}", l.LanguageCode, l.Value)));
                    }
                    else
                    {
                        attr.Value = string.Empty;
                    }
                }
            }
        }

        private static bool IsLabelVariable(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "Variable", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "TypeArguments", StringComparison.OrdinalIgnoreCase));

                if (attr == null)
                {
                    return false;
                }

                if (!string.Equals(attr.Value, "x:String", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Name", StringComparison.OrdinalIgnoreCase));

                if (attr == null)
                {
                    return false;
                }

                if (!string.Equals(attr.Value, "stepLabelLabelId", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Default", StringComparison.OrdinalIgnoreCase));

                if (attr == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(attr.Value))
                {
                    return false;
                }

                Guid temp;

                if (!Guid.TryParse(attr.Value, out temp))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
