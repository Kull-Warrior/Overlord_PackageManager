using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
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
            long start = origin + RelOffset;
            long end = start + Length;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelOffset;
            LeadingBytes = reader.ReadBytes(3);
            Table = new ReferenceTable(reader, end);

            foreach (var entry in Table.Entries)
            {
                if (entry is ReflectionCubeMapAsset)
                {
                    ((ReflectionCubeMapAsset)entry).Read(reader, Table.OffsetOrigin, ReflectionCubeMapAssetDictionary);
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
                if (entry is BlobEntry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
            }
        }
        public void WriteToFiles(string baseDir)
        {
            foreach (var entry in Table.Entries)
            {
                if (entry is ReflectionCubeMapAsset)
                {
                    Directory.CreateDirectory(baseDir + "\\ReflectionCubeMap");
                    ((ReflectionCubeMapAsset)entry).WriteToFile(baseDir + "\\ReflectionCubeMap\\");
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
