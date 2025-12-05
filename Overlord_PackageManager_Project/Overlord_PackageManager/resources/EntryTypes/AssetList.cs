using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.ReflectionMap;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class AssetList(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes(3);
            varRefTable = new RefTable(reader);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is ReflectionMapTextureAsset)
                {
                    ((ReflectionMapTextureAsset)entry).Read(reader, varRefTable.origin, ReflectionMapTextureAssetDictionary);
                }
                if (entry is DDSTextureAsset)
                {
                    ((DDSTextureAsset)entry).Read(reader, varRefTable.origin,DDSTextureAssetDictionary);
                }
                if (entry is TgaTifTextureAsset)
                {
                    ((TgaTifTextureAsset)entry).Read(reader, varRefTable.origin, TgaTifTextureAssetDictionary);
                }
                if (entry is SFXAsset)
                {
                    ((SFXAsset)entry).Read(reader, varRefTable.origin, SFXAssetDictionary);
                }
            }
        }
    }
}
