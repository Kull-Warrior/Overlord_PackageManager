using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.Data.Interfaces;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.Factories;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS
{
    public abstract class DDSImageAssetBase(uint id, uint relOffset, uint typeIdentifier) : AssetEntry(id, relOffset, typeIdentifier), IFileExportable
    {
        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => DDSTextureFactory.CreateDDSTextureAsset;

        public abstract void ReplaceFromDDS(byte[] fileBytes);
        public abstract void WriteToDDS(Stream output);

        protected AssetList GetTextureList()
        {
            DDSTextureAssetDataContainer? dataContainer = Table.Entries.OfType<DDSTextureAssetDataContainer>().FirstOrDefault();
            AssetListContainer? mipContainer = dataContainer.Table.Entries.OfType<AssetListContainer>().FirstOrDefault();
            AssetList? list = mipContainer.Table.Entries.OfType<AssetList>().FirstOrDefault();
            return list;
        }

        public void WriteToFile(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            StringEntry fileNameEntry = (StringEntry)Table.Entries[1];
            string fileName = fileNameEntry.Value;

            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                fileName += ".dds";

            string fullPath = Path.Combine(directory, fileName);

            using FileStream fs = File.Create(fullPath);
            {
                WriteToDDS(fs);
            }
        }
    }
}