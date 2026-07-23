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
                30 => new UInt32Entry(id, relOffset),
                31 => new UInt32Entry(id, relOffset),
                33 => new ByteArrayEntry(id, relOffset),
                34 => new CharCountedArrayEntry(id, relOffset),
                35 => new CharCountedArrayEntry(id, relOffset),
                36 => new FloatEntry(id, relOffset),
                37 => new ByteArrayEntry(id, relOffset),
                38 => new ByteEntry(id, relOffset),
                39 => new ByteArrayEntry(id, relOffset),
                40 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}