using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.ProxyClassGeneration
{
    internal static class StaticNamingService
    {
        private static Dictionary<string, string> _attributeNames;

        public static string GetNameForEntity(EntityMetadata entityMetadata)
        {
            switch (entityMetadata.LogicalName.ToLower())
            {
                case "activitymimeattachment":
                    return "ActivityMimeAttachment";
                case "monthlyfiscalcalendar":
                    return "MonthlyFiscalCalendar";
                case "fixedmonthlyfiscalcalendar":
                    return "FixedMonthlyFiscalCalendar";
                case "quarterlyfiscalcalendar":
                    return "QuarterlyFiscalCalendar";
                case "semiannualfiscalcalendar":
                    return "SemiAnnualFiscalCalendar";
                case "annualfiscalcalendar":
                    return "AnnualFiscalCalendar";
                default:
                    return string.Empty;
            }
        }

        public static string GetNameForAttribute(AttributeMetadata attributeMetadata)
        {
            if (!StaticNamingService._attributeNames.ContainsKey(attributeMetadata.LogicalName))
            {
                return null;
            }

            return StaticNamingService._attributeNames[attributeMetadata.LogicalName];
        }

        static StaticNamingService()
        {
            StaticNamingService.InitializeAtributeNames();
        }

        private static void InitializeAtributeNames()
        {
            if (StaticNamingService._attributeNames != null)
            {
                return;
            }

            StaticNamingService._attributeNames = new Dictionary<string, string>()
            {
                {
                    "month1",
                    "Month1"
                },
                {
                    "month1_base",
                    "Month1_Base"
                },
                {
                    "month2",
                    "Month2"
                },
                {
                    "month2_base",
                    "Month2_Base"
                },
                {
                    "month3",
                    "Month3"
                },
                {
                    "month3_base",
                    "Month3_Base"
                },
                {
                    "month4",
                    "Month4"
                },
                {
                    "month4_base",
                    "Month4_Base"
                },
                {
                    "month5",
                    "Month5"
                },
                {
                    "month5_base",
                    "Month5_Base"
                },
                {
                    "month6",
                    "Month6"
                },
                {
                    "month6_base",
                    "Month6_Base"
                },
                {
                    "month7",
                    "Month7"
                },
                {
                    "month7_base",
                    "Month7_Base"
                },
                {
                    "month8",
                    "Month8"
                },
                {
                    "month8_base",
                    "Month8_Base"
                },
                {
                    "month9",
                    "Month9"
                },
                {
                    "month9_base",
                    "Month9_Base"
                },
                {
                    "month10",
                    "Month10"
                },
                {
                    "month10_base",
                    "Month10_Base"
                },
                {
                    "month11",
                    "Month11"
                },
                {
                    "month11_base",
                    "Month11_Base"
                },
                {
                    "month12",
                    "Month12"
                },
                {
                    "month12_base",
                    "Month12_Base"
                },
                {
                    "quarter1",
                    "Quarter1"
                },
                {
                    "quarter1_base",
                    "Quarter1_Base"
                },
                {
                    "quarter2",
                    "Quarter2"
                },
                {
                    "quarter2_base",
                    "Quarter2_Base"
                },
                {
                    "quarter3",
                    "Quarter3"
                },
                {
                    "quarter3_base",
                    "Quarter3_Base"
                },
                {
                    "quarter4",
                    "Quarter4"
                },
                {
                    "quarter4_base",
                    "Quarter4_Base"
                },
                {
                    "firsthalf",
                    "FirstHalf"
                },
                {
                    "firsthalf_base",
                    "FirstHalf_Base"
                },
                {
                    "secondhalf",
                    "SecondHalf"
                },
                {
                    "secondhalf_base",
                    "SecondHalf_Base"
                },
                {
                    "annual",
                    "Annual"
                },
                {
                    "annual_base",
                    "Annual_Base"
                },
                {
                    "requiredattendees",
                    "RequiredAttendees"
                },
                {
                    "from",
                    "From"
                },
                {
                    "to",
                    "To"
                },
                {
                    "cc",
                    "Cc"
                },
                {
                    "bcc",
                    "Bcc"
                }
            };
        }
    }
}
