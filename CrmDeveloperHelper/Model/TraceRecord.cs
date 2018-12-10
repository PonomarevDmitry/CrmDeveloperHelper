using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
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

        public Guid RequestId { get; set; }

        public string Process { get; set; }

        public string Thread { get; set; }

        public string Category { get; set; }

        public string Level { get; set; }

        public Guid? UserId { get; set; }

        public SystemUser User { get; set; }

        public Guid? Organization { get; set; }

        public string Description { get; set; }

        public TraceFile TraceFile { get; set; }

        public static Task<List<TraceRecord>> ParseFilesAsync(IEnumerable<string> files)
        {
            return Task.Run(() => ParseFiles(files));
        }

        private static List<TraceRecord> ParseFiles(IEnumerable<string> files)
        {
            List<TraceRecord> traces = new List<TraceRecord>();

            foreach (var filePath in files)
            {
                var tempFile = FileOperations.GetNewTempFile(Path.GetFileNameWithoutExtension(filePath), Path.GetExtension(filePath));

                File.Copy(filePath, tempFile);

                var traceFile = new TraceFile()
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                };

                int index = 0;

                var fileStrings = File.ReadAllLines(tempFile);

                {
                    string line = fileStrings.FirstOrDefault(l => !string.IsNullOrEmpty(l) && l.StartsWith("#"));

                    if (!string.IsNullOrEmpty(line))
                    {
                        traceFile.Name = line.Trim(' ', '#');
                    }
                }

                TraceRecord traceRecord = null;
                StringBuilder description = null;

                foreach (var line in fileStrings)
                {
                    if (!string.IsNullOrEmpty(line) && line.Trim().StartsWith("#"))
                    {
                        var parts = line.Trim(' ', '#', ' ').Split(':');

                        var value = string.Join(":", parts.Skip(1)).Trim();

                        switch (parts[0].Trim())
                        {
                            case "LocalTime":
                                if (DateTime.TryParse(value, out var temp))
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
                    else
                    {
                        int indexLast;

                        if (line.StartsWith("[") && (indexLast = line.IndexOf("]")) > 0)
                        {
                            var datePart = line.Substring(1, indexLast - 2);

                            if (DateTime.TryParse(datePart, out var tempDate))
                            {
                                if (traceRecord != null)
                                {
                                    if (description != null)
                                    {
                                        traceRecord.Description = description.ToString();
                                    }

                                    traces.Add(traceRecord);
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
                                            {
                                                if (Guid.TryParse(value, out var tempGuid))
                                                {
                                                    traceRecord.Organization = tempGuid;
                                                }
                                            }
                                            break;

                                        case "Thread":
                                            traceRecord.Thread = value;
                                            break;

                                        case "Category":
                                            traceRecord.Category = value;
                                            break;

                                        case "User":
                                            {
                                                if (Guid.TryParse(value, out var tempGuid))
                                                {
                                                    traceRecord.UserId = tempGuid;
                                                }
                                            }
                                            break;

                                        case "Level":
                                            traceRecord.Level = value;
                                            break;

                                        case "ReqId":
                                            {
                                                if (Guid.TryParse(value, out var tempGuid))
                                                {
                                                    traceRecord.RequestId = tempGuid;
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
                                    traceRecord.RequestId = Guid.Empty;
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
            }

            return traces;
        }
    }
}