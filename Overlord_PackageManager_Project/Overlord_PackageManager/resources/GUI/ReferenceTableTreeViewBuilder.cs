using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.Data.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Overlord_PackageManager.resources.GUI
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

        private static void OnTreeViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            if (sender is not TreeView tree)
                return;

            if (tree.SelectedItem is not TreeViewItem node)
                return;

            DeleteEntry(node);

            e.Handled = true;
        }

        public static void AttachDeleteKeyHandler(TreeView tree)
        {
            tree.PreviewKeyDown += OnTreeViewKeyDown;
        }

        private static void DeleteEntry(TreeViewItem node)
        {
            if (node.Tag is not Entry entry)
                return;

            ItemsControl parentControl = ItemsControl.ItemsControlFromItemContainer(node);

            if (parentControl is not TreeViewItem parentNode)
                return;

            ReferenceTable table = parentNode.Tag switch
            {
                ReferenceTable t => t,
                IHasReferenceTable h => h.GetReferenceTable(),
                _ => null
            };

            if (table == null)
                return;

            int index = parentNode.Items.IndexOf(node);

            TreeViewItem nextSelection = null;

            if (index + 1 < parentNode.Items.Count)
                nextSelection = parentNode.Items[index + 1] as TreeViewItem;
            else if (index - 1 >= 0)
                nextSelection = parentNode.Items[index - 1] as TreeViewItem;
            else
                nextSelection = parentNode;

            table.Entries.Remove(entry);
            parentNode.Items.Remove(node);

            if (nextSelection != null)
            {
                nextSelection.IsSelected = true;
                nextSelection.Focus();
            }
        }

        private static ContextMenu BuildEntryContextMenu(TreeViewItem node)
        {
            ContextMenu menu = new ContextMenu();

            MenuItem deleteItem = new MenuItem
            {
                Header = "Delete"
            };

            deleteItem.Click += (s, e) => DeleteEntry(node);

            menu.Items.Add(deleteItem);

            return menu;
        }

        private static TreeViewItem CreateEntryNode(Entry entry)
        {
            TreeViewItem node = new TreeViewItem
            {
                Header = Describe(entry),
                Tag = entry
            };

            node.ContextMenu = BuildEntryContextMenu(node);

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