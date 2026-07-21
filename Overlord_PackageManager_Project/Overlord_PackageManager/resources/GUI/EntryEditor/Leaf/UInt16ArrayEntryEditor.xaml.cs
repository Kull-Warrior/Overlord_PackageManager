using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public class IndexRowViewModel
    {
        public List<IndexViewModel> Indices { get; set; } = new List<IndexViewModel>();
    }

    public class IndexViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly ushort[] _source;
        private readonly int _idx;
        public IndexViewModel(ushort[] s, int i) { _source = s; _idx = i; }

        public ushort Value
        {
            get => _source[_idx];
            set { _source[_idx] = value; OnPropertyChanged(nameof(Value)); }
        }
        public string Label => $"Idx: {_idx}";

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(n));
    }

    public partial class UInt16ArrayEntryEditor : UserControl
    {
        public UInt16ArrayEntryEditor(UInt16ArrayEntry entry)
        {
            InitializeComponent();

            List<IndexRowViewModel> rows = new List<IndexRowViewModel>();
            for (int i = 0; i < entry.Value.Length; i += 3)
            {
                IndexRowViewModel row = new IndexRowViewModel();
                for (int j = 0; j < 3 && (i + j) < entry.Value.Length; j++)
                {
                    row.Indices.Add(new IndexViewModel(entry.Value, i + j));
                }
                rows.Add(row);
            }

            IndexListBox.ItemsSource = rows;
        }
    }
}