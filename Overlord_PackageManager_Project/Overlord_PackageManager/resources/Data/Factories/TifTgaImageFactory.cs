using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.Tga_Tif;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class TifTgaImageFactory
    {
        public static Entry CreateRawTgaTifTextureData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                30 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public static Entry CreateTgaTifTextureAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new TgaTifTextureData(id, relOffset),
                19 => new ScalarEntry<int>(id, relOffset, BinaryTypes.Int32),
                20 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                21 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                32 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                33 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}