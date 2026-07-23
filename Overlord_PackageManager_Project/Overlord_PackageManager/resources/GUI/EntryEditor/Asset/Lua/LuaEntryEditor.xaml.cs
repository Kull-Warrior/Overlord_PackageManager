using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth;
using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Lua
{
    public partial class LuaEntryEditor : UserControl
    {
        private readonly TableEntry _entry;

        private CharCountedArrayEntry? _nameEntry;
        private CharListCountedArrayEntry _luaTextEntry;
        private UInt32Entry _bytecodeLengthEntry;
        private ByteArrayEntry _bytecodeEntry;

        private CharListCountedArrayEntry _currentTextEntry;

        private StringCountedListEntryEditor _textEditor;
        private string _lastCompiledText = "";

        public LuaEntryEditor(TableEntry entry)
        {
            InitializeComponent();

            _entry = entry;

            ResolveEntries();
            BuildUI();
            Unloaded += LuaEntryEditor_Unloaded;
        }

        private void LuaEntryEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            CompileLua();
        }

        private void ResolveEntries()
        {
            foreach (var e in _entry.Table.Entries)
            {
                switch (e)
                {
                    case CharCountedArrayEntry c:
                        _nameEntry = c;
                        break;

                    case CharListCountedArrayEntry cl:
                        _luaTextEntry = cl;
                        break;

                    case UInt32Entry i:
                        _bytecodeLengthEntry = i;
                        break;

                    case ByteArrayEntry b:
                        _bytecodeEntry = b;
                        break;
                }
            }
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            CompileLua();
        }

        private void UpdateExportState()
        {
            ExportButton.IsEnabled = !string.IsNullOrWhiteSpace(GetLuaText());
        }

        private CharListCountedArrayEntry CreateTemporaryEntry()
        {
            return new CharListCountedArrayEntry(21, 0)
            {
                Value = new List<char[]>()
            };
        }

        private void CreateLuaEntries()
        {
            _luaTextEntry = new CharListCountedArrayEntry(21, 0);
            _bytecodeLengthEntry = new UInt32Entry(22, (uint)_entry.Table.Entries.Sum(e => e.PayloadLength));
            _bytecodeEntry = new ByteArrayEntry(23, (uint)_entry.Table.Entries.Sum(e => e.PayloadLength));

            _entry.Table.Entries.Add(_luaTextEntry);
            _entry.Table.Entries.Add(_bytecodeLengthEntry);
            _entry.Table.Entries.Add(_bytecodeEntry);
        }

        private void RemoveLuaEntries()
        {
            if (_luaTextEntry == null)
            {
                return;
            }

            _entry.Table.Entries.Remove(_luaTextEntry);
            _entry.Table.Entries.Remove(_bytecodeLengthEntry);
            _entry.Table.Entries.Remove(_bytecodeEntry);

            _luaTextEntry = null;
            _bytecodeLengthEntry = null;
            _bytecodeEntry = null;

            _currentTextEntry = CreateTemporaryEntry();
            _textEditor.AttachEntry(_currentTextEntry);
        }

        private void TextChanged()
        {
            if (string.IsNullOrWhiteSpace(GetLuaText()))
            {
                RemoveLuaEntries();
                UpdateExportState();
                return;
            }


            if (_luaTextEntry == null)
            {
                CreateLuaEntries();
                _textEditor.AttachEntry(_luaTextEntry);
                _currentTextEntry = _luaTextEntry;
            }

            UpdateExportState();
        }

        private void BuildUI()
        {
            RootPanel.Children.Clear();

            if (_nameEntry != null)
            {
                RootPanel.Children.Add(new StringEntryEditor(_nameEntry)
                {
                    Label = "File Name"
                });
            }

            if (_luaTextEntry != null)
            {
                _currentTextEntry = _luaTextEntry;
            }
            else
            {
                _currentTextEntry = CreateTemporaryEntry();
            }

            _textEditor = new StringCountedListEntryEditor(_currentTextEntry);
            _textEditor.HideFileButtons();
            _textEditor.TextChangedExternally += TextChanged;
            _textEditor.LostFocus += TextEditor_LostFocus;

            RootPanel.Children.Add(_textEditor);
            UpdateExportState();
        }

        private string GetLuaText()
        {
            if (_currentTextEntry?.Value == null)
            {
                return string.Empty;
            }

            return string.Join("\n", _currentTextEntry.Value.Select(line => new string(line)));
        }

        private void CompileLua()
        {
            string text = GetLuaText();

            if (text == _lastCompiledText)
                return;

            _lastCompiledText = text;

            string tempDir = Path.Combine(Path.GetTempPath(), "OverlordLua");

            Directory.CreateDirectory(tempDir);

            string luaFile = Path.Combine(tempDir, "script.lua");
            string bytecodeFile = Path.Combine(tempDir, "script.luac");

            File.WriteAllText(luaFile, GetLuaText(), Encoding.ASCII);

            string compiler = EnsureLuaTools();

            ProcessStartInfo psi = new()
            {
                FileName = compiler,
                Arguments = $"-o \"{bytecodeFile}\" \"{luaFile}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = tempDir
            };

            using Process p = Process.Start(psi);

            p.WaitForExit();

            if (!File.Exists(bytecodeFile) || p.ExitCode != 0)
            {
                MessageBox.Show("Lua compilation failed.");
                return;
            }

            byte[] bytecode = File.ReadAllBytes(bytecodeFile);

            _bytecodeEntry.Value = bytecode;
            _bytecodeLengthEntry.Value = (uint)bytecode.Length;
        }

        private void ExtractIfMissing(string resourceName, string outputPath)
        {
            if (File.Exists(outputPath))
                return;

            using Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            using FileStream fs = File.Create(outputPath);

            s.CopyTo(fs);
        }

        private string EnsureLuaTools()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "OverlordLua");

            Directory.CreateDirectory(tempDir);

            string compilerPath = Path.Combine(tempDir, "luac50.exe");
            string dllPath = Path.Combine(tempDir, "lua50.dll");

            ExtractIfMissing("Overlord_PackageManager.resources.Tools.luac50.exe", compilerPath);
            ExtractIfMissing("Overlord_PackageManager.resources.Tools.lua50.dll", dllPath);

            return compilerPath;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();

            dialog.Filter = "Lua Files (*.lua)|*.lua";

            if (dialog.ShowDialog() != true)
                return;

            string text = File.ReadAllText(dialog.FileName);

            _textEditor.SetText(text);

            if (_nameEntry != null)
                _nameEntry.Value = Path.GetFileName(dialog.FileName).ToCharArray();

            CompileLua();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();

            dialog.Filter = "Lua Files (*.lua)|*.lua";

            if (_nameEntry != null)
                dialog.FileName = new string(_nameEntry.Value);

            if (dialog.ShowDialog() != true)
                return;

            File.WriteAllText(dialog.FileName, GetLuaText());
        }
    }
}