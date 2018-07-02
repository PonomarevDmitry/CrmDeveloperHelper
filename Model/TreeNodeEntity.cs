using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class TreeNodeEntity
    {
        public string Name { get; set; }

        public List<TreeNodeEntity> SubNodes { get; private set; }

        public List<Tuple<string, WebResource>> Entities { get; private set; }

        public TreeNodeEntity()
        {
            this.SubNodes = new List<TreeNodeEntity>();
            this.Entities = new List<Tuple<string, WebResource>>();
        }

        public static TreeNodeEntity Convert(IEnumerable<WebResource> list)
        {
            TreeNodeEntity result = new TreeNodeEntity();

            foreach (var item in list)
            {
                var split = item.Name.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 1)
                {
                    result.Entities.Add(Tuple.Create(item.Name, item));
                }
                else
                {
                    var node = result;

                    for (int index = 0; index < split.Length - 1; index++)
                    {
                        var name = split[index];

                        var nextNode = node.SubNodes.FirstOrDefault(n => string.Equals(n.Name, name));

                        if (nextNode == null)
                        {
                            nextNode = new TreeNodeEntity()
                            {
                                Name = name,
                            };

                            node.SubNodes.Add(nextNode);
                        }

                        node = nextNode;
                    }

                    string lastName = split.Last();

                    node.Entities.Add(Tuple.Create(lastName, item));
                }
            }

            return result;
        }
    }
}