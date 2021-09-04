using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class RoleEditorLayoutTab
    {
        public string id { get; private set; }

        public string Name { get; private set; }

        public string LocId { get; private set; }

        public List<RoleEditorLayoutEntity> Entities { get; private set; }

        public HashSet<int> EntitiesHash { get; private set; }

        public List<RoleEditorLayoutPrivilege> Privileges { get; private set; }

        public HashSet<Guid> PrivilegesHash { get; private set; }

        private RoleEditorLayoutTab(string id, string name, string locId, List<RoleEditorLayoutEntity> entities, List<RoleEditorLayoutPrivilege> privileges)
        {
            this.id = id;
            this.Name = name;
            this.LocId = locId;

            this.Entities = entities;
            this.Privileges = privileges;

            this.EntitiesHash = new HashSet<int>(entities.Select(e => e.EntityCode));
            this.PrivilegesHash = new HashSet<Guid>(privileges.Select(p => p.id));
        }

        public override string ToString()
        {
            return this.Name;
        }

        private static List<RoleEditorLayoutTab> _tabs = null;

        public static List<RoleEditorLayoutTab> GetTabs()
        {
            if (_tabs == null)
            {
                _tabs = ParseResource();
            }

            return _tabs;
        }

        private static List<RoleEditorLayoutTab> ParseResource()
        {
            var result = new List<RoleEditorLayoutTab>();

            var uri = FileOperations.GetResourceUri("RoleEditorLayout.xml");
            var info = Application.GetResourceStream(uri);

            var doc = XDocument.Load(info.Stream);
            info.Stream.Dispose();

            foreach (var tabElement in doc.Descendants("Tab"))
            {
                var entities = new List<RoleEditorLayoutEntity>();
                var privileges = new List<RoleEditorLayoutPrivilege>();

                foreach (var entity in tabElement.Descendants("Entity"))
                {
                    entities.Add(new RoleEditorLayoutEntity(
                        (string)entity.Attribute("Name")
                        , (int)entity.Attribute("EntityCode")
                        , (string)entity.Attribute("LocId")
                    ));
                }

                foreach (var privilege in tabElement.Descendants("Privilege"))
                {
                    privileges.Add(new RoleEditorLayoutPrivilege(
                        (Guid)privilege.Attribute("id")
                        , (string)privilege.Attribute("Name")
                        , (string)privilege.Attribute("LocId")
                    ));
                }

                var tab = new RoleEditorLayoutTab(
                    (string)tabElement.Attribute("id")
                    , (string)tabElement.Attribute("Name")
                    , (string)tabElement.Attribute("LocId")
                    , entities
                    , privileges
                );

                result.Add(tab);
            }

            return result;
        }
    }
}
