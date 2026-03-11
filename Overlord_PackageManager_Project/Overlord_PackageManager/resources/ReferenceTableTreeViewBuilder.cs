using Overlord_PackageManager.resources.Generic;
using System.Windows;
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

            node.Expanded += OnNodeExpanded;
            node.Collapsed += OnNodeCollapsed;

            AddPlaceholder(node);

            return node;
        }

        private static TreeViewItem CreateEntryNode(Entry entry)
        {
            TreeViewItem node = new TreeViewItem
            {
                Header = Describe(entry),
                Tag = entry
            };

            if (entry is IHasReferenceTable hasTable && hasTable.GetReferenceTable() != null)
            {
                node.Expanded += OnNodeExpanded;
                node.Collapsed += OnNodeCollapsed;

                AddPlaceholder(node);
            }

            return node;
        }

        private static void OnNodeExpanded(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;

            if (sender is not TreeViewItem node)
                return;

            if (!IsPlaceholder(node))
                return;

            node.Items.Clear();

            ReferenceTable table = null;

            if (node.Tag is ReferenceTable t)
                table = t;
            else if (node.Tag is IHasReferenceTable hasTable)
                table = hasTable.GetReferenceTable();

            if (table != null)
            {
                foreach (Entry entry in table.Entries)
                    node.Items.Add(CreateEntryNode(entry));
            }
        }

        private static void OnNodeCollapsed(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;

            if (sender is not TreeViewItem node)
                return;

            node.Items.Clear();
            AddPlaceholder(node);
        }

        private static void AddPlaceholder(TreeViewItem node)
        {
            node.Items.Add(new TreeViewItem { Header = "Loading..." });
        }

        private static bool IsPlaceholder(TreeViewItem node)
        {
            if (node.Items.Count != 1)
                return false;

            return node.Items[0] is TreeViewItem placeholder &&
                   (string)placeholder.Header == "Loading...";
        }

        private static string Describe(Entry e)
        {
            return $"Id={e.Id:X4} {e.GetType().Name}";
        }
    }
}