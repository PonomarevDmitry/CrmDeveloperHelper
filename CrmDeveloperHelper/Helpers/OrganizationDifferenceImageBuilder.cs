using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class OrganizationDifferenceImageBuilder
    {
        private readonly IOrganizationServiceExtented _service1;

        private readonly IOrganizationServiceExtented _service2;

        public SolutionComponentDescriptor Descriptor1 { get; private set; }

        public SolutionComponentDescriptor Descriptor2 { get; private set; }

        private readonly HashSet<SolutionComponent> _componentsOnlyIn1;
        private readonly HashSet<SolutionComponent> _componentsOnlyIn2;

        private readonly TupleList<SolutionComponent, SolutionComponent, string> _componentsDifferent;

        public OrganizationDifferenceImageBuilder(IOrganizationServiceExtented service1, IOrganizationServiceExtented service2)
        {
            this._service1 = service1;
            this._service2 = service2;

            this.Descriptor1 = new SolutionComponentDescriptor(this._service1, false)
            {
                WithManagedInfo = false,
                WithSolutionsInfo = false,
            };
            this.Descriptor2 = new SolutionComponentDescriptor(this._service2, false)
            {
                WithManagedInfo = false,
                WithSolutionsInfo = false,
            };

            this._componentsOnlyIn1 = new HashSet<SolutionComponent>();
            this._componentsOnlyIn2 = new HashSet<SolutionComponent>();
            this._componentsDifferent = new TupleList<SolutionComponent, SolutionComponent, string>();
        }

        public async Task<OrganizationDifferenceImage> GetImage()
        {
            DateTime now = DateTime.Now;

            var result = new OrganizationDifferenceImage()
            {
                MachineName = Environment.MachineName,
                ExecuteUserDomainName = Environment.UserDomainName,
                ExecuteUserName = Environment.UserName,

                CreatedOn = now,

                Connection1Image = new SolutionImage()
                {
                    ConnectionName = _service1.ConnectionData.Name,

                    ConnectionOrganizationName = _service1.ConnectionData.UniqueOrgName,
                    ConnectionDiscoveryService = _service1.ConnectionData.DiscoveryUrl,
                    ConnectionOrganizationService = _service1.ConnectionData.OrganizationUrl,
                    ConnectionPublicUrl = _service1.ConnectionData.PublicUrl,

                    ConnectionSystemUserName = _service1.ConnectionData.GetUsername,

                    MachineName = Environment.MachineName,
                    ExecuteUserDomainName = Environment.UserDomainName,
                    ExecuteUserName = Environment.UserName,

                    CreatedOn = now,
                },

                Connection2Image = new SolutionImage()
                {
                    ConnectionName = _service2.ConnectionData.Name,

                    ConnectionOrganizationName = _service2.ConnectionData.UniqueOrgName,
                    ConnectionDiscoveryService = _service2.ConnectionData.DiscoveryUrl,
                    ConnectionOrganizationService = _service2.ConnectionData.OrganizationUrl,
                    ConnectionPublicUrl = _service2.ConnectionData.PublicUrl,

                    MachineName = Environment.MachineName,
                    ExecuteUserDomainName = Environment.UserDomainName,
                    ExecuteUserName = Environment.UserName,

                    CreatedOn = now,
                },
            };

            {
                var list = await Descriptor1.GetSolutionImageComponentsListAsync(this._componentsOnlyIn1);

                foreach (var item in list)
                {
                    if (!result.Connection1Image.Components.Contains(item))
                    {
                        result.Connection1Image.Components.Add(item);
                    }
                }
            }

            {
                var list = await Descriptor2.GetSolutionImageComponentsListAsync(this._componentsOnlyIn2);

                foreach (var item in list)
                {
                    if (!result.Connection2Image.Components.Contains(item))
                    {
                        result.Connection2Image.Components.Add(item);
                    }
                }
            }

            {
                var task1 = Descriptor1.GetSolutionImageComponentsListAsync(this._componentsDifferent.Select(e => e.Item1));
                var task2 = Descriptor2.GetSolutionImageComponentsListAsync(this._componentsDifferent.Select(e => e.Item2));

                await task1;
                await task2;

                foreach (var item in this._componentsDifferent)
                {
                    var list = await Descriptor1.GetSolutionImageComponentsListAsync(new[] { item.Item1 });

                    foreach (var item1 in list)
                    {
                        var diff = new OrganizationDifferenceImageComponent(item1)
                        {
                            DescriptionSecond = Descriptor2.GetComponentDescription(item.Item2.ComponentType.Value, item.Item2.ObjectId.Value),
                            DescriptionDifference = item.Item3,
                        };

                        if (!result.DifferentComponents.Contains(diff))
                        {
                            result.DifferentComponents.Add(diff);
                        }
                    }
                }
            }

            var sorter = new SolutionImageComponentSorter();

            result.Connection1Image.Components.Sort(sorter);
            result.Connection2Image.Components.Sort(sorter);
            result.DifferentComponents.Sort(sorter);

            return result;
        }

        public void AddComponentSolution1(int componentType, Guid objectId, int? rootComponentBehavior = null)
        {
            var item = new SolutionComponent()
            {
                ComponentType = new Microsoft.Xrm.Sdk.OptionSetValue(componentType),
                ObjectId = objectId,
            };

            if (rootComponentBehavior.HasValue)
            {
                item.RootComponentBehavior = new Microsoft.Xrm.Sdk.OptionSetValue(rootComponentBehavior.Value);
            }

            this._componentsOnlyIn1.Add(item);
        }

        public void AddComponentSolution2(int componentType, Guid objectId, int? rootComponentBehavior = null)
        {
            var item = new SolutionComponent()
            {
                ComponentType = new Microsoft.Xrm.Sdk.OptionSetValue(componentType),
                ObjectId = objectId,
            };

            if (rootComponentBehavior.HasValue)
            {
                item.RootComponentBehavior = new Microsoft.Xrm.Sdk.OptionSetValue(rootComponentBehavior.Value);
            }

            this._componentsOnlyIn2.Add(item);
        }

        public void AddComponentDifferent(int componentType, Guid objectId1, Guid objectId2, string difference = null, int? rootComponentBehavior = null)
        {
            var item1 = new SolutionComponent()
            {
                ComponentType = new Microsoft.Xrm.Sdk.OptionSetValue(componentType),
                ObjectId = objectId1,
            };

            var item2 = new SolutionComponent()
            {
                ComponentType = new Microsoft.Xrm.Sdk.OptionSetValue(componentType),
                ObjectId = objectId2,
            };

            if (rootComponentBehavior.HasValue)
            {
                item1.RootComponentBehavior = new Microsoft.Xrm.Sdk.OptionSetValue(rootComponentBehavior.Value);
                item2.RootComponentBehavior = new Microsoft.Xrm.Sdk.OptionSetValue(rootComponentBehavior.Value);
            }

            this._componentsDifferent.Add(item1, item2, difference);
        }
    }
}
