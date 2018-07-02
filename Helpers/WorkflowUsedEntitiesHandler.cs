using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class WorkflowUsedEntitiesHandler
    {
        private Dictionary<string, Guid> _usedGuids;
        private Dictionary<string, string[]> _usedCreateEntityReference;

        public Task<List<EntityReference>> GetWorkflowUsedEntitiesAsync(XElement doc)
        {
            return Task.Run(() => GetWorkflowUsedEntities(doc));
        }

        private List<EntityReference> GetWorkflowUsedEntities(XElement doc)
        {
            List<EntityReference> result = new List<EntityReference>();

            this._usedGuids = new Dictionary<string, Guid>(StringComparer.InvariantCultureIgnoreCase);

            this._usedCreateEntityReference = new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);

            FillGuids(doc);

            FillEntityReference(doc);

            foreach (var reference in this._usedCreateEntityReference)
            {
                if (reference.Value.Length == 4)
                {
                    var entityName = reference.Value[0];
                    var name = reference.Value[1];
                    var key = reference.Value[2];
                    var type = reference.Value[3];

                    if (_usedGuids.ContainsKey(key) && string.Equals(type, "Lookup", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var newValue = new EntityReference(entityName, _usedGuids[key])
                        {
                            Name = name,
                        };

                        if (!result.Contains(newValue))
                        {
                            result.Add(newValue);
                        }
                    }
                }
            }

            return result;
        }

        private static readonly Regex regexGuid = new Regex(@"\[New Object\(\) { Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.Guid, (.*) }\]", RegexOptions.Compiled);
        private static readonly Regex regexEntityReference = new Regex(@"\[New Object\(\) { Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.EntityReference, (.*) }\]", RegexOptions.Compiled);

        private static readonly Regex regexParameters = new Regex(@"([^,]*),?", RegexOptions.Compiled);

        private void FillGuids(XElement doc)
        {
            var inArguments = doc.Descendants().Where(IsInArgumentCreateCrmType);

            foreach (var inArg in inArguments)
            {
                var nodeParameters = inArg.ElementsAfterSelf().FirstOrDefault(IsInArgumentParametersGuid);

                var nodeResult = inArg.ElementsAfterSelf().FirstOrDefault(IsOutArgumentResult);

                if (nodeParameters != null && nodeResult != null)
                {
                    var result = nodeResult.Value;

                    result = result.Replace("[", string.Empty).Replace("]", string.Empty);

                    var parameters = nodeParameters.Value;

                    Match match = regexGuid.Match(parameters);

                    //[New Object() { Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.Guid, "53df4519-cb4c-e511-80e5-02bfc0a81214", "Key" }]

                    if (match.Success && match.Groups.Count > 1)
                    {
                        var arguments = match.Groups[1].Value;

                        var split = arguments.Split(',');

                        if (split.Length == 2)
                        {
                            var arg1 = split[0];

                            arg1 = arg1.Replace("\"", string.Empty);

                            if (Guid.TryParse(arg1, out Guid tempGuid) && tempGuid != Guid.Empty)
                            {
                                _usedGuids.Add(result, tempGuid);
                            }
                        }
                    }
                }
            }
        }

        private void FillEntityReference(XElement doc)
        {
            var inArguments = doc.Descendants().Where(IsInArgumentCreateCrmType);

            foreach (var inArg in inArguments)
            {
                var nodeParameters = inArg.ElementsAfterSelf().FirstOrDefault(IsInArgumentParametersEntityReference);

                var nodeResult = inArg.ElementsAfterSelf().FirstOrDefault(IsOutArgumentResult);

                if (nodeParameters != null && nodeResult != null)
                {
                    var result = nodeResult.Value;

                    result = result.Replace("[", string.Empty).Replace("]", string.Empty);

                    var parameters = nodeParameters.Value;

                    Match match = regexEntityReference.Match(parameters);

                    if (match.Success && match.Groups.Count > 1)
                    {
                        var arguments = match.Groups[1].Value;

                        var split = arguments.Split(',');

                        for (int i = 0; i < split.Length; i++)
                        {
                            split[i] = System.Net.WebUtility.HtmlDecode(split[i]).Trim('"', ' '); // XmlConvert.DecodeName();
                        }

                        _usedCreateEntityReference.Add(result, split);
                    }
                }
            }
        }

        private static bool IsInArgumentCreateCrmType(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "InArgument", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            {
                var attrTypeArguments = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "TypeArguments", StringComparison.InvariantCultureIgnoreCase));

                if (attrTypeArguments == null || !attrTypeArguments.Value.Contains("String"))
                {
                    return false;
                }
            }

            {
                var attrKey = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Key", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey == null || !string.Equals(attrKey.Value, "ExpressionOperator", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            if (!string.Equals(element.Value, "CreateCrmType", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static bool IsInArgumentParametersGuid(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "InArgument", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            {
                var attrTypeArguments = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "TypeArguments", StringComparison.InvariantCultureIgnoreCase));

                if (attrTypeArguments == null || !attrTypeArguments.Value.Contains("Object[]"))
                {
                    return false;
                }
            }

            {
                var attrKey = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Key", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey == null || !string.Equals(attrKey.Value, "Parameters", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            if (!element.Value.Contains("Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.Guid"))
            {
                return false;
            }

            return true;
        }

        private static bool IsOutArgumentResult(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "OutArgument", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            {
                var attrTypeArguments = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "TypeArguments", StringComparison.InvariantCultureIgnoreCase));

                if (attrTypeArguments == null || !attrTypeArguments.Value.Contains("Object"))
                {
                    return false;
                }
            }

            {
                var attrKey = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Key", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey == null || !string.Equals(attrKey.Value, "Result", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsInArgumentParametersEntityReference(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "InArgument", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            {
                var attrTypeArguments = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "TypeArguments", StringComparison.InvariantCultureIgnoreCase));

                if (attrTypeArguments == null || !attrTypeArguments.Value.Contains("Object[]"))
                {
                    return false;
                }
            }

            {
                var attrKey = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Key", StringComparison.InvariantCultureIgnoreCase));

                if (attrKey == null || !string.Equals(attrKey.Value, "Parameters", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            if (!element.Value.Contains("Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.EntityReference"))
            {
                return false;
            }

            return true;
        }

        public static void ReplaceGuids(XElement doc)
        {
            var inArguments = doc.Descendants().Where(IsInArgumentCreateCrmType);

            foreach (var inArg in inArguments)
            {
                var nodeParameters = inArg.ElementsAfterSelf().FirstOrDefault(IsInArgumentParametersGuid);

                var nodeResult = inArg.ElementsAfterSelf().FirstOrDefault(IsOutArgumentResult);

                if (nodeParameters != null && nodeResult != null)
                {
                    var parameters = nodeParameters.Value;

                    Match match = regexGuid.Match(parameters);

                    //[New Object() { Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.Guid, "53df4519-cb4c-e511-80e5-02bfc0a81214", "Key" }]

                    if (match.Success && match.Groups.Count > 1)
                    {
                        var arguments = match.Groups[1].Value;

                        var split = arguments.Split(',');

                        if (split.Length == 2)
                        {
                            var arg1 = split[0];

                            arg1 = arg1.Replace("\"", string.Empty);

                            if (Guid.TryParse(arg1, out Guid tempGuid) && tempGuid != Guid.Empty)
                            {
                                var newMatch = string.Format("\"{0}\"", Guid.Empty) + ", " + split[1];

                                var newParameters = "[New Object() { Microsoft.Xrm.Sdk.Workflow.WorkflowPropertyType.Guid, " + newMatch + " }]";

                                nodeParameters.Value = newParameters;
                            }
                        }
                    }
                }
            }
        }
    }
}