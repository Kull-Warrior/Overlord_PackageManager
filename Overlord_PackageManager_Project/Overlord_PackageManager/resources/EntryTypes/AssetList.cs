using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    public class AssetList(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table;

        public ReferenceTable GetReferenceTable() => Table;


        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end);

            foreach (var entry in Table.Entries)
            {
                if (entry is ReflectionCubeMapAsset)
                {
                    ((ReflectionCubeMapAsset)entry).Read(reader, Table.PayloadStartOffset, ReflectionCubeMapAssetDictionary);
                }
                if (entry is DDSTextureAsset)
                {
                    ((DDSTextureAsset)entry).Read(reader, Table.PayloadStartOffset,DDSTextureAssetDictionary);
                }
                if (entry is DDSTextures)
                {
                    ((DDSTextures)entry).Read(reader, Table.PayloadStartOffset, DDSTextureDictionary);
                }
                if (entry is TgaTifTextureAsset)
                {
                    ((TgaTifTextureAsset)entry).Read(reader, Table.PayloadStartOffset, TgaTifTextureAssetDictionary);
                }
                if (entry is SFXAsset)
                {
                    ((SFXAsset)entry).Read(reader, Table.PayloadStartOffset, SFXAssetDictionary);
                }
                if (entry is AnimationAsset)
                {
                    ((AnimationAsset)entry).Read(reader, Table.PayloadStartOffset, AnimationAssetDictionary);
                }
                if (entry is BoneAnimationData)
                {
                    ((BoneAnimationData)entry).Read(reader, Table.PayloadStartOffset, BoneAnimationDataDictionary);
                }
                if (entry is BlobEntry)
                {
                    entry.Read(reader, Table.PayloadStartOffset);
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
