using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.XML
{
    public partial class XMLTextEditor : UserControl
    {
        public event Action<string>? XmlChanged;

        public XMLTextEditor()
        {
            InitializeComponent();
        }

        public void SetText(string xml)
        {
            XmlBox.Text = xml;
        }

        private void XmlBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            XmlChanged?.Invoke(XmlBox.Text);
        }
    }
}
