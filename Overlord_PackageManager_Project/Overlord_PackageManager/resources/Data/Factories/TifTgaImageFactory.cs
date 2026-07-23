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
                30 => new ByteArrayEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }

        public static Entry CreateTgaTifTextureAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                1 => new TgaTifTextureData(id, relOffset),
                19 => new UInt32Entry(id, relOffset),
                20 => new CharCountedArrayEntry(id, relOffset),
                21 => new CharCountedArrayEntry(id, relOffset),
                32 => new UInt32Entry(id, relOffset),
                33 => new UInt32Entry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}