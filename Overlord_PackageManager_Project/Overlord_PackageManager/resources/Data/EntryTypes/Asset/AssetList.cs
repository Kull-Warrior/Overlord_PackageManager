using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.Tga_Tif;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Material;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Object;
using Overlord_PackageManager.resources.Data.Factories;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Asset
{
    public class AssetList(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        protected static Func<BinaryReader, uint, uint, long, Entry> Factory => AssetListFactory.CreateAssetList;

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
                string entryDir;

                switch (entry)
                {
                    case ReflectionCubeMapAsset reflectionCubeMap:
                        entryDir = Path.Combine(baseDir, "ReflectionCubeMap");
                        Directory.CreateDirectory(entryDir);
                        reflectionCubeMap.WriteToFile(entryDir);
                        break;
                    case DDSTextureAsset ddsTexture:
                        entryDir = Path.Combine(baseDir, "Image", "DDS");
                        Directory.CreateDirectory(entryDir);
                        ddsTexture.WriteToFile(entryDir);
                        break;
                    case TgaTifTextureAsset tgaTifTexture:
                        entryDir = Path.Combine(baseDir, "Image");
                        Directory.CreateDirectory(entryDir);
                        tgaTifTexture.WriteToFile(entryDir);
                        break;
                    case SFXAsset sfxAsset:
                        entryDir = Path.Combine(baseDir, "SFX");
                        Directory.CreateDirectory(entryDir);
                        sfxAsset.WriteToFile(entryDir + "\\");
                        break;
                    case AnimationAsset animationAsset:
                    case ObjectAsset objectAsset:
                    case MeshAsset meshAsset:
                    case BumpedDiffuseMaterial bumpedDiffuseMaterial:
                    case DiffuseMaterial diffuseMaterial:
                    case MaskedPBRMaterial maskedPBRMaterial:
                        // NotImplemented
                        break;
                    default:
                        entryDir = Path.Combine(baseDir, "Unknown");
                        Directory.CreateDirectory(entryDir);
                        ((AssetEntry)entry).WriteToFile(entryDir);
                        break;
                }
            }
        }
    }
}
