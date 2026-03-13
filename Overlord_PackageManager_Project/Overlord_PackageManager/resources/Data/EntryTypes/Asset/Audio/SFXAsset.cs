using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio
{
    public class SFXAsset(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier)
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => SFXAssetDictionary;

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
