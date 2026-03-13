using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                30 => new Int32Entry(id, relOffset),
                31 => new StringEntry(id, relOffset),
                32 => new StringEntry(id, relOffset),
                33 => new SingleByteEntry(id, relOffset),
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
            };
        }
    }
}