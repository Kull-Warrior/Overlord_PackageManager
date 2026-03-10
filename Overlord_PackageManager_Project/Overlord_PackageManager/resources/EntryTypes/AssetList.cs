using Overlord_PackageManager.resources.EntryTypes.Animation;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.Tga_Tif;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    public class AssetList(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected static Func<BinaryReader, uint, uint, long, Entry> Factory => Entry.AssetListDictionary;

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start + PayloadOffset;
            Table = new ReferenceTable();
            Table.TableEndOffset = end;
            Table.ReadHeader(reader);
            Table.ReadAssetListEntryStructure(reader, Factory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
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
