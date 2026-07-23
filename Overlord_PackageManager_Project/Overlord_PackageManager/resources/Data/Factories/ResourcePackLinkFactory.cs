using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Factories
{
    internal class ResourcePackLinkFactory
    {
        public static Entry CreateResourcePackLink(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                30 => new UInt32Entry(id, relOffset),
                31 => new CharCountedArrayEntry(id, relOffset),
                32 => new CharCountedArrayEntry(id, relOffset),
                33 => new ByteEntry(id, relOffset),
                // Add more IDs here
                _ => new ByteArrayEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}