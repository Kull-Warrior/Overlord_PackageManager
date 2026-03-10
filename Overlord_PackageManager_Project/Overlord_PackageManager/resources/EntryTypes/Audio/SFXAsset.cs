using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Audio
{
    public class SFXAsset(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => Entry.SFXAssetDictionary;

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start + 4;
            Table = new ReferenceTable(reader, end, EntryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
        }

        public void WriteToFile(string baseDir)
        {
            List<StringEntry> sfxAssetStrings = Table.Entries.OfType<StringEntry>().ToList();
            string rawName = sfxAssetStrings[2].Value;
            string fileName = Path.GetFileName(rawName);
            List<SFXData> sfxData = Table.Entries.OfType<SFXData>().ToList();
            byte[] audioData = ((BlobEntry)sfxData[0].Table.Entries[1]).Value;

            using FileStream fs = File.Open(baseDir + fileName, FileMode.Create);
            using BinaryWriter br = new BinaryWriter(fs);
            {
                br.Write(audioData);
            }
        }
    }
}
