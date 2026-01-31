using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Audio
{
    class SFXAsset(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] identifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            identifier = reader.ReadBytes(4);
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is SFXData)
                {
                    ((SFXData)entry).Read(reader, varRefTable.origin, SFXDataDictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(string baseDir)
        {
            List<StringEntry> sfxAssetStrings = varRefTable.Entries.OfType<StringEntry>().ToList();
            string rawName = sfxAssetStrings[2].varString;
            string fileName = Path.GetFileName(rawName);
            List<SFXData> sfxData = varRefTable.Entries.OfType<SFXData>().ToList();
            byte[] audioData = ((BinaryEntry)sfxData[0].varRefTable.Entries[1]).varBytes;

            using FileStream fs = File.Open(baseDir + fileName, FileMode.Create);
            using BinaryWriter br = new BinaryWriter(fs);
            {
                br.Write(audioData);
            }
        }
    }
}
