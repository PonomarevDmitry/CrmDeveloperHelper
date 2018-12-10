using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class DefaultSolutionComponentDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        public const string formatSpacer = "    {0}";

        public const string unknowedMessage = "Default Description for Unknowned Solution Components:";

        private readonly object _syncObjectEntityCache = new object();

        protected IOrganizationServiceExtented _service { get; private set; }

        public const string SolutionAttribute = "solutionid";

        public const string SupportingSolutionAlias = "suppsolution";
        public const string SupportingSolutionAttribute = "supportingsolutionid";

        public virtual int ComponentTypeValue { get; private set; }

        public virtual ComponentType? ComponentTypeEnum
        {
            get
            {
                if (SolutionComponent.IsDefinedComponentType(this.ComponentTypeValue))
                {
                    return (ComponentType)this.ComponentTypeValue;
                }

                return null;
            }
        }

        public virtual string EntityLogicalName
        {
            get
            {
                SolutionComponent.GetComponentTypeEntityName(this.ComponentTypeValue, out string entityName, out string entityIdName);

                return entityName;
            }
        }

        public virtual string EntityPrimaryIdAttribute
        {
            get
            {
                SolutionComponent.GetComponentTypeEntityName(this.ComponentTypeValue, out string entityName, out string entityIdName);

                return entityIdName;
            }
        }

        public DefaultSolutionComponentDescriptionBuilder(IOrganizationServiceExtented service, int componentType)
        {
            this._service = service;
            this.ComponentTypeValue = componentType;
        }

        private ConcurrentDictionary<Guid, Entity> _cache = new ConcurrentDictionary<Guid, Entity>();

        public List<T> GetEntities<T>(IEnumerable<Guid> components) where T : Entity
        {
            return GetEntities<T>(components.Select(id => (Guid?)id));
        }

        public List<T> GetEntities<T>(IEnumerable<Guid?> components) where T : Entity
        {
            List<Guid> idsNotCached = components.Where(c => c.HasValue).Select(comp => comp.Value).ToList();

            var list = GetCachedEntities(idsNotCached);

            if (idsNotCached.Count > 0)
            {
                QueryExpression query = GetQuery(idsNotCached);

                List<Entity> result = GetEntitiesByQuery(query);

                CacheEntities(result);

                list.AddRange(result);
            }

            return list.Select(e => e.ToEntity<T>()).ToList();
        }

        protected virtual List<Entity> GetEntitiesByQuery(QueryExpression query)
        {
            List<Entity> result = new List<Entity>();

            query.PageInfo = new PagingInfo()
            {
                PageNumber = 1,
                Count = 5000,
            };

            if (query != null)
            {
                try
                {
                    while (true)
                    {
                        var coll = _service.RetrieveMultiple(query);

                        result.AddRange(coll.Entities);

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
                    DTEHelper.Singleton.WriteErrorToOutput(ex);
                }
            }

            return result;
        }

        protected virtual QueryExpression GetQuery(List<Guid> idsNotCached)
        {
            if (string.IsNullOrEmpty(this.EntityLogicalName) || string.IsNullOrEmpty(this.EntityPrimaryIdAttribute))
            {
                return null;
            }

            var query = new QueryExpression()
            {
                NoLock = true,

                EntityName = this.EntityLogicalName,

                ColumnSet = GetColumnSet(),

                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(this.EntityPrimaryIdAttribute, ConditionOperator.In, idsNotCached.ToArray()),
                    },
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = this.EntityLogicalName,
                        LinkFromAttributeName = SolutionAttribute,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = Solution.EntityLogicalName,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },

                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,

                        LinkFromEntityName = this.EntityLogicalName,
                        LinkFromAttributeName = SupportingSolutionAttribute,

                        LinkToEntityName = Solution.EntityLogicalName,
                        LinkToAttributeName = Solution.PrimaryIdAttribute,

                        EntityAlias = SupportingSolutionAlias,

                        Columns = new ColumnSet(Solution.Schema.Attributes.uniquename, Solution.Schema.Attributes.ismanaged),
                    },
                },
            };

            return query;
        }

        protected virtual ColumnSet GetColumnSet()
        {
            return new ColumnSet(true);
        }

        private List<Entity> GetCachedEntities(List<Guid> idsNotCached)
        {
            var result = new List<Entity>();

            lock (_syncObjectEntityCache)
            {
                if (idsNotCached.Any())
                {
                    var listToDelete = new List<Guid>();

                    foreach (var objectId in idsNotCached)
                    {
                        if (_cache.ContainsKey(objectId))
                        {
                            listToDelete.Add(objectId);

                            result.Add(_cache[objectId]);
                        }
                    }

                    listToDelete.ForEach(id => idsNotCached.Remove(id));
                }
            }

            return result;
        }

        private void CacheEntities(List<Entity> listEntities)
        {
            lock (_syncObjectEntityCache)
            {
                foreach (var entity in listEntities)
                {
                    _cache.TryAdd(entity.Id, entity);
                }
            }
        }

        public T GetEntity<T>(Guid idEntity) where T : Entity
        {
            var listEntities = GetEntities<T>(new Guid[] { idEntity });

            var entity = listEntities.FirstOrDefault();

            if (entity != null)
            {
                return entity.ToEntity<T>();
            }

            return null;
        }

        public void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            var list = GetEntities<Entity>(components.Select(c => c.ObjectId));

            {
                var hash = new HashSet<Guid>(list.Select(en => en.Id));
                var notFinded = components.Where(en => !hash.Contains(en.ObjectId.Value)).ToList();
                if (notFinded.Any())
                {
                    builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();
                    foreach (var item in notFinded)
                    {
                        var behavior = SolutionComponent.GetRootComponentBehaviorName(item.RootComponentBehavior?.Value);

                        builder.AppendFormat(formatSpacer, item.ToString(), string.Format(formatSpacer, behavior)).AppendLine();
                    }
                }
            }

            FormatTextTableHandler handler = GetDescriptionHeader(withManaged, withSolutionInfo, withUrls, AppendIntoTableHeader);

            foreach (var entity in list)
            {
                var solutionComponent = components.SingleOrDefault(e => e.ObjectId == entity.Id);

                var behavior = SolutionComponent.GetRootComponentBehaviorName(solutionComponent?.RootComponentBehavior?.Value);

                List<string> values = GetDescriptionValues(entity, behavior, withManaged, withSolutionInfo, withUrls, AppendIntoValues);

                handler.AddLine(values);
            }

            List<string> lines = handler.GetFormatedLines(true);

            lines.ForEach(item => builder.AppendFormat(formatSpacer, item).AppendLine());
        }

        protected virtual FormatTextTableHandler GetDescriptionHeader(bool withManaged, bool withSolutionInfo, bool withUrls, Action<FormatTextTableHandler, bool, bool, bool> action)
        {
            var result = new FormatTextTableHandler();
            result.SetHeader("Id");
            result.SetHeader("Behavior");

            action(result, withUrls, withManaged, withSolutionInfo);

            return result;
        }

        protected virtual List<string> GetDescriptionValues(Entity entityInput, string behavior, bool withManaged, bool withSolutionInfo, bool withUrls, Action<List<string>, Entity, bool, bool, bool> action)
        {
            var result = new List<string>() { entityInput.Id.ToString() };

            action(result, entityInput, withUrls, withManaged, withSolutionInfo);

            return result;
        }

        public string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withManaged, bool withSolutionInfo, bool withUrls)
        {
            if (solutionComponent == null)
            {
                return null;
            }

            var entityInput = GetEntity<Entity>(solutionComponent.ObjectId.Value);

            if (entityInput != null)
            {
                FormatTextTableHandler handler = GetDescriptionHeader(withManaged, withSolutionInfo, withUrls, AppendIntoTableHeaderSingle);

                var behavior = SolutionComponent.GetRootComponentBehaviorName(solutionComponent?.RootComponentBehavior?.Value);

                List<string> values = GetDescriptionValues(entityInput, behavior, withManaged, withSolutionInfo, withUrls, AppendIntoValuesSingle);

                handler.AddLine(values);

                var str = handler.GetFormatedLinesWithHeadersInLine(false).FirstOrDefault();

                return string.Format("{0} {1}", entityInput.LogicalName, str);
            }

            return solutionComponent.ToString();
        }

        public virtual string GetName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || !solutionComponent.ObjectId.HasValue)
            {
                return "null";
            }

            var entity = GetEntity<Entity>(solutionComponent.ObjectId.Value);

            if (entity != null && entity.Attributes.ContainsKey("name"))
            {
                string name = entity.GetAttributeValue<string>("name");

                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }

            return solutionComponent.ObjectId.ToString();
        }

        public virtual string GetDisplayName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public virtual string GetCustomizableName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var entity = GetEntity<Entity>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.Contains("iscustomizable") && entity.Attributes["iscustomizable"] is BooleanManagedProperty booleanManagedProperty)
                {
                    return booleanManagedProperty.Value.ToString();
                }

                if (entity.Contains("iscustomizable") && entity.Attributes["iscustomizable"] is bool boolValue)
                {
                    return boolValue.ToString();
                }
            }

            return null;
        }

        public virtual string GetManagedName(SolutionComponent solutionComponent)
        {
            if (solutionComponent == null || solutionComponent.ComponentType == null || !solutionComponent.ObjectId.HasValue)
            {
                return null;
            }

            var entity = GetEntity<Entity>(solutionComponent.ObjectId.Value);

            if (entity != null)
            {
                if (entity.Contains("ismanaged"))
                {
                    if (entity.Contains("ismanaged") && entity.Attributes["ismanaged"] is BooleanManagedProperty booleanManagedProperty)
                    {
                        return booleanManagedProperty.Value.ToString();
                    }

                    if (entity.Contains("ismanaged") && entity.Attributes["ismanaged"] is bool boolValue)
                    {
                        return boolValue.ToString();
                    }
                }
            }

            return null;
        }

        public virtual string GetLinkedEntityName(SolutionComponent solutionComponent)
        {
            return null;
        }

        public virtual void FillSolutionImageComponent(ICollection<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            if (solutionComponent == null)
            {
                return;
            }

            result.Add(new SolutionImageComponent()
            {
                ObjectId = solutionComponent.ObjectId,
                ComponentType = this.ComponentTypeValue,
                RootComponentBehavior = (solutionComponent.RootComponentBehavior?.Value).GetValueOrDefault((int)RootComponentBehavior.IncludeSubcomponents),

                Description = GenerateDescriptionSingle(solutionComponent, true, false, false),
            });
        }

        public virtual void FillSolutionComponent(ICollection<SolutionComponent> result, SolutionImageComponent solutionImageComponent)
        {
            if (solutionImageComponent == null || !solutionImageComponent.ObjectId.HasValue)
            {
                return;
            }

            Guid entityId = solutionImageComponent.ObjectId.Value;
            int? behavior = solutionImageComponent.RootComponentBehavior;

            FillSolutionComponentInternal(result, entityId, behavior);
        }

        public virtual void FillSolutionComponentFromXml(ICollection<SolutionComponent> result, XElement elementRootComponent, XDocument docCustomizations)
        {
            if (elementRootComponent == null)
            {
                return;
            }

            var id = GetIdFromXml(elementRootComponent);

            if (id.HasValue)
            {
                int? behavior = GetBehaviorFromXml(elementRootComponent);

                FillSolutionComponentInternal(result, id.Value, behavior);
            }
        }

        protected void FillSolutionComponentInternal(ICollection<SolutionComponent> result, Guid entityId, int? behavior)
        {
            var entity = GetEntity<Entity>(entityId);

            if (entity != null)
            {
                var component = new SolutionComponent()
                {
                    ComponentType = new OptionSetValue(this.ComponentTypeValue),
                    ObjectId = entityId,
                    RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                };

                if (behavior.HasValue)
                {
                    component.RootComponentBehavior = new OptionSetValue(behavior.Value);
                }

                result.Add(component);
            }
        }

        public virtual string GetFileName(string connectionName, Guid objectId, string fieldTitle, string extension)
        {
            var entity = GetEntity<Entity>(objectId);

            if (entity != null)
            {
                return EntityFileNameFormatter.GetEntityName(connectionName, entity, fieldTitle, extension);
            }

            return string.Format("{0}.ComponentType {1} - {2} - {3}.{4}", connectionName, this.ComponentTypeValue, objectId, fieldTitle, extension);
        }

        public virtual TupleList<string, string> GetComponentColumns()
        {
            return new TupleList<string, string>();
        }

        protected void AppendIntoTableHeader(FormatTextTableHandler handler, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            if (withSolutionInfo)
            {
                handler.AppendHeader("SolutionName");

                if (withManaged)
                {
                    handler.AppendHeader("SolutionIsManaged");
                }

                handler.AppendHeader("SupportingName");

                if (withManaged)
                {
                    handler.AppendHeader("SupportinIsManaged");
                }
            }

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }
        }

        protected void AppendIntoValues(List<string> values, Entity entity, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            if (withManaged)
            {
                values.Add(EntityDescriptionHandler.GetAttributeString(entity, "ismanaged"));
            }

            if (withSolutionInfo)
            {
                values.Add(EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));

                if (withManaged)
                {
                    values.Add(EntityDescriptionHandler.GetAttributeString(entity, "solution.ismanaged"));
                }

                values.Add(EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.uniquename"));

                if (withManaged)
                {
                    values.Add(EntityDescriptionHandler.GetAttributeString(entity, "suppsolution.ismanaged"));
                }
            }

            if (withUrls && this.ComponentTypeEnum.HasValue)
            {
                values.Add(_service.UrlGenerator?.GetSolutionComponentUrl(this.ComponentTypeEnum.Value, entity.Id));
            }
        }

        protected void AppendIntoTableHeaderSingle(FormatTextTableHandler handler, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            if (withManaged)
            {
                handler.AppendHeader("IsManaged");
            }

            if (withSolutionInfo)
            {
                handler.AppendHeader("SolutionName");
            }

            if (withUrls)
            {
                handler.AppendHeader("Url");
            }
        }

        protected void AppendIntoValuesSingle(List<string> values, Entity entity, bool withUrls, bool withManaged, bool withSolutionInfo)
        {
            if (withManaged)
            {
                values.Add(EntityDescriptionHandler.GetAttributeString(entity, "ismanaged"));
            }

            if (withSolutionInfo)
            {
                values.Add(EntityDescriptionHandler.GetAttributeString(entity, "solution.uniquename"));
            }

            if (withUrls && this.ComponentTypeEnum.HasValue)
            {
                values.Add(_service.UrlGenerator?.GetSolutionComponentUrl(this.ComponentTypeEnum.Value, entity.Id));
            }
        }

        public static string GetSchemaNameFromXml(XElement elementRootComponent)
        {
            var elementSchemaName = elementRootComponent.Attribute("schemaName");

            return elementSchemaName?.Value;
        }

        public static int? GetBehaviorFromXml(XElement elementRootComponent)
        {
            int? result = null;

            var elementBehavior = elementRootComponent.Attribute("behavior");

            if (elementBehavior != null && int.TryParse(elementBehavior.Value, out var tempInt))
            {
                result = tempInt;
            }

            return result;
        }

        public static string GetAttributeValue(XElement elementRootComponent, string attributeName)
        {
            var elementSchemaName = elementRootComponent.Attribute(attributeName);

            return elementSchemaName?.Value;
        }

        public static Guid? GetIdFromXml(XElement elementRootComponent)
        {
            Guid? result = null;

            var elementId = elementRootComponent.Attribute("id");

            if (elementId != null && Guid.TryParse(elementId.Value, out var tempGuid))
            {
                result = tempGuid;
            }

            return result;
        }
    }
}