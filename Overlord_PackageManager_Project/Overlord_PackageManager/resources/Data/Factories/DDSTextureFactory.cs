using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class DDSTextureFactory
    {
        public static Entry CreateDDSTexture(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Image width
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // Image height
                22 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Raw image data
                23 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // DDS Format
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateDDSTextureAssetDataContainer(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                20 => new AssetListContainer(id, relOffset),     // List of all dds images making up the entire dds file, meaning the main image and each mipmap
                21 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),     // Unkown
                23 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),     // Unkown
                24 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateDDSTextureAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // FFFF Block unkown use
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Chunk or In-Game Object Name
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // File name
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateReflectionCubeMapAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new DDSTextureAssetDataContainer(id, relOffset),    // Sub reference table containing a int32 and list of dds textures
                19 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),    // FFFF Block unkown use
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // Chunk or In-Game Object Name
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),   // File name
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}