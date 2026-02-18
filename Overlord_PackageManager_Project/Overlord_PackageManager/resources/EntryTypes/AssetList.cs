using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.ReflectionMap;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class AssetList(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] LeadingBytes;
        public ReferenceTable Table;

        public ReferenceTable GetReferenceTable() => Table;


        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            LeadingBytes = reader.ReadBytes(3);
            Table = new ReferenceTable(reader);

            foreach (var entry in Table.Entries)
            {
                if (entry is ReflectionMapTextureAsset)
                {
                    ((ReflectionMapTextureAsset)entry).Read(reader, Table.OffsetOrigin, ReflectionMapTextureAssetDictionary);
                }
                if (entry is DDSTextureAsset)
                {
                    ((DDSTextureAsset)entry).Read(reader, Table.OffsetOrigin,DDSTextureAssetDictionary);
                }
                if (entry is TgaTifTextureAsset)
                {
                    ((TgaTifTextureAsset)entry).Read(reader, Table.OffsetOrigin, TgaTifTextureAssetDictionary);
                }
                if (entry is SFXAsset)
                {
                    ((SFXAsset)entry).Read(reader, Table.OffsetOrigin, SFXAssetDictionary);
                }
                if (entry is AnimationAsset)
                {
                    ((AnimationAsset)entry).Read(reader, Table.OffsetOrigin, AnimationAssetDictionary);
                }
            }
        }
        public void WriteToFiles(string baseDir)
        {
            foreach (var entry in Table.Entries)
            {
                if (entry is ReflectionMapTextureAsset)
                {
                    Directory.CreateDirectory(baseDir + "\\ReflectionMap");
                    ((ReflectionMapTextureAsset)entry).WriteToFile(baseDir + "\\ReflectionMap\\");
                }
                if (entry is DDSTextureAsset)
                {
                    Directory.CreateDirectory(baseDir + "\\Image");
                    Directory.CreateDirectory(baseDir + "\\Image\\DDS");
                    ((DDSTextureAsset)entry).WriteToFile(baseDir + "\\Image\\DDS\\");
                }
                if (entry is TgaTifTextureAsset)
                {
                    Directory.CreateDirectory(baseDir + "\\Image");
                    ((TgaTifTextureAsset)entry).WriteToFile(baseDir + "\\Image\\");
                }
                if (entry is SFXAsset)
                {
                    Directory.CreateDirectory(baseDir + "\\SFX");
                    ((SFXAsset)entry).WriteToFile(baseDir + "\\SFX\\");
                }
                if (entry is AnimationAsset)
                {
                    //NotImplemented
                }
            }
        }
    }
}
