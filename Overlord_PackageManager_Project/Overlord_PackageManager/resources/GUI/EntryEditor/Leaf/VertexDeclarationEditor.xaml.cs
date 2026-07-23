using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class VertexDeclarationEditor : UserControl
    {
        private readonly RawListEntry<VertexAttribute> _entry;

        public VertexDeclarationEditor(RawListEntry<VertexAttribute> entry)
        {
            InitializeComponent();

            _entry = entry;

            if (_entry.Value == null)
            {
                _entry.Value = [];
            }

            RefreshEditors();
        }

        private void RefreshEditors()
        {
            EditorContainer.Children.Clear();

            for (int i = 0; i < _entry.Value.Count; i++)
            {
                int index = i;

                VertexAttribute attribute = _entry.Value[i];

                VertexAttributeEditor editor = new(attribute)
                {
                    Label = $"Attribute {index}"
                };

                editor.ValueChanged += (_, updatedValue) =>
                {
                    _entry.Value[index] = updatedValue;
                };

                // Main container
                Grid container = new();

                // Editor content
                Border border = new()
                {
                    Padding = new Thickness(5),
                    BorderThickness = new Thickness(1),
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    CornerRadius = new CornerRadius(4),
                    Child = editor
                };

                container.Children.Add(border);

                // Remove button
                Button removeButton = new()
                {
                    Content = "X",
                    Width = 24,
                    Height = 24,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 5, 5, 0),
                    FontWeight = FontWeights.Bold,
                    Background = System.Windows.Media.Brushes.DarkRed,
                    Foreground = System.Windows.Media.Brushes.White,
                    BorderBrush = System.Windows.Media.Brushes.Firebrick
                };

                removeButton.Click += (_, _) =>
                {
                    _entry.Value.RemoveAt(index);
                    RefreshEditors();
                };

                container.Children.Add(removeButton);

                EditorContainer.Children.Add(new Border
                {
                    Margin = new Thickness(0, 0, 0, 10),
                    Child = container
                });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            VertexAttribute defaultAttribute = new(0x02010001);

            _entry.Value.Add(defaultAttribute);

            RefreshEditors();
        }
    }
}