using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Animation;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.Tga_Tif;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class AssetListFactory
    {
        public static Entry CreateAssetListContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new AssetList(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateAssetList(BinaryReader reader, uint id, uint relOffset, long payloadStart)
        {
            long pos = payloadStart + relOffset;

            long saved = reader.BaseStream.Position;
            reader.BaseStream.Position = pos;

            uint typeIdentifier = reader.ReadUInt32();

            reader.BaseStream.Position = saved;

            return typeIdentifier switch
            {
                4259993 => new ReflectionCubeMapAsset(id, relOffset, typeIdentifier),
                4259992 => new TgaTifTextureAsset(id, relOffset, typeIdentifier),
                4259901 => new DDSTextureAsset(id, relOffset, typeIdentifier),
                4259876 => new DDSTextures(id, relOffset, typeIdentifier),
                4259845 => new AnimationAsset(id, relOffset, typeIdentifier),
                4259847 => new BoneAnimationData(id, relOffset, typeIdentifier),
                10551296 => new SFXAsset(id, relOffset, typeIdentifier),
                67109013 => new LuaAsset(id, relOffset, typeIdentifier),
                _ => new BlobEntry(id, relOffset)
            };
        }
    }
}