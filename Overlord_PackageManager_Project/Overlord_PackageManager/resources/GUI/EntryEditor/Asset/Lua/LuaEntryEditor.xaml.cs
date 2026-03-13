using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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

        private StringEntry? _nameEntry;
        private StringListEntry _luaTextEntry;
        private Int32Entry _bytecodeLengthEntry;
        private BlobEntry _bytecodeEntry;

        private StringListEntryEditor _textEditor;
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
                    case StringEntry s:
                        _nameEntry = s;
                        break;

                    case StringListEntry sl:
                        _luaTextEntry = sl;
                        break;

                    case Int32Entry i:
                        _bytecodeLengthEntry = i;
                        break;

                    case BlobEntry b:
                        _bytecodeEntry = b;
                        break;
                }
            }
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            CompileLua();
        }

        private void BuildUI()
        {
            RootPanel.Children.Clear();

            if (_nameEntry != null)
            {
                var nameEditor = new StringEntryEditor(_nameEntry)
                {
                    Label = "File Name"
                };

                RootPanel.Children.Add(nameEditor);
            }

            _textEditor = new StringListEntryEditor(_luaTextEntry);

            _textEditor.LostFocus += TextEditor_LostFocus;

            RootPanel.Children.Add(_textEditor);
        }

        private string GetLuaText()
        {
            return string.Join("\n", _luaTextEntry.Value.Select(x => x.Text));
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
                _nameEntry.Value = Path.GetFileName(dialog.FileName);

            CompileLua();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();

            dialog.Filter = "Lua Files (*.lua)|*.lua";

            if (_nameEntry != null)
                dialog.FileName = _nameEntry.Value;

            if (dialog.ShowDialog() != true)
                return;

            File.WriteAllText(dialog.FileName, GetLuaText());
        }
    }
}