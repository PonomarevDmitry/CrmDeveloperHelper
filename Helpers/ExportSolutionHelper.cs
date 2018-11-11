using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class ExportSolutionHelper
    {
        private IOrganizationServiceExtented _service;

        public ExportSolutionHelper(IOrganizationServiceExtented service)
        {
            this._service = service;
        }

        public Task<string> ExportAsync(ExportSolutionConfig config, ExportSolutionOverrideInformation solutionInfo)
        {
            return Task.Run(() => Export(config, solutionInfo));
        }

        private string Export(ExportSolutionConfig config, ExportSolutionOverrideInformation solutionInfo)
        {
            var solution = _service.Retrieve(Solution.EntityLogicalName, config.IdSolution, new ColumnSet(true)).ToEntity<Solution>();

            string solutionName = solution.UniqueName;
            string solutionVersion = solution.Version;

            solutionVersion = RemoveSymbols(solutionVersion);

            ExportSolutionRequest request = new ExportSolutionRequest()
            {
                SolutionName = solutionName,
                Managed = config.Managed,
            };

            if (config.ExportAutoNumberingSettings)
            {
                request.ExportAutoNumberingSettings = config.ExportAutoNumberingSettings;
            }

            if (config.ExportCalendarSettings)
            {
                request.ExportCalendarSettings = config.ExportCalendarSettings;
            }

            if (config.ExportCustomizationSettings)
            {
                request.ExportCustomizationSettings = config.ExportCustomizationSettings;
            }

            if (config.ExportEmailTrackingSettings)
            {
                request.ExportEmailTrackingSettings = config.ExportEmailTrackingSettings;
            }

            if (config.ExportExternalApplications)
            {
                request.ExportExternalApplications = config.ExportExternalApplications;
            }

            if (config.ExportGeneralSettings)
            {
                request.ExportGeneralSettings = config.ExportGeneralSettings;
            }

            if (config.ExportIsvConfig)
            {
                request.ExportIsvConfig = config.ExportIsvConfig;
            }

            if (config.ExportMarketingSettings)
            {
                request.ExportMarketingSettings = config.ExportMarketingSettings;
            }

            if (config.ExportOutlookSynchronizationSettings)
            {
                request.ExportOutlookSynchronizationSettings = config.ExportOutlookSynchronizationSettings;
            }

            if (config.ExportRelationshipRoles)
            {
                request.ExportRelationshipRoles = config.ExportRelationshipRoles;
            }

            if (config.ExportSales)
            {
                request.ExportSales = config.ExportSales;
            }

            var response = (ExportSolutionResponse)_service.Execute(request);

            var fileBody = response.ExportSolutionFile;

            if (!Directory.Exists(config.ExportFolder))
            {
                Directory.CreateDirectory(config.ExportFolder);
            }


            if (solutionInfo.OverrideNameAndVersion || solutionInfo.OverrideDescription)
            {
                fileBody = OverrideSolutionInformation(fileBody, solutionInfo);
            }

            string fileName = string.Format("{0}.{1}_{2}{3} at {4}.zip"
                , config.ConnectionName
                , solutionName
                , solutionVersion
                , (config.Managed ? "_managed" : string.Empty)
                , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                );

            string filePath = Path.Combine(config.ExportFolder, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllBytes(filePath, fileBody);

            return filePath;
        }

        private byte[] OverrideSolutionInformation(byte[] fileBody, ExportSolutionOverrideInformation solutionInfo)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(fileBody, 0, fileBody.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open, FileAccess.ReadWrite))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/solution.xml", UriKind.Relative));

                    if (part != null)
                    {
                        XDocument doc = null;

                        using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            doc = XDocument.Load(streamPart);
                        }

                        if (solutionInfo.OverrideNameAndVersion)
                        {
                            if (!string.IsNullOrEmpty(solutionInfo.UniqueName))
                            {
                                var uniqueNameNode = doc.XPathSelectElement("ImportExportXml/SolutionManifest/UniqueName");

                                if (uniqueNameNode != null)
                                {
                                    uniqueNameNode.Value = solutionInfo.UniqueName;
                                }
                            }

                            if (!string.IsNullOrEmpty(solutionInfo.DisplayName))
                            {
                                var labelNode = doc.XPathSelectElements("ImportExportXml/SolutionManifest/LocalizedNames/LocalizedName");

                                if (labelNode.Any())
                                {
                                    foreach (var node in labelNode)
                                    {
                                        node.SetAttributeValue("description", solutionInfo.DisplayName);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(solutionInfo.Version))
                            {
                                var versionNode = doc.XPathSelectElement("ImportExportXml/SolutionManifest/Version");

                                if (versionNode != null)
                                {
                                    versionNode.Value = solutionInfo.Version;
                                }
                            }
                        }

                        if (solutionInfo.OverrideDescription)
                        {
                            var labelNode = doc.XPathSelectElements("ImportExportXml/SolutionManifest/Descriptions/Description");

                            if (labelNode.Any())
                            {
                                foreach (var node in labelNode)
                                {
                                    node.SetAttributeValue("description", solutionInfo.Description);
                                }
                            }
                        }

                        using (Stream streamPart = part.GetStream(FileMode.Create, FileAccess.Write))
                        {
                            XmlWriterSettings settings = new XmlWriterSettings
                            {
                                OmitXmlDeclaration = true,
                                Indent = true,
                                Encoding = Encoding.UTF8
                            };

                            using (XmlWriter xmlWriter = XmlWriter.Create(streamPart, settings))
                            {
                                doc.Save(xmlWriter);
                            }
                        }
                    }
                }

                memStream.Position = 0;
                byte[] result = memStream.ToArray();

                return result;
            }
        }

        private string RemoveSymbols(string solutionVersion)
        {
            var result = new StringBuilder(solutionVersion);

            result = result.Replace(".", "_");
            result = result.Replace(",", "_");

            return result.ToString();
        }

        public Task<byte[]> ExportSolutionAndGetBodyBinaryAsync(string solutionUniqueName)
        {
            return Task.Run(() => ExportSolutionAndGetBodyBinary(solutionUniqueName));
        }

        private byte[] ExportSolutionAndGetBodyBinary(string solutionUniqueName)
        {
            ExportSolutionRequest request = new ExportSolutionRequest()
            {
                SolutionName = solutionUniqueName,
                Managed = false,
            };

            var response = (ExportSolutionResponse)_service.Execute(request);

            return response.ExportSolutionFile;
        }

        public Task<string> ExportSolutionAndGetRibbonDiffAsync(string solutionUniqueName, string entityName)
        {
            return Task.Run(() => ExportSolutionAndGetRibbonDiff(solutionUniqueName, entityName));
        }

        private string ExportSolutionAndGetRibbonDiff(string solutionUniqueName, string entityName)
        {
            var fileBody = this.ExportSolutionAndGetBodyBinary(solutionUniqueName);

            string result = GetRibbonDiffXmlForEntityFromSolutionBody(entityName, fileBody);

            return result;
        }

        public static string GetRibbonDiffXmlForEntityFromSolutionBody(string entityName, byte[] fileBody)
        {
            string result = string.Empty;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(fileBody, 0, fileBody.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open, FileAccess.ReadWrite))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/customizations.xml", UriKind.Relative));

                    if (part != null)
                    {
                        XDocument doc = null;

                        using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            doc = XDocument.Load(streamPart);
                        }

                        var nodes = doc.XPathSelectElements("ImportExportXml/Entities/Entity");

                        foreach (var item in nodes)
                        {
                            var elementName = item.Element("Name");

                            if (elementName != null && string.Equals(elementName.Value, entityName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var ribbonDiffXml = item.Element("RibbonDiffXml");

                                if (ribbonDiffXml != null)
                                {
                                    result = ribbonDiffXml.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public Task<string> ExportSolutionAndGetApplicationRibbonDiffAsync(string solutionUniqueName)
        {
            return Task.Run(() => ExportSolutionAndGetApplicationRibbonDiff(solutionUniqueName));
        }

        private string ExportSolutionAndGetApplicationRibbonDiff(string solutionUniqueName)
        {
            var fileBody = this.ExportSolutionAndGetBodyBinary(solutionUniqueName);

            string result = GetApplicationRibbonDiffXmlFromSolutionBody(fileBody);

            return result;
        }

        public static string GetApplicationRibbonDiffXmlFromSolutionBody(byte[] fileBody)
        {
            string result = string.Empty;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(fileBody, 0, fileBody.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open, FileAccess.ReadWrite))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/customizations.xml", UriKind.Relative));

                    if (part != null)
                    {
                        XDocument doc = null;

                        using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            doc = XDocument.Load(streamPart);
                        }

                        var ribbonDiffXml = doc.XPathSelectElement("ImportExportXml/RibbonDiffXml");

                        if (ribbonDiffXml != null)
                        {
                            result = ribbonDiffXml.ToString();
                        }
                    }
                }
            }

            return result;
        }

        public static byte[] ReplaceRibbonDiffXmlForEntityInSolutionBody(string entityLogicalName, byte[] solutionBodyBinary, XElement newRibbonDiffXml)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(solutionBodyBinary, 0, solutionBodyBinary.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open, FileAccess.ReadWrite))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/customizations.xml", UriKind.Relative));

                    if (part != null)
                    {
                        XDocument doc = null;

                        using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            doc = XDocument.Load(streamPart);
                        }

                        var nodes = doc.XPathSelectElements("ImportExportXml/Entities/Entity");

                        foreach (var item in nodes)
                        {
                            var elementName = item.Element("Name");

                            if (elementName != null && string.Equals(elementName.Value, entityLogicalName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var ribbonDiffXml = item.Element("RibbonDiffXml");

                                if (ribbonDiffXml != null)
                                {
                                    ribbonDiffXml.ReplaceWith(newRibbonDiffXml);
                                }

                                break;
                            }
                        }

                        using (Stream streamPart = part.GetStream(FileMode.Create, FileAccess.Write))
                        {
                            XmlWriterSettings settings = new XmlWriterSettings
                            {
                                OmitXmlDeclaration = true,
                                Indent = true,
                                Encoding = Encoding.UTF8
                            };

                            using (XmlWriter xmlWriter = XmlWriter.Create(streamPart, settings))
                            {
                                doc.Save(xmlWriter);
                            }
                        }
                    }
                }

                memStream.Position = 0;
                byte[] result = memStream.ToArray();

                return result;
            }
        }

        public static byte[] ReplaceApplicationRibbonDiffXmlInSolutionBody(byte[] solutionBodyBinary, XElement newRibbonDiffXml)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(solutionBodyBinary, 0, solutionBodyBinary.Length);

                using (ZipPackage package = (ZipPackage)ZipPackage.Open(memStream, FileMode.Open, FileAccess.ReadWrite))
                {
                    ZipPackagePart part = (ZipPackagePart)package.GetPart(new Uri("/customizations.xml", UriKind.Relative));

                    if (part != null)
                    {
                        XDocument doc = null;

                        using (Stream streamPart = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            doc = XDocument.Load(streamPart);
                        }

                        var ribbonDiffXml = doc.XPathSelectElement("ImportExportXml/RibbonDiffXml");

                        if (ribbonDiffXml != null)
                        {
                            ribbonDiffXml.ReplaceWith(newRibbonDiffXml);
                        }

                        using (Stream streamPart = part.GetStream(FileMode.Create, FileAccess.Write))
                        {
                            XmlWriterSettings settings = new XmlWriterSettings
                            {
                                OmitXmlDeclaration = true,
                                Indent = true,
                                Encoding = Encoding.UTF8
                            };

                            using (XmlWriter xmlWriter = XmlWriter.Create(streamPart, settings))
                            {
                                doc.Save(xmlWriter);
                            }
                        }
                    }
                }

                memStream.Position = 0;
                byte[] result = memStream.ToArray();

                return result;
            }
        }

        public Task ImportSolutionAsync(byte[] solutionBodyBinary)
        {
            return Task.Run(() => ImportSolution(solutionBodyBinary));
        }

        private void ImportSolution(byte[] solutionBodyBinary)
        {
            ImportSolutionRequest request = new ImportSolutionRequest()
            {
                CustomizationFile = solutionBodyBinary,
                OverwriteUnmanagedCustomizations = true,
            };

            var response = (ImportSolutionResponse)_service.Execute(request);
        }
    }
}
