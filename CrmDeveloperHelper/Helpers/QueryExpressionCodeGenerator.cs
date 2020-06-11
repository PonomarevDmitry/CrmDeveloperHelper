using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class QueryExpressionCodeGenerator : CreateFileHandler
    {
        public QueryExpressionCodeGenerator(TextWriter writer)
            : base(writer, _defaultTabSpacer, true)
        {

        }

        public void WriteCSharpQueryExpression(FetchType fetchXml)
        {
            WriteLine($"var query = new {nameof(QueryExpression)}()");

            WriteLine("{");

            var fetchEntity = fetchXml.Items.OfType<FetchEntityType>().FirstOrDefault();

            if (fetchEntity != null)
            {
                bool isFirstLineWrited = false;

                if (fetchXml.nolock)
                {
                    WriteLineIfHasLine(ref isFirstLineWrited);
                    WriteLine($"{nameof(QueryExpression.NoLock)} = true,");
                }

                if (fetchXml.distinct && fetchXml.distinctSpecified)
                {
                    WriteLineIfHasLine(ref isFirstLineWrited);
                    WriteLine($"{nameof(QueryExpression.Distinct)} = true,");
                }

                if (!string.IsNullOrEmpty(fetchXml.top) && int.TryParse(fetchXml.top, out int top))
                {
                    WriteLineIfHasLine(ref isFirstLineWrited);
                    WriteLine($"{nameof(QueryExpression.TopCount)} = {top},");
                }
                else if (!string.IsNullOrEmpty(fetchXml.count) && int.TryParse(fetchXml.count, out int count))
                {
                    WriteLineIfHasLine(ref isFirstLineWrited);
                    WriteLine($"{nameof(QueryExpression.TopCount)} = {count},");
                }

                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine($"{nameof(QueryExpression.EntityName)} = \"{fetchEntity.name}\",");

                var columnSetDefinition = GetColumnSetDefinition(fetchEntity.Items);

                WriteLine();
                WriteLine($"{nameof(QueryExpression.ColumnSet)} = {columnSetDefinition},");

                var criteria = fetchEntity.Items.OfType<filter>().Where(IsFilterExpressionNotEmpty).ToList();

                if (criteria.Any())
                {
                    filter filter;

                    if (criteria.Count == 1)
                    {
                        filter = criteria[0];
                    }
                    else
                    {
                        filter = new filter()
                        {
                            Items = criteria.ToArray(),
                        };
                    }

                    WriteLine();

                    WriteLine($"{nameof(QueryExpression.Criteria)} =");

                    WriteLine("{");

                    WriteFilterExpressionContent(filter);

                    WriteLine("},");
                }

                if (fetchEntity.Items.OfType<FetchLinkEntityType>().Any())
                {
                    WriteLine();

                    WriteLine($"{nameof(QueryExpression.LinkEntities)} =");

                    WriteLine("{");

                    bool isFirstLinkWrited = false;

                    foreach (var link in fetchEntity.Items.OfType<FetchLinkEntityType>())
                    {
                        WriteLineIfHasLine(ref isFirstLinkWrited);
                        WriteLinkEntity(fetchEntity.name, link);
                    }

                    WriteLine("},");
                }

                if (fetchEntity.Items.OfType<FetchOrderType>().Any())
                {
                    WriteLine();

                    WriteLine($"{nameof(QueryExpression.Orders)} =");

                    WriteLine("{");

                    foreach (var order in fetchEntity.Items.OfType<FetchOrderType>())
                    {
                        var orderType = order.descending ? OrderType.Descending : OrderType.Ascending;

                        WriteLine($"new {nameof(OrderExpression)}(\"{order.attribute}\", {nameof(OrderType)}.{orderType}),");
                    }

                    WriteLine("},");
                }
            }

            Write("};");
        }

        private static bool IsColumnSetNotDefault(object[] items)
        {
            if (items != null)
            {
                if (items.OfType<allattributes>().Any() || items.OfType<FetchAttributeType>().Any())
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetColumnSetDefinition(object[] items)
        {
            if (items != null)
            {
                if (items.OfType<allattributes>().Any())
                {
                    return $"new {nameof(ColumnSet)}(true)";
                }
                else if (items.OfType<FetchAttributeType>().Any())
                {
                    var cols = "\"" + string.Join("\", \"", items.OfType<FetchAttributeType>().Select(a => a.name).OrderBy(s => s)) + "\"";

                    return $"new {nameof(ColumnSet)}({cols})";
                }
            }

            return $"new {nameof(ColumnSet)}(false)";
        }

        private void WriteLinkEntity(string parentEntityName, FetchLinkEntityType link)
        {
            bool isFirstLineWrited = false;

            WriteLine($"new {nameof(LinkEntity)}()");
            WriteLine("{");

            if (string.Equals(link.linktype, "outer", StringComparison.InvariantCulture))
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine($"{nameof(LinkEntity.JoinOperator)} = {nameof(JoinOperator)}.{nameof(JoinOperator.LeftOuter)},");
            }

            WriteLineIfHasLine(ref isFirstLineWrited);

            WriteLine($"{nameof(LinkEntity.LinkFromEntityName)} = \"{parentEntityName}\",");
            WriteLine($"{nameof(LinkEntity.LinkFromAttributeName)} = \"{link.to}\",");

            WriteLine();

            WriteLine($"{nameof(LinkEntity.LinkToEntityName)} = \"{link.name}\",");
            WriteLine($"{nameof(LinkEntity.LinkToAttributeName)} = \"{link.from}\",");

            if (!string.IsNullOrEmpty(link.alias) && !string.IsNullOrWhiteSpace(link.alias))
            {
                WriteLine();
                WriteLine($"{nameof(LinkEntity.EntityAlias)} = \"{link.alias}\",");
            }

            if (IsColumnSetNotDefault(link.Items))
            {
                var columnDefinition = GetColumnSetDefinition(link.Items);

                WriteLine();
                WriteLine($"{nameof(LinkEntity.Columns)} = {columnDefinition},");
            }

            var criteria = link.Items.OfType<filter>().Where(IsFilterExpressionNotEmpty).ToList();

            if (criteria.Any())
            {
                filter filter;

                if (criteria.Count == 1)
                {
                    filter = criteria[0];
                }
                else
                {
                    filter = new filter()
                    {
                        Items = criteria.ToArray(),
                    };
                }

                WriteLine();

                WriteLine($"{nameof(LinkEntity.LinkCriteria)} =");

                WriteLine("{");

                WriteFilterExpressionContent(filter);

                WriteLine("},");
            }

            if (link.Items.OfType<FetchLinkEntityType>().Any())
            {
                WriteLine();

                WriteLine($"{nameof(LinkEntity.LinkEntities)} =");

                WriteLine("{");

                bool isFirstLinkWrited = false;

                foreach (var sublink in link.Items.OfType<FetchLinkEntityType>())
                {
                    WriteLineIfHasLine(ref isFirstLinkWrited);
                    WriteLinkEntity(link.name, sublink);
                }

                WriteLine("},");
            }

            WriteLine("},");
        }

        private static bool IsFilterExpressionNotEmpty(filter filterExpression)
        {
            if (filterExpression != null)
            {
                if (filterExpression.Items.OfType<condition>().Any()
                    || filterExpression.Items.OfType<filter>().Any()
                )
                {
                    return true;
                }
            }

            return false;
        }

        private void WriteFilterExpressionContent(filter filterExpression)
        {
            if (filterExpression == null)
            {
                return;
            }

            bool isFirstLineWrited = false;

            if (filterExpression.type == filterType.or)
            {
                WriteLineIfHasLine(ref isFirstLineWrited);
                WriteLine($"{nameof(FilterExpression.FilterOperator)} = {nameof(LogicalOperator)}.{nameof(LogicalOperator.Or)},");
            }

            if (filterExpression.Items.OfType<condition>().Any())
            {
                WriteLineIfHasLine(ref isFirstLineWrited);

                WriteLine($"{nameof(FilterExpression.Conditions)} =");
                WriteLine("{");

                foreach (var cond in filterExpression.Items.OfType<condition>())
                {
                    var entityName = string.Empty;
                    var valuesString = string.Empty;

                    if (!string.IsNullOrEmpty(cond.entityname) && !string.IsNullOrWhiteSpace(cond.entityname))
                    {
                        entityName = "\"" + cond.entityname + "\", ";
                    }

                    List<object> values = GetConditionValues(cond);

                    if (values.Count > 0)
                    {
                        valuesString = ", " + GetConditionValues(values);
                    }

                    var conditionOperator = ConvertToConditionOperator(cond.@operator);

                    WriteLine($"new {nameof(ConditionExpression)}({entityName}\"{cond.attribute}\", {nameof(ConditionOperator)}.{conditionOperator.ToString()}{valuesString}),");
                }

                WriteLine("},");
            }

            if (filterExpression.Items.OfType<filter>().Any())
            {
                WriteLineIfHasLine(ref isFirstLineWrited);

                WriteLine($"{nameof(FilterExpression.Filters)} =");
                WriteLine("{");

                bool isFirstFilterWrited = false;

                foreach (var subfilter in filterExpression.Items.OfType<filter>())
                {
                    if (IsFilterExpressionNotEmpty(subfilter))
                    {
                        WriteLineIfHasLine(ref isFirstFilterWrited);

                        WriteLine($"new {nameof(FilterExpression)}()");
                        WriteLine("{");

                        WriteFilterExpressionContent(subfilter);

                        WriteLine("},");
                    }
                }

                WriteLine("},");
            }
        }

        private static ConditionOperator ConvertToConditionOperator(@operator @operator)
        {
            switch (@operator)
            {
                case @operator.eq:
                    return ConditionOperator.Equal;

                case @operator.neq:
                case @operator.ne:
                    return ConditionOperator.NotEqual;

                case @operator.gt:
                    return ConditionOperator.GreaterThan;

                case @operator.ge:
                    return ConditionOperator.GreaterEqual;

                case @operator.le:
                    return ConditionOperator.LessEqual;

                case @operator.lt:
                    return ConditionOperator.LessThan;

                case @operator.like:
                    return ConditionOperator.Like;

                case @operator.notlike:
                    return ConditionOperator.NotLike;

                case @operator.@in:
                    return ConditionOperator.In;

                case @operator.notin:
                    return ConditionOperator.NotIn;

                case @operator.between:
                    return ConditionOperator.Between;

                case @operator.notbetween:
                    return ConditionOperator.NotBetween;

                case @operator.@null:
                    return ConditionOperator.Null;

                case @operator.notnull:
                    return ConditionOperator.NotNull;

                case @operator.yesterday:
                    return ConditionOperator.Yesterday;

                case @operator.today:
                    return ConditionOperator.Today;

                case @operator.tomorrow:
                    return ConditionOperator.Tomorrow;

                case @operator.lastsevendays:
                    return ConditionOperator.Last7Days;

                case @operator.nextsevendays:
                    return ConditionOperator.Next7Days;

                case @operator.lastweek:
                    return ConditionOperator.LastWeek;

                case @operator.thisweek:
                    return ConditionOperator.ThisWeek;

                case @operator.nextweek:
                    return ConditionOperator.NextWeek;

                case @operator.lastmonth:
                    return ConditionOperator.LastMonth;

                case @operator.thismonth:
                    return ConditionOperator.ThisMonth;

                case @operator.nextmonth:
                    return ConditionOperator.NextMonth;

                case @operator.on:
                    return ConditionOperator.On;

                case @operator.onorbefore:
                    return ConditionOperator.OnOrBefore;

                case @operator.onorafter:
                    return ConditionOperator.OnOrAfter;

                case @operator.lastyear:
                    return ConditionOperator.LastYear;

                case @operator.thisyear:
                    return ConditionOperator.ThisYear;

                case @operator.nextyear:
                    return ConditionOperator.NextYear;

                case @operator.lastxhours:
                    return ConditionOperator.LastXHours;

                case @operator.nextxhours:
                    return ConditionOperator.NextXHours;

                case @operator.lastxdays:
                    return ConditionOperator.LastXDays;

                case @operator.nextxdays:
                    return ConditionOperator.NextXDays;

                case @operator.lastxweeks:
                    return ConditionOperator.LastXWeeks;

                case @operator.nextxweeks:
                    return ConditionOperator.NextXWeeks;

                case @operator.lastxmonths:
                    return ConditionOperator.LastXMonths;

                case @operator.nextxmonths:
                    return ConditionOperator.NextXMonths;

                case @operator.olderthanxmonths:
                    return ConditionOperator.OlderThanXMonths;

                case @operator.olderthanxyears:
                    return ConditionOperator.OlderThanXYears;

                case @operator.olderthanxweeks:
                    return ConditionOperator.OlderThanXWeeks;

                case @operator.olderthanxdays:
                    return ConditionOperator.OlderThanXDays;

                case @operator.olderthanxhours:
                    return ConditionOperator.OlderThanXHours;

                case @operator.olderthanxminutes:
                    return ConditionOperator.OlderThanXMinutes;

                case @operator.lastxyears:
                    return ConditionOperator.LastXYears;

                case @operator.nextxyears:
                    return ConditionOperator.NextXYears;

                case @operator.equserid:
                    return ConditionOperator.EqualUserId;

                case @operator.neuserid:
                    return ConditionOperator.NotEqualUserId;

                case @operator.equserteams:
                    return ConditionOperator.EqualUserTeams;

                case @operator.equseroruserteams:
                    return ConditionOperator.EqualUserOrUserTeams;

                case @operator.equseroruserhierarchy:
                    return ConditionOperator.EqualUserOrUserHierarchy;

                case @operator.equseroruserhierarchyandteams:
                    return ConditionOperator.EqualUserOrUserHierarchyAndTeams;

                case @operator.eqbusinessid:
                    return ConditionOperator.EqualBusinessId;

                case @operator.nebusinessid:
                    return ConditionOperator.NotEqualBusinessId;

                case @operator.equserlanguage:
                    return ConditionOperator.EqualUserLanguage;

                case @operator.thisfiscalyear:
                    return ConditionOperator.ThisFiscalYear;

                case @operator.thisfiscalperiod:
                    return ConditionOperator.ThisFiscalPeriod;

                case @operator.nextfiscalyear:
                    return ConditionOperator.NextFiscalYear;

                case @operator.nextfiscalperiod:
                    return ConditionOperator.NextFiscalPeriod;

                case @operator.lastfiscalyear:
                    return ConditionOperator.LastFiscalYear;

                case @operator.lastfiscalperiod:
                    return ConditionOperator.LastFiscalPeriod;

                case @operator.lastxfiscalyears:
                    return ConditionOperator.LastXFiscalYears;

                case @operator.lastxfiscalperiods:
                    return ConditionOperator.LastXFiscalPeriods;

                case @operator.nextxfiscalyears:
                    return ConditionOperator.NextXFiscalYears;

                case @operator.nextxfiscalperiods:
                    return ConditionOperator.NextXFiscalPeriods;

                case @operator.infiscalyear:
                    return ConditionOperator.InFiscalYear;

                case @operator.infiscalperiod:
                    return ConditionOperator.InFiscalPeriod;

                case @operator.infiscalperiodandyear:
                    return ConditionOperator.InFiscalPeriodAndYear;

                case @operator.inorbeforefiscalperiodandyear:
                    return ConditionOperator.InOrBeforeFiscalPeriodAndYear;

                case @operator.inorafterfiscalperiodandyear:
                    return ConditionOperator.InOrAfterFiscalPeriodAndYear;

                case @operator.beginswith:
                    return ConditionOperator.BeginsWith;

                case @operator.notbeginwith:
                    return ConditionOperator.DoesNotBeginWith;

                case @operator.endswith:
                    return ConditionOperator.EndsWith;

                case @operator.notendwith:
                    return ConditionOperator.DoesNotEndWith;

                case @operator.under:
                    return ConditionOperator.Under;

                case @operator.eqorunder:
                    return ConditionOperator.UnderOrEqual;

                case @operator.notunder:
                    return ConditionOperator.NotUnder;

                case @operator.above:
                    return ConditionOperator.Above;

                case @operator.eqorabove:
                    return ConditionOperator.AboveOrEqual;

                case @operator.containvalues:
                    return ConditionOperator.ContainValues;

                case @operator.notcontainvalues:
                    return ConditionOperator.DoesNotContainValues;

                default:
                    break;
            }

            return ConditionOperator.Equal;
        }

        private static List<object> GetConditionValues(condition cond)
        {
            List<object> result = new List<object>();

            if (cond.Items != null && cond.Items.Any())
            {
                foreach (var item in cond.Items)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        result.Add(item.Value);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(cond.value))
            {
                result.Add(cond.value);
            }

            return result;
        }

        private static string GetConditionValues(IEnumerable<object> values)
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
