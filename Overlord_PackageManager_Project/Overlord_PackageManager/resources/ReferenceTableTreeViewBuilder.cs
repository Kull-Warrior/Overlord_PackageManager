using Overlord_PackageManager.resources.Generic;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources
{
    public static class RefTableTreeBuilder
    {
        public static TreeViewItem Build(ReferenceTable table, string name = "Table")
        {
            TreeViewItem node = new TreeViewItem
            {
                Header = name,
                Tag = table
            };

            foreach (Entry entry in table.Entries)
            {
                node.Items.Add(BuildEntryNode(entry));
            }

            return node;
        }

        private static TreeViewItem BuildEntryNode(Entry entry)
        {
            TreeViewItem node = new TreeViewItem
            {
                Header = Describe(entry),
                Tag = entry
            };

            if (entry is IHasReferenceTable hasTable)
            {
                ReferenceTable childTable = hasTable.GetReferenceTable();

                if (childTable != null)
                {
                    foreach (var child in childTable.Entries)
                        node.Items.Add(BuildEntryNode(child));
                }
            }

            return node;
        }

        private static string Describe(Entry e)
        {
            return $"{e.GetType().Name}  Id={e.Id:X}  Offset=0x{e.RelOffset:X}";
        }
    }
}
