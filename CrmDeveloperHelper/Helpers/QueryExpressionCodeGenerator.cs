using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class QueryExpressionCodeGenerator : CreateFileHandler
    {
        public QueryExpressionCodeGenerator(TextWriter writer)
            : base(writer, _defaultTabSpacer, true)
        {

        }

        public void WriteCSharpQueryExpression(QueryExpression query)
        {
            WriteLine("var query = new QueryExpression()");

            WriteLine("{");

            bool isFirstLineWrited = false;

            if (query.NoLock)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine("NoLock = true,");
            }

            if (query.Distinct)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine("Distinct = true,");
            }

            if (query.TopCount.HasValue)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine("TopCount = " + query.TopCount.ToString() + ",");
            }

            WriteLineIfHasLine(ref isFirstLineWrited);
            WriteLine("EntityName = \"" + query.EntityName + "\",");

            var columnSetDefinition = GetColumnSetDefinition(query.ColumnSet);

            WriteLine();
            WriteLine("ColumnSet = " + columnSetDefinition + ",");

            if (IsFilterExpressionNotEmpty(query.Criteria))
            {
                WriteLine();

                WriteLine("Criteria =");

                WriteLine("{");

                WriteFilterExpressionContent(query.Criteria);

                WriteLine("},");
            }

            if (query.LinkEntities.Any())
            {
                WriteLine();

                WriteLine("LinkEntities =");

                WriteLine("{");

                bool isFirstLinkWrited = false;

                foreach (var link in query.LinkEntities)
                {
                    WriteLineIfHasLine(ref isFirstLinkWrited);
                    WriteLinkEntity(link);
                }

                WriteLine("},");
            }

            if (query.Orders.Any())
            {
                WriteLine();

                WriteLine("Orders =");

                WriteLine("{");

                foreach (var order in query.Orders)
                {
                    WriteLine("new OrderExpression(\"" + order.AttributeName + "\", OrderType." + order.OrderType.ToString() + "),");
                }

                WriteLine("},");
            }

            WriteLine("};");
        }

        private bool IsColumnSetNotDefault(ColumnSet columnSet)
        {
            if (columnSet != null)
            {
                if (columnSet.AllColumns || columnSet.Columns.Any())
                {
                    return true;
                }
            }

            return false;
        }

        private string GetColumnSetDefinition(ColumnSet columnSet)
        {
            if (columnSet != null)
            {
                if (columnSet.AllColumns)
                {
                    return "new ColumnSet(true)";
                }
                else if (columnSet.Columns.Any())
                {
                    var cols = "\"" + string.Join("\", \"", columnSet.Columns.OrderBy(s => s)) + "\"";

                    return $"new ColumnSet({cols})";
                }
            }

            return "new ColumnSet(false)";
        }

        private void WriteLinkEntity(LinkEntity link)
        {
            bool isFirstLineWrited = false;

            WriteLine("new LinkEntity()");
            WriteLine("{");

            if (link.JoinOperator != JoinOperator.Inner)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine($"JoinOperator = JoinOperator.{link.JoinOperator},");
            }

            WriteLineIfHasLine(ref isFirstLineWrited);

            WriteLine($"LinkFromEntityName = \"{link.LinkFromEntityName}\",");
            WriteLine($"LinkFromAttributeName = \"{link.LinkFromAttributeName}\",");

            WriteLine();

            WriteLine($"LinkToEntityName = \"{link.LinkToEntityName}\",");
            WriteLine($"LinkToAttributeName = \"{link.LinkToAttributeName}\",");

            if (!string.IsNullOrWhiteSpace(link.EntityAlias))
            {
                WriteLine();
                WriteLine("EntityAlias = \"" + link.EntityAlias + "\",");
            }

            if (IsColumnSetNotDefault(link.Columns))
            {
                var columnDefinition = GetColumnSetDefinition(link.Columns);

                WriteLine();
                WriteLine("Columns = " + columnDefinition + ",");
            }

            if (IsFilterExpressionNotEmpty(link.LinkCriteria))
            {
                WriteLine();

                WriteLine("LinkCriteria =");

                WriteLine("{");

                WriteFilterExpressionContent(link.LinkCriteria);

                WriteLine("},");
            }

            if (link.LinkEntities.Any())
            {
                WriteLine();

                WriteLine("LinkEntities =");

                WriteLine("{");

                bool isFirstLinkWrited = false;

                foreach (var sublink in link.LinkEntities)
                {
                    WriteLineIfHasLine(ref isFirstLinkWrited);
                    WriteLinkEntity(sublink);
                }

                WriteLine("},");
            }

            WriteLine("},");
        }

        private bool IsFilterExpressionNotEmpty(FilterExpression filterExpression)
        {
            if (filterExpression != null && filterExpression.Conditions.Count > 0 || filterExpression.Filters.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void WriteFilterExpressionContent(FilterExpression filterExpression)
        {
            if (filterExpression == null)
            {
                return;
            }

            bool isFirstLineWrited = false;

            if (filterExpression.FilterOperator == LogicalOperator.Or)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine("FilterOperator = LogicalOperator.Or,");
            }

            if (filterExpression.Conditions.Any())
            {
                WriteLineIfHasLine(ref isFirstLineWrited);

                WriteLine("Conditions =");
                WriteLine("{");

                foreach (var cond in filterExpression.Conditions)
                {
                    var entity = string.Empty;
                    var values = string.Empty;

                    if (!string.IsNullOrWhiteSpace(cond.EntityName))
                    {
                        entity = "\"" + cond.EntityName + "\", ";
                    }

                    if (cond.Values.Count > 0)
                    {
                        values = ", " + GetConditionValues(cond.Values);
                    }

                    WriteLine($"new ConditionExpression({entity}\"{cond.AttributeName}\", ConditionOperator.{cond.Operator.ToString()}{values}),");
                }

                WriteLine("},");
            }

            if (filterExpression.Filters.Any())
            {
                WriteLineIfHasLine(ref isFirstLineWrited);

                WriteLine("Filters =");
                WriteLine("{");

                bool isFirstFilterWrited = false;

                foreach (var subfilter in filterExpression.Filters)
                {
                    if (IsFilterExpressionNotEmpty(subfilter))
                    {
                        WriteLineIfHasLine(ref isFirstFilterWrited);

                        WriteLine("new FilterExpression()");
                        WriteLine("{");

                        WriteFilterExpressionContent(subfilter);

                        WriteLine("},");
                    }
                }

                WriteLine("},");
            }
        }

        private string GetConditionValues(DataCollection<object> values)
        {
            var strings = new List<string>();

            foreach (var value in values)
            {
                if (value is string || value is Guid)
                {
                    strings.Add("\"" + value.ToString() + "\"");
                }
                else if (value is bool)
                {
                    strings.Add((bool)value ? "true" : "false");
                }
                else
                {
                    strings.Add(value.ToString());
                }
            }

            return string.Join(", ", strings);
        }
    }
}
