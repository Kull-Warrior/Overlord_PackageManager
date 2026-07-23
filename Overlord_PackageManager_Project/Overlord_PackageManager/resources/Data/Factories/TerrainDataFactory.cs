using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class TerrainDataFactory
    {
        public static Entry CreateTerrainData(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                30 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                31 => new ScalarEntry<uint>(id, relOffset, BinaryTypes.UInt32),
                33 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                34 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                35 => new CountedArrayEntry<char>(id, relOffset, BinaryTypes.Char),
                36 => new ScalarEntry<float>(id, relOffset, BinaryTypes.Float),
                37 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                38 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                39 => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),
                40 => new ScalarEntry<byte>(id, relOffset, BinaryTypes.Byte),
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }
    }
}