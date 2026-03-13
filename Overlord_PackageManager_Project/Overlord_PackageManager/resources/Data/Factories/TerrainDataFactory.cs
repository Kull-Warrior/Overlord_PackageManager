using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                30 => new Int32Entry(id, relOffset),
                31 => new Int32Entry(id, relOffset),
                33 => new BlobEntry(id, relOffset),
                34 => new StringEntry(id, relOffset),
                35 => new StringEntry(id, relOffset),
                36 => new FloatEntry(id, relOffset),
                37 => new BlobEntry(id, relOffset),
                38 => new SingleByteEntry(id, relOffset),
                39 => new BlobEntry(id, relOffset),
                40 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}