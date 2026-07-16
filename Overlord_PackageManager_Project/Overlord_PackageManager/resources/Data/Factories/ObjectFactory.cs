using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    public abstract class ObjectFactory
    {
        public static Entry CreateObjectAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                01 => new BlobEntry(id, relOffset),
                19 => new Int32Entry(id, relOffset),
                20 => new StringEntry(id, relOffset),
                21 => new StringEntry(id, relOffset),
                30 => new TableEntry(id, relOffset),
                32 => new FloatArrayEntry(id, relOffset),
                33 => new TableEntry(id, relOffset),
                34 => new FloatArrayEntry(id, relOffset),
                35 => new TableEntry(id, relOffset),
                36 => new TableEntry(id, relOffset),

                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}