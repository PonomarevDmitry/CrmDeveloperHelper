using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription.Implementation
{
    public class DefaultSolutionComponentDescriptionBuilder : ISolutionComponentDescriptionBuilder
    {
        public const string formatSpacer = "    {0}";

        public const string unknowedMessage = "Default Description for Unknowned Solution Components:";

        private readonly object _syncObjectEntityCache = new object();

        protected readonly IOrganizationServiceExtented _service;

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
                GetEntityName(this.ComponentTypeValue, out string entityName, out string entityIdName);

                return entityName;
            }
        }

        public virtual string EntityPrimaryIdAttribute
        {
            get
            {
                GetEntityName(this.ComponentTypeValue, out string entityName, out string entityIdName);

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

                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 5000,
                }
            };

            return query;
        }

        protected virtual ColumnSet GetColumnSet()
        {
            return new ColumnSet(true);
        }

        public static void GetEntityName(int type, out string entityName, out string entityIdName)
        {
            entityName = string.Empty;
            entityIdName = string.Empty;

            if (!SolutionComponent.IsDefinedComponentType(type))
            {
                return;
            }

            ComponentType componentType = (ComponentType)type;

            if (SolutionComponent.IsComponentTypeMetadata(componentType))
            {
                return;
            }

            switch (componentType)
            {
                case ComponentType.EmailTemplate:
                    entityName = Template.EntityLogicalName;
                    entityIdName = Template.PrimaryIdAttribute;
                    break;

                case ComponentType.RolePrivileges:
                    entityName = RolePrivileges.EntityLogicalName;
                    entityIdName = RolePrivileges.PrimaryIdAttribute;
                    break;

                case ComponentType.SystemForm:
                    entityName = SystemForm.EntityLogicalName;
                    entityIdName = SystemForm.PrimaryIdAttribute;
                    break;

                case ComponentType.DependencyFeature:
                    break;

                default:
                    entityName = componentType.ToString().ToLower();
                    entityIdName = entityName + "id";
                    break;
            }
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

        public virtual void GenerateDescription(StringBuilder builder, IEnumerable<SolutionComponent> components, bool withUrls)
        {
            builder.AppendFormat(formatSpacer, unknowedMessage).AppendLine();

            foreach (var item in components)
            {
                builder.AppendFormat(formatSpacer, item.ToString()).AppendLine();
            }
        }

        public virtual string GenerateDescriptionSingle(SolutionComponent solutionComponent, bool withUrls)
        {
            if (solutionComponent == null)
            {
                return null;
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

        public void FillSolutionImageComponent(List<SolutionImageComponent> result, SolutionComponent solutionComponent)
        {
            result.Add(new SolutionImageComponent()
            {
                ObjectId = solutionComponent.ObjectId,
                ComponentType = (solutionComponent.ComponentType?.Value).GetValueOrDefault(),
                RootComponentBehavior = solutionComponent.RootComponentBehavior?.Value,

                Description = GenerateDescriptionSingle(solutionComponent, false),
            });
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
    }
}