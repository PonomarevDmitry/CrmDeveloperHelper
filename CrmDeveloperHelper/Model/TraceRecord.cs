using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class TraceRecord
    {
        public DateTime Date { get; set; }

        public int RecordNumber { get; set; }

        public string Operation { get; set; }

        public Guid? RequestId { get; set; }

        public string Process { get; set; }

        public int Thread { get; set; }

        public string Category { get; set; }

        public TraceRecordLevel Level { get; set; }

        public Guid? UserId { get; set; }

        public SystemUser User { get; set; }

        public Guid? Organization { get; set; }

        public string Description { get; set; }

        public TraceFile TraceFile { get; set; }

        public static async Task<List<TraceRecord>> ParseFilesAsync(IEnumerable<string> files)
        {
            var tasks = new List<Task<List<TraceRecord>>>();

            foreach (var filePath in files)
            {
                tasks.Add(ParseTraceFileAsync(filePath));
            }

            await Task.WhenAll(tasks);

            List<TraceRecord> traces = new List<TraceRecord>();

            foreach (var task in tasks)
            {
                traces.AddRange(await task);
            }

            return traces;
        }

        private static Task<List<TraceRecord>> ParseTraceFileAsync(string filePath)
        {
            return Task.Run(() => ParseTraceFile(filePath));
        }

        private static List<TraceRecord> ParseTraceFile(string filePath)
        {
            List<TraceRecord> result = new List<TraceRecord>();

            var tempFilePath = FileOperations.GetNewTempFilePath(Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath));

            File.Copy(filePath, tempFilePath);

            var traceFile = new TraceFile()
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
            };

            using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream))
            {
                string line;

                while ((line = reader.ReadLine()) != null && string.IsNullOrEmpty(line))
                {

                }

                if (!string.IsNullOrEmpty(line))
                {
                    traceFile.Name = line.Trim(' ', '#');
                }

                while ((line = reader.ReadLine()) != null && line.Trim().StartsWith("#"))
                {
                    var parts = line.Trim(' ', '#', ' ').Split(':');

                    var value = string.Join(":", parts.Skip(1)).Trim();

                    switch (parts[0].Trim())
                    {
                        case "LocalTime":
                            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var temp))
                            {
                                traceFile.LocalTime = temp;
                            }
                            break;

                        case "Categories":
                            traceFile.Categories = value;
                            break;

                        case "CallStackOn":
                            traceFile.CallStackOn = value;
                            break;

                        case "ComputerName":
                            traceFile.ComputerName = value;
                            break;

                        case "CRMVersion":
                            traceFile.CRMVersion = value;
                            break;

                        case "DeploymentType":
                            traceFile.DeploymentType = value;
                            break;

                        case "ScaleGroup":
                            traceFile.ScaleGroup = value;
                            break;

                        case "ServerRole":
                            traceFile.ServerRole = value;
                            break;
                    }
                }

                TraceRecord traceRecord = null;
                StringBuilder description = null;

                int index = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    int indexLast;

                    if (line.StartsWith("[") && (indexLast = line.IndexOf("]")) > 0)
                    {
                        var datePart = line.Substring(1, indexLast - 1);

                        if (DateTime.TryParseExact(datePart, "yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var tempDate))
                        {
                            if (traceRecord != null)
                            {
                                if (description != null)
                                {
                                    traceRecord.Description = description.ToString();
                                }

                                result.Add(traceRecord);
                            }

                            traceRecord = new TraceRecord()
                            {
                                TraceFile = traceFile,
                                RecordNumber = ++index,
                            };
                            description = new StringBuilder();

                            traceRecord.Date = tempDate;

                            var lastPart = line.Substring(indexLast + 1);

                            var lineParts = lastPart.Split('|');

                            var processName = lineParts.FirstOrDefault();

                            for (int i = 0; i < lineParts.Length; i++)
                            {
                                var parts = lineParts[i].Split(':');

                                var value = string.Join(":", parts.Skip(1)).Trim();

                                switch (parts[0].Trim())
                                {
                                    case "Process":
                                        traceRecord.Process = value;
                                        break;

                                    case "Organization":
                                        if (Guid.TryParse(value, out var organizationGuid)
                                            && organizationGuid != Guid.Empty
                                            )
                                        {
                                            traceRecord.Organization = organizationGuid;
                                        }
                                        break;

                                    case "Thread":
                                        if (int.TryParse(value, out var threadNumber))
                                        {
                                            traceRecord.Thread = threadNumber;
                                        }
                                        break;

                                    case "Category":
                                        traceRecord.Category = value;
                                        break;

                                    case "User":
                                        if (Guid.TryParse(value, out var userGuid)
                                            && userGuid != Guid.Empty
                                            )
                                        {
                                            traceRecord.UserId = userGuid;
                                        }
                                        break;

                                    case "Level":
                                        traceRecord.Level = ParseTraceRecordLevel(value);
                                        break;

                                    case "ReqId":
                                        {
                                            if (Guid.TryParse(value, out var requestGuid)
                                                && requestGuid != Guid.Empty
                                                )
                                            {
                                                traceRecord.RequestId = requestGuid;
                                            }
                                        }
                                        break;

                                    default:
                                        traceRecord.Operation = parts[0].Trim();
                                        break;
                                }
                            }

                            if (string.Equals(traceRecord.Process, "OUTLOOK", StringComparison.InvariantCultureIgnoreCase))
                            {
                                traceRecord.Organization = null;
                                traceRecord.RequestId = null;
                                traceRecord.Operation = string.Empty;
                            }
                        }
                    }

                    if (description != null)
                    {
                        description.AppendLine(line);
                    }
                }
            }

            return result;
        }

        private static TraceRecordLevel ParseTraceRecordLevel(string value)
        {
            if (string.Equals(value, "Off", StringComparison.InvariantCultureIgnoreCase))
            {
                return TraceRecordLevel.Off;
            }
            else if (string.Equals(value, "Error", StringComparison.InvariantCultureIgnoreCase))
            {
                return TraceRecordLevel.Error;
            }
            else if (string.Equals(value, "Warning", StringComparison.InvariantCultureIgnoreCase))
            {
                return TraceRecordLevel.Warning;
            }
            else if (string.Equals(value, "Info", StringComparison.InvariantCultureIgnoreCase))
            {
                return TraceRecordLevel.Info;
            }
            else if (string.Equals(value, "Verbose", StringComparison.InvariantCultureIgnoreCase))
            {
                return TraceRecordLevel.Verbose;
            }

            return TraceRecordLevel.Unknown;
        }
    }
}