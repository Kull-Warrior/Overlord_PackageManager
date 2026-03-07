using Microsoft.Win32;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class SFXAssetEditor : UserControl
    {
        private readonly SFXAsset _asset;
        private MediaPlayer? _mediaPlayer;
        private string? _tempFilePath;
        private double _currentVolume = 1;

        private StringEntryEditor? _tagEditor;
        private StringEntryEditor? _audioNameEditor;
        private StringEntryEditor? _fileNameEditor;

        public SFXAssetEditor(SFXAsset asset)
        {
            InitializeComponent();
            _asset = asset;
            BuildUI();

            Unloaded += SFXAssetEditor_Unloaded;
        }

        private void BuildUI()
        {
            RootPanel.Children.Clear();

            List<StringEntry> stringEntries = _asset.Table.Entries.OfType<StringEntry>().ToList();
            if (stringEntries.Count >= 3)
            {
                _tagEditor = new StringEntryEditor(stringEntries[0]) { Label = "Tag" };
                _audioNameEditor = new StringEntryEditor(stringEntries[1]) { Label = "Audio Name" };
                _fileNameEditor = new StringEntryEditor(stringEntries[2]) { Label = "File Name" };

                RootPanel.Children.Add(_tagEditor);
                RootPanel.Children.Add(_audioNameEditor);
                RootPanel.Children.Add(_fileNameEditor);

                // Playback controls aligned with label
                Grid playbackGrid = new Grid
                {
                    Margin = new Thickness(10, 10, 0, 10)
                };
                playbackGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Play button
                playbackGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Volume slider

                Button playButton = new Button
                {
                    Content = "Play",
                    Width = 60,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                playButton.Click += Play_Click;

                Slider volumeSlider = new Slider
                {
                    Minimum = 0,
                    Maximum = 1,
                    Value = _currentVolume,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 150
                };
                volumeSlider.ValueChanged += VolumeSlider_ValueChanged;

                playbackGrid.Children.Add(playButton);
                Grid.SetColumn(playButton, 0);

                playbackGrid.Children.Add(volumeSlider);
                Grid.SetColumn(volumeSlider, 1);

                RootPanel.Children.Add(playbackGrid);
            }
        }

        private byte[]? GetAudioBytes()
        {
            SFXData? sfxData = _asset.Table.Entries.OfType<SFXData>().FirstOrDefault();
            BlobEntry? blob = sfxData?.Table.Entries.OfType<BlobEntry>().FirstOrDefault();
            return blob?.Data;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            byte[]? audioData = GetAudioBytes();
            if (audioData == null)
            {
                MessageBox.Show("No audio data found.");
                return;
            }

            // Stop player before overwriting temp file
            _mediaPlayer?.Stop();
            _mediaPlayer?.Close();
            _mediaPlayer = null;

            _tempFilePath = Path.Combine(Path.GetTempPath(), $"temp_sfx_{_asset.Id}.wav");
            File.WriteAllBytes(_tempFilePath, audioData);

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(new System.Uri(_tempFilePath));
            _mediaPlayer.Volume = _currentVolume;
            _mediaPlayer.Play();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _currentVolume = e.NewValue;
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Volume = _currentVolume;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            byte[]? audioData = GetAudioBytes();
            if (audioData == null)
            {
                MessageBox.Show("No audio data to export.");
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav",
                FileName = _fileNameEditor?.ValueBox.Text ?? "sound.wav"
            };

            if (saveFile.ShowDialog() != true)
            {
                return;
            }

            File.WriteAllBytes(saveFile.FileName, audioData);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav"
            };

            if (openFile.ShowDialog() != true)
            {
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(openFile.FileName);

            // Stop and close player to release temp file
            _mediaPlayer?.Stop();
            _mediaPlayer?.Close();
            _mediaPlayer = null;

            // Update SFXData
            SFXData? sfxData = _asset.Table.Entries.OfType<SFXData>().FirstOrDefault();
            BlobEntry? blob = sfxData?.Table.Entries.OfType<BlobEntry>().FirstOrDefault();
            Int32Entry? lengthEntry = sfxData?.Table.Entries.OfType<Int32Entry>().LastOrDefault();

            if (sfxData == null || blob == null || lengthEntry == null)
            {
                MessageBox.Show("Invalid SFXData structure.");
                return;
            }

            blob.Data = fileBytes;
            lengthEntry.Value = (uint)fileBytes.Length;

            // Update StringEntryEditors UI (automatically updates varString)
            _fileNameEditor!.ValueBox.Text = Path.GetFileName(openFile.FileName);
            _audioNameEditor!.ValueBox.Text = Path.GetFileNameWithoutExtension(openFile.FileName);
        }

        private void SFXAssetEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaPlayer?.Stop();
                _mediaPlayer?.Close();
                _mediaPlayer = null;

                if (_tempFilePath != null && File.Exists(_tempFilePath))
                {
                    File.Delete(_tempFilePath);
                    _tempFilePath = null;
                }
            }
            catch
            {
                // ignore if file is locked
            }
        }
    }
}