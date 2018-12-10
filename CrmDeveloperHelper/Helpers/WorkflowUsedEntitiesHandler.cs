using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
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
        public Task<List<EntityReference>> GetWorkflowUsedEntitiesAsync(XElement doc)
        {
            return Task.Run(() => GetWorkflowUsedEntities(doc));
        }

        private List<EntityReference> GetWorkflowUsedEntities(XElement doc)
        {
            List<EntityReference> result = new List<EntityReference>();

            Dictionary<string, Guid> usedGuids = new Dictionary<string, Guid>(StringComparer.InvariantCultureIgnoreCase);

            Dictionary<string, string[]> usedCreateEntityReference = new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);

            FillGuids(doc, usedGuids);

            FillEntityReference(doc, usedCreateEntityReference);

            foreach (var reference in usedCreateEntityReference)
            {
                if (reference.Value.Length == 4)
                {
                    var entityName = reference.Value[0];
                    var name = reference.Value[1];
                    var key = reference.Value[2];
                    var type = reference.Value[3];

                    if (usedGuids.ContainsKey(key) && string.Equals(type, "Lookup", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var newValue = new EntityReference(entityName, usedGuids[key])
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

        private void FillGuids(XElement doc, Dictionary<string, Guid> usedGuids)
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
                                usedGuids.Add(result, tempGuid);
                            }
                        }
                    }
                }
            }
        }

        private void FillEntityReference(XElement doc, Dictionary<string, string[]> usedCreateEntityReference)
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

                        usedCreateEntityReference.Add(result, split);
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

        private static readonly string[] _attributesForCreateOrUpdate = new[] { "DisplayName", "Entity", "EntityName" };

        private static bool IsCreateOrUpdateEntity(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "CreateEntity", StringComparison.InvariantCultureIgnoreCase)
                && !string.Equals(element.Name.LocalName, "UpdateEntity", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return false;
            }

            foreach (var attrName in _attributesForCreateOrUpdate)
            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, attrName, StringComparison.InvariantCultureIgnoreCase));

                if (attr == null || string.IsNullOrEmpty(attr.Value))
                {
                    return false;
                }
            }

            return true;
        }

        private static readonly string[] _attributesForSetEntityProperty = new[] { "Attribute", "EntityName", "Value" };

        private static bool IsSetEntityProperty(XElement element, string entityVarible)
        {
            if (!string.Equals(element.Name.LocalName, "SetEntityProperty", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Entity", StringComparison.InvariantCultureIgnoreCase));

                if (attr == null
                    || !string.Equals(attr.Value, entityVarible, StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return false;
                }
            }

            foreach (var attrName in _attributesForSetEntityProperty)
            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, attrName, StringComparison.InvariantCultureIgnoreCase));

                if (attr == null || string.IsNullOrEmpty(attr.Value))
                {
                    return false;
                }
            }

            return true;
        }

        private static readonly string[] _attributesForGetEntityProperty = new[] { "Attribute", "EntityName" };

        private static bool IsGetEntityProperty(XElement element)
        {
            if (!string.Equals(element.Name.LocalName, "GetEntityProperty", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            foreach (var attrName in _attributesForGetEntityProperty)
            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, attrName, StringComparison.InvariantCultureIgnoreCase));

                if (attr == null || string.IsNullOrEmpty(attr.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public WorkflowAnalysis GetWorkflowAnalysis(XElement doc)
        {
            WorkflowAnalysis result = new WorkflowAnalysis();

            result.CreateUpdateEntitySteps.AddRange(GetWorkflowEntityOperationsSteps(doc));

            result.UsedEntityAttributes.AddRange(GetWorkflowUsedAttributes(doc));

            return result;
        }

        private List<WorkflowCreateUpdateEntityStep> GetWorkflowEntityOperationsSteps(XElement doc)
        {
            List<WorkflowCreateUpdateEntityStep> result = new List<WorkflowCreateUpdateEntityStep>();

            var stepsElements = doc.Descendants().Where(IsCreateOrUpdateEntity);

            foreach (var stepElement in stepsElements)
            {
                var newStep = new WorkflowCreateUpdateEntityStep()
                {
                    DisplayName = stepElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "DisplayName", StringComparison.InvariantCultureIgnoreCase))?.Value,
                    EntityName = stepElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "EntityName", StringComparison.InvariantCultureIgnoreCase))?.Value,
                };

                string entityVarible = stepElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Entity", StringComparison.InvariantCultureIgnoreCase))?.Value;

                if (string.Equals(stepElement.Name.LocalName, "UpdateEntity", StringComparison.InvariantCultureIgnoreCase))
                {
                    newStep.StepType = WorkflowCreateUpdateEntityStepType.Update;

                    if (!string.IsNullOrEmpty(entityVarible)
                        && entityVarible.IndexOf("[CreatedEntities(\"CreateStep", StringComparison.InvariantCultureIgnoreCase) > -1
                        )
                    {
                        newStep.StepType = WorkflowCreateUpdateEntityStepType.UpdateCreated;
                    }
                }

                if (!string.IsNullOrEmpty(entityVarible))
                {
                    var parentElement = stepElement.Parent;

                    var setPropertiesList = parentElement.Descendants().Where(e => IsSetEntityProperty(e, entityVarible));

                    foreach (var setElement in setPropertiesList)
                    {
                        var newSetAttribute = new WorkflowSetEntityPropertyStep()
                        {
                            EntityName = setElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "EntityName", StringComparison.InvariantCultureIgnoreCase))?.Value,
                            Attribute = setElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Attribute", StringComparison.InvariantCultureIgnoreCase))?.Value,
                            //Value = string.Empty,
                            //StepType = WorkflowSetEntityPropertyStepType.Nullify,
                        };

                        //var attributeValue = setElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Value", StringComparison.InvariantCultureIgnoreCase))?.Value;

                        //Tuple<WorkflowSetEntityPropertyStepType, string> findValue = FindValueInElements(setElement.ElementsBeforeSelf(), attributeValue);

                        //if (findValue != null)
                        //{
                        //    newSetAttribute.StepType = findValue.Item1;
                        //    newSetAttribute.Value = findValue.Item2;
                        //}

                        newStep.SetEntityPropertySteps.Add(newSetAttribute);
                    }
                }

                result.Add(newStep);
            }

            return result;
        }

        private List<WorkflowGetEntityPropertyStep> GetWorkflowUsedAttributes(XElement doc)
        {
            List<WorkflowGetEntityPropertyStep> result = new List<WorkflowGetEntityPropertyStep>();

            var getPropertiesList = doc.Descendants().Where(IsGetEntityProperty);

            foreach (var getElement in getPropertiesList)
            {
                var newSetAttribute = new WorkflowGetEntityPropertyStep()
                {
                    EntityName = getElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "EntityName", StringComparison.InvariantCultureIgnoreCase))?.Value,
                    Attribute = getElement.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "Attribute", StringComparison.InvariantCultureIgnoreCase))?.Value,
                    DisplayName = getElement
                        .AncestorsAndSelf()
                        .FirstOrDefault(e => e.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "DisplayName", StringComparison.InvariantCultureIgnoreCase)) != null)
                        .Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "DisplayName", StringComparison.InvariantCultureIgnoreCase))
                        ?.Value,
                };

                if (!string.Equals(newSetAttribute.Attribute, "!Process_Custom_Attribute_URL_", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(newSetAttribute);
                }
            }

            return result;
        }

        //private Tuple<WorkflowSetEntityPropertyStepType, string> FindValueInElements(IEnumerable<XElement> enumerable, string attributeValue)
        //{
        //    return null;
        //}

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