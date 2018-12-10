using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Repository
{
    /// <summary>
    /// Репозиторий функций по поиску веб-ресурсов.
    /// </summary>
    public class WebResourceRepository
    {
        private static string[] _fields = null;

        /// <summary>
        /// Маппинг расширений файла
        /// </summary>
        private static ConcurrentDictionary<string, int> Types = new ConcurrentDictionary<string, int>
        (
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                {".html", (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1},
                {".htm",  (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1},
                {".css",  (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_CSS_2},
                {".js",   (int)WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3},
                {".xml",  (int)WebResource.Schema.OptionSets.webresourcetype.Data_XML_4},
                {".png",  (int)WebResource.Schema.OptionSets.webresourcetype.PNG_format_5},
                {".jpg",  (int)WebResource.Schema.OptionSets.webresourcetype.JPG_format_6},
                {".gif",  (int)WebResource.Schema.OptionSets.webresourcetype.GIF_format_7},
                {".xap",  (int)WebResource.Schema.OptionSets.webresourcetype.Silverlight_XAP_8},
                {".xslt", (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_XSL_9},
                {".xsl",  (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_XSL_9},
                {".ico",  (int)WebResource.Schema.OptionSets.webresourcetype.ICO_format_10},
                {".svg",  (int)WebResource.Schema.OptionSets.webresourcetype.SVG_format_11}
            }
            , StringComparer.InvariantCultureIgnoreCase
        );


        /// <summary>
        /// Сервис CRM
        /// </summary>
        private readonly IOrganizationServiceExtented _Service;

        /// <summary>
        /// Конструктор репозитория функция по поиску веб-ресурсов.
        /// </summary>
        /// <param name="service"></param>
        public WebResourceRepository(IOrganizationServiceExtented service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Task<WebResource> FindByNameAsync(string friendlyPath, string extension)
        {
            return Task.Run(() => FindByName(friendlyPath, extension));
        }

        /// <summary>
        /// Выполняет поиск веб-ресурса по названию
        /// </summary>
        /// <param name="name">имя файла</param>
        /// <param name="extension">расширение файла</param>
        /// <returns></returns>
        private WebResource FindByName(string friendlyPath, string extension)
        {
            extension = extension.ToLower();

            if (!Types.ContainsKey(extension))
                throw new Exception("File Extension " + extension + " is not allowed");

            var type = Types[extension];

            var result = SearchSingle(friendlyPath, extension, type);

            return result;
        }

        private WebResource SearchSingle(string friendlyPath, string extension, int type)
        {
            List<string> names = new List<string>();

            names.AddRange(GetSplitedNames(friendlyPath, extension));

            if (names.Count > 0)
            {
                var webResourceCollection = SearchByName(type, names.ToArray());

                return webResourceCollection.SingleOrDefault();
            }

            return null;
        }

        public Task<WebResource> FindByIdAsync(Guid resourceId, ColumnSet columnSet = null)
        {
            return Task.Run(() => FindById(resourceId, columnSet));
        }

        /// <summary>
        /// Получение веб-ресурса по его идентификатору
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private WebResource FindById(Guid resourceId, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(WebResource.Schema.Attributes.webresourceid, ConditionOperator.Equal, resourceId)
                    },
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<WebResource>()).SingleOrDefault();
        }

        /// <summary>
        /// Поиск веб-ресурсов по именам.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public Dictionary<string, WebResource> FindMultiple(string extension, string[] names, ColumnSet columnSet = null)
        {
            extension = extension.ToLower();

            if (!Types.ContainsKey(extension))
                throw new Exception("File Extension " + extension + " is not allowed");

            var type = Types[extension];

            var list = new List<string>();

            foreach (var item in names)
            {
                list.AddRange(WebResourceRepository.GetSplitedNames(item, extension));
            }

            var webResourceCollection = SearchByName(type, list.ToArray(), columnSet);

            Dictionary<string, WebResource> result = new Dictionary<string, WebResource>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var ent in webResourceCollection)
            {
                result.Add(ent.Name.ToLower(), ent);
            }

            return result;
        }

        private List<WebResource> SearchByName(int type, IEnumerable<string> names, ColumnSet columnSet = null)
        {
            List<WebResource> result = new List<WebResource>();

            int count = 700;

            int pages = (names.Count() + count - 1) / count;

            for (int i = 0; i < pages; i++)
            {
                result.AddRange(FindWebResourcesByNames(type, names.Skip(i * count).Take(count), columnSet));
            }

            return result;
        }

        private IEnumerable<WebResource> FindWebResourcesByNames(int type, IEnumerable<string> names, ColumnSet columnSet = null)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        //new ConditionExpression(WebResource.Schema.Attributes.ismanaged, ConditionOperator.Equal, false),
                        //new ConditionExpression(WebResource.Schema.Attributes.iscustomizable, ConditionOperator.Equal, true),
                        new ConditionExpression(WebResource.Schema.Attributes.webresourcetype, ConditionOperator.Equal, type),
                        new ConditionExpression(WebResource.Schema.Attributes.name, ConditionOperator.In, names.ToArray()),
                    }
                },

                PageInfo =
                {
                    Count = 5000,
                    PageNumber = 1,
                },
            };

            while (true)
            {
                var coll = _Service.RetrieveMultiple(query);

                foreach (var item in coll.Entities.Select(e => e.ToEntity<WebResource>()))
                {
                    yield return item;
                }

                if (!coll.MoreRecords)
                {
                    yield break;
                }

                query.PageInfo.PagingCookie = coll.PagingCookie;
                query.PageInfo.PageNumber++;
            }
        }

        public Task<List<WebResource>> GetListAsync(string name)
        {
            return Task.Run(() => GetList(name));
        }

        private List<WebResource> GetList(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = new ColumnSet(GetAttributes(_Service)),

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            //qe.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);
            //query.Criteria.AddCondition("iscustomizable", ConditionOperator.Equal, true);

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.AddCondition(WebResource.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
            }

            query.AddOrder(WebResource.Schema.Attributes.name, OrderType.Ascending);

            query.PageInfo = new PagingInfo()
            {
                PageNumber = 1,
                Count = 5000,
            };

            List<WebResource> result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<WebResource>> GetListSupportsTextAsync(string name = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetListSupportsText(name, columnSet));
        }

        private List<WebResource> GetListSupportsText(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(WebResource.Schema.Attributes.webresourcetype, ConditionOperator.In
                            , (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1
                            , (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_CSS_2
                            , (int)WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3
                            , (int)WebResource.Schema.OptionSets.webresourcetype.Data_XML_4
                            , (int)WebResource.Schema.OptionSets.webresourcetype.Style_Sheet_XSL_9
                        ),
                    }
                },

                Orders =
                {
                    new OrderExpression(WebResource.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo =
                {
                    Count = 5000,
                    PageNumber = 1,
                },
            };

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.AddCondition(WebResource.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
            }

            List<WebResource> result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<WebResource>> GetListAllAsync(string name = null, ColumnSet columnSet = null)
        {
            return Task.Run(() => GetListAll(name, columnSet));
        }

        private List<WebResource> GetListAll(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_Service)),

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = WebResource.EntityLogicalName,
                        LinkFromAttributeName = WebResource.Schema.Attributes.solutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = WebResource.EntityLogicalName,
                        LinkFromAttributeName = WebResource.Schema.Attributes.supportingsolutionid,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.Schema.EntityAliasFields.SupportingSolution,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },

                PageInfo =
                {
                    Count = 5000,
                    PageNumber = 1,
                },

                Orders =
                {
                    new OrderExpression(WebResource.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            //qe.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);
            //query.Criteria.AddCondition("iscustomizable", ConditionOperator.Equal, true);

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.AddCondition(WebResource.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
            }

            List<WebResource> result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<WebResource>> GetListByTypesAsync(IEnumerable<int> types, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByTypes(types, columnSet));
        }

        private List<WebResource> GetListByTypes(IEnumerable<int> types, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(GetAttributes(_Service)),

                PageInfo =
                {
                    Count = 5000,
                    PageNumber = 1,
                },

                Orders =
                {
                    new OrderExpression(WebResource.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            if (types != null && types.Any())
            {
                query.Criteria.AddCondition(new ConditionExpression(WebResource.Schema.Attributes.webresourcetype, ConditionOperator.In, types.ToArray()));
            }

            List<WebResource> result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<List<WebResource>> GetListAllWithContentAsync(string name = null)
        {
            return Task.Run(() => GetListAllWithContent(name));
        }

        private List<WebResource> GetListAllWithContent(string name)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = new ColumnSet(true),

                PageInfo =
                {
                    Count = 5000,
                    PageNumber = 1,
                },

                Orders =
                {
                    new OrderExpression(WebResource.Schema.Attributes.name, OrderType.Ascending),
                },
            };

            //qe.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);
            //query.Criteria.AddCondition("iscustomizable", ConditionOperator.Equal, true);

            if (!string.IsNullOrEmpty(name))
            {
                query.Criteria.AddCondition(WebResource.Schema.Attributes.name, ConditionOperator.Like, "%" + name + "%");
            }

            List<WebResource> result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }

        public Task<Guid> CreateNewWebResourceAsync(string name, string displayName, string description, string extension, string solutionUniqueName)
        {
            return Task.Run(() => CreateNewWebResource(name, displayName, description, extension, solutionUniqueName));
        }

        private Guid CreateNewWebResource(string name, string displayName, string description, string extension, string solutionUniqueName)
        {
            WebResource webResource = new WebResource()
            {
                LogicalName = WebResource.EntityLogicalName,
                Attributes =
                {
                    { WebResource.Schema.Attributes.displayname, displayName },
                    { WebResource.Schema.Attributes.name, name },
                    { WebResource.Schema.Attributes.webresourcetype, new OptionSetValue(WebResourceRepository.GetTypeByExtension(extension)) }
                }
            };

            if (!string.IsNullOrEmpty(description))
            {
                webResource.Attributes.Add(WebResource.Schema.Attributes.description, description);
            }


            CreateRequest request = new CreateRequest()
            {
                Target = webResource
            };

            request.Parameters.Add("SolutionUniqueName", solutionUniqueName);
            CreateResponse response = (CreateResponse)_Service.Execute(request);

            return response.id;
        }

        public static string GetTypeMainExtension(int type)
        {
            foreach (var item in Types)
            {
                if (item.Value == type)
                {
                    return item.Key;
                }
            }

            return string.Empty;
        }

        public static List<string> GetTypeAllExtensions(int type)
        {
            List<string> result = new List<string>();

            foreach (var item in Types)
            {
                if (item.Value == type)
                {
                    result.Add(item.Key);
                }
            }

            return result;
        }


        public static bool IsSupportedExtension(string extension)
        {
            return Types.ContainsKey(extension);
        }

        public static int GetTypeByExtension(string extension)
        {
            return Types[extension];
        }

        private static string[] GetAttributes(IOrganizationServiceExtented service)
        {
            if (_fields != null)
            {
                return _fields;
            }

            MetadataFilterExpression entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, WebResource.EntityLogicalName));

            var entityQueryExpression = new EntityQueryExpression()
            {
                Properties = new MetadataPropertiesExpression("Attributes"),
                AttributeQuery = new AttributeQueryExpression() { Properties = new MetadataPropertiesExpression() { AllProperties = true } },

                Criteria = entityFilter,
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(
                new RetrieveMetadataChangesRequest()
                {
                    ClientVersionStamp = null,
                    Query = entityQueryExpression,
                }
            );

            var metadata = response.EntityMetadata.SingleOrDefault(e => string.Equals(e.LogicalName, WebResource.EntityLogicalName, StringComparison.OrdinalIgnoreCase));

            if (metadata == null)
            {
                _fields = new string[0];
                return _fields;
            }

            var list = new List<string>();

            foreach (var attr in metadata.Attributes.OrderBy(a => a.LogicalName))
            {
                if (!string.IsNullOrEmpty(attr.AttributeOf))
                {
                    continue;
                }

                if (string.Equals(attr.LogicalName, WebResource.Schema.Attributes.content, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!attr.IsValidForRead.GetValueOrDefault()
                    //&& !attr.IsValidForCreate.GetValueOrDefault()
                    //&& !attr.IsValidForUpdate.GetValueOrDefault()
                    //&& !attr.IsValidForAdvancedFind.Value
                    )
                {
                    continue;
                }

                list.Add(attr.LogicalName);
            }

            _fields = list.ToArray();

            return _fields;
        }

        public static string GetWebResourceFileName(WebResource webResource)
        {
            string extension = WebResourceRepository.GetTypeMainExtension(webResource.WebResourceType.Value);

            var allExtensions = WebResourceRepository.GetTypeAllExtensions(webResource.WebResourceType.Value);

            string webResourceFileName = Path.GetFileName(webResource.Name);

            webResourceFileName = FileOperations.RemoveWrongSymbols(webResourceFileName);

            webResourceFileName = CheckExtension(webResourceFileName, extension, allExtensions);

            return webResourceFileName;
        }

        private static string CheckExtension(string webResourceFileName, string extension, List<string> allExtensions)
        {
            if (!FileEndsWith(webResourceFileName, allExtensions))
            {
                webResourceFileName += extension;
            }

            return webResourceFileName;
        }

        private static bool FileEndsWith(string localFileName, List<string> allExtensions)
        {
            foreach (var ext in allExtensions)
            {
                if (localFileName.EndsWith(ext))
                {
                    return true;
                }
            }

            return false;
        }

        public WebResource FindByExactName(string name, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                NoLock = true,

                TopCount = 2,

                EntityName = WebResource.EntityLogicalName,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(WebResource.Schema.Attributes.name, ConditionOperator.Equal, name),
                    },
                },
            };

            return _Service.RetrieveMultiple(query).Entities.Select(e => e.ToEntity<WebResource>()).SingleOrDefault();
        }

        public static IEnumerable<string> GetSplitedNames(string friendlyPath, string extension)
        {
            if (string.IsNullOrEmpty(friendlyPath))
            {
                yield break;
            }

            {
                var temp = string.Empty;

                var split = friendlyPath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in split.Reverse())
                {
                    if (temp.Length > 0)
                    {
                        temp = "/" + temp;
                    }

                    temp = item + temp;

                    yield return temp;

                    if (!string.IsNullOrEmpty(extension))
                    {
                        if (temp.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                        {
                            yield return temp.Substring(0, temp.LastIndexOf(extension, StringComparison.OrdinalIgnoreCase));
                        }
                        else
                        {
                            yield return temp + extension;
                        }
                    }
                }
            }
        }

        public static WebResource FindWebResourceInDictionary(Dictionary<string, WebResource> dict, string friendlyFilePath, string extension)
        {
            foreach (var item in GetSplitedNames(friendlyFilePath, extension))
            {
                if (dict.ContainsKey(item))
                {
                    return dict[item];
                }
            }

            return null;
        }

        public static string GenerateWebResouceName(string name, string publisherPrefix)
        {
            string result = name;

            publisherPrefix = publisherPrefix.TrimEnd(' ', '_');

            if (!string.IsNullOrEmpty(publisherPrefix)
                && !string.Equals(publisherPrefix, "none", StringComparison.InvariantCultureIgnoreCase)
                && !result.StartsWith(publisherPrefix + "_")
                )
            {
                result = string.Format("{0}_{1}", publisherPrefix, result);
            }

            return result;
        }

        public Task<List<WebResource>> GetListByIdListAsync(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            return Task.Run(() => GetListByIdList(ids, columnSet));
        }

        private List<WebResource> GetListByIdList(IEnumerable<Guid> ids, ColumnSet columnSet)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = WebResource.EntityLogicalName,

                NoLock = true,

                ColumnSet = columnSet ?? new ColumnSet(true),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(WebResource.Schema.EntityPrimaryIdAttribute, ConditionOperator.In, ids.ToArray()),
                    },
                },

                Orders =
                {
                    new OrderExpression(WebResource.Schema.Attributes.name, OrderType.Ascending),
                },

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                },
            };

            var result = new List<WebResource>();

            try
            {
                while (true)
                {
                    var coll = _Service.RetrieveMultiple(query);

                    result.AddRange(coll.Entities.Select(e => e.ToEntity<WebResource>()));

                    if (!coll.MoreRecords)
                    {
                        break;
                    }

                    query.PageInfo.PagingCookie = coll.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }
            catch (Exception ex)
            {
                Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.DTEHelper.WriteExceptionToOutput(ex);
            }

            return result;
        }
    }
}
